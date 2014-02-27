using System.Collections.Generic;
using QuickGraph;
using System.Windows;

namespace GraphSharp.Algorithms.Layout
{
    public abstract class LayoutAlgorithmBase<TVertex, TEdge, TGraph, TVertexInfo, TEdgeInfo> : LayoutAlgorithmBase<TVertex, TEdge, TGraph>, ILayoutAlgorithm<TVertex, TEdge, TGraph, TVertexInfo, TEdgeInfo>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        private readonly IDictionary<TVertex, TVertexInfo> _vertexInfos = new Dictionary<TVertex, TVertexInfo>();
        private readonly IDictionary<TEdge, TEdgeInfo> _edgeInfos = new Dictionary<TEdge, TEdgeInfo>();

        protected LayoutAlgorithmBase(TGraph visitedGraph)
            : base(visitedGraph)
        {
        }

        protected LayoutAlgorithmBase(TGraph visitedGraph, IDictionary<TVertex, Point> vertexPositions)
            : base(visitedGraph, vertexPositions)
        {
        }

        public IDictionary<TVertex, TVertexInfo> VertexInfos
        {
            get { return _vertexInfos; }
        }

        public IDictionary<TEdge, TEdgeInfo> EdgeInfos
        {
            get { return _edgeInfos; }
        }

        public override object GetVertexInfo( TVertex vertex )
        {
            TVertexInfo info;
            if ( VertexInfos.TryGetValue( vertex, out info ) )
                return info;

            return null;
        }

        public override object GetEdgeInfo( TEdge edge )
        {
            TEdgeInfo info;
            if ( EdgeInfos.TryGetValue( edge, out info ) )
                return info;

            return null;
        }
    }

    public abstract class LayoutAlgorithmBase<TVertex, TEdge, TGraph> : AlgorithmBase, ILayoutAlgorithm<TVertex, TEdge, TGraph>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        private readonly Dictionary<TVertex, Point> _vertexPositions;
        private readonly TGraph _visitedGraph;
        private readonly Dictionary<TVertex, double> _vertexAngles; 

        public IDictionary<TVertex, Point> VertexPositions
        {
            get { return _vertexPositions; }
        }

        public IDictionary<TVertex, double> VertexAngles
        {
            get { return _vertexAngles; }
        }

        public TGraph VisitedGraph
        {
            get { return _visitedGraph; }
        }

        public event LayoutIterationEndedEventHandler<TVertex> IterationEnded;

        public event ProgressChangedEventHandler ProgressChanged;

        public bool ReportOnIterationEndNeeded
        {
            get { return IterationEnded != null; }
        }

        public bool ReportOnProgressChangedNeeded
        {
            get { return ProgressChanged != null; }
        }

        protected LayoutAlgorithmBase(TGraph visitedGraph) :
            this(visitedGraph, null)
        {
        }

        protected LayoutAlgorithmBase(TGraph visitedGraph, IDictionary<TVertex, Point> vertexPositions)
        {
            _visitedGraph = visitedGraph;
            _vertexPositions = vertexPositions != null ? new Dictionary<TVertex, Point>(vertexPositions) : new Dictionary<TVertex, Point>(visitedGraph.VertexCount);
            _vertexAngles = new Dictionary<TVertex, double>(visitedGraph.VertexCount);
        }

        protected virtual void OnIterationEnded(ILayoutIterationEventArgs<TVertex> args)
        {
            if ( IterationEnded != null )
            {
                IterationEnded( this, args );

                //if the layout should be aborted
                if ( args.Abort )
                    Abort();
            }
        }

        protected virtual void OnProgressChanged( double percent )
        {
            if ( ProgressChanged != null )
                ProgressChanged( this, percent );
        }

        public virtual object GetVertexInfo( TVertex vertex )
        {
            return null;
        }

        public virtual object GetEdgeInfo( TEdge edge )
        {
            return null;
        }
    }
}