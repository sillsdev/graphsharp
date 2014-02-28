using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using QuickGraph;

namespace GraphSharp.Algorithms.Layout.Contextual
{
    public class RadialTreeLayoutAlgorithm<TVertex, TEdge, TGraph> : DefaultParameterizedLayoutAlgorithmBase<TVertex, TEdge, TGraph, RadialTreeLayoutParameters>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : IBidirectionalGraph<TVertex, TEdge>
    {
        private readonly IDictionary<TVertex, Size> _vertexSizes;
        private readonly Dictionary<TVertex, int> _leafCounts;
        private readonly TVertex _root;
        private double _slope;

        public RadialTreeLayoutAlgorithm(TGraph visitedGraph, IDictionary<TVertex, Point> vertexPositions, IDictionary<TVertex, Size> vertexSizes,
            RadialTreeLayoutParameters parameters, TVertex selectedVertex)
            : base(visitedGraph, vertexPositions, parameters)
        {
            _vertexSizes = vertexSizes;
            _leafCounts = new Dictionary<TVertex, int>();
            _root = selectedVertex;
        }

        protected override void InternalCompute()
        {
            _leafCounts.Clear();
            CountLeaves(null, _root);

            double denom = 2 * Math.Tan(Math.PI / _leafCounts[_root]);
            double minLen = VisitedGraph.Edges.Where(e => !(e is ILengthEdge<TVertex>) || ((ILengthEdge<TVertex>) e).Length > 0).Min(e => e is ILengthEdge<TVertex> ? ((ILengthEdge<TVertex>) e).Length : 1);
            double minSlope = Parameters.MinimumLength / minLen;
            switch (Parameters.BranchLengthScaling)
            {
                case BranchLengthScaling.MinimizeLabelOverlapAverage:
                    bool first = true;
                    foreach (TVertex v in VisitedGraph.Vertices.Where(v => VisitedGraph.Degree(v) == 1))
                    {
                        Size sz = _vertexSizes[v];
                        TEdge edge = GetEdges(v).First();
                        double x = 1;
                        var lengthEdge = edge as ILengthEdge<TVertex>;
                        if (lengthEdge != null)
                            x = lengthEdge.Length;
                        if (x <= 0.0)
                            continue;
                        double y = sz.Height / denom;
                        double slope = y / x;
                        _slope = first ? slope : (_slope + slope) / 2;
                        first = false;
                    }
                    _slope = Math.Max(minSlope, _slope);
                    break;

                case BranchLengthScaling.MinimizeLabelOverlapMinimum:
                    _slope = double.MaxValue;
                    foreach (TVertex v in VisitedGraph.Vertices.Where(v => VisitedGraph.Degree(v) == 1))
                    {
                        Size sz = _vertexSizes[v];
                        TEdge edge = GetEdges(v).First();
                        double x = 1;
                        var lengthEdge = edge as ILengthEdge<TVertex>;
                        if (lengthEdge != null)
                            x = lengthEdge.Length;
                        if (x <= 0.0)
                            continue;
                        double y = sz.Height / denom;
                        _slope = Math.Min(_slope, y / x);
                    }
                    _slope = Math.Max(minSlope, _slope);
                    break;

                case BranchLengthScaling.FixedMinimumLength:
                    _slope = minSlope;
                    break;
            }

            VertexPositions[_root] = new Point(0, 0);
            CalcPositions(default(TEdge), _root, 2 * Math.PI, 0);
        }

        private void CountLeaves(TVertex parent, TVertex v)
        {
            if (VisitedGraph.Degree(v) == 1)
            {
                _leafCounts[v] = 1;
            }
            else
            {
                int count = 0;
                foreach (TEdge edge in GetEdges(v).Where(e => !e.OtherVertex(v).Equals(parent)))
                {
                    TVertex child = edge.OtherVertex(v);
                    CountLeaves(v, child);
                    count += _leafCounts[child];
                }
                _leafCounts[v] = count;
            }
        }

        private void CalcPositions(TEdge inEdge, TVertex v, double wedgeSize, double wedgeBorderAngle)
        {
            if (!v.Equals(_root))
            {
                TVertex parent = inEdge.OtherVertex(v);
                double angle = wedgeBorderAngle + (wedgeSize / 2);
                Size parentSize = _vertexSizes[parent];
                Size vSize = _vertexSizes[v];
                double len = GetLength(inEdge) + (parentSize.Width / 2) + (vSize.Width / 2);
                double xDelta = Math.Cos(angle) * len;
                double yDelta = Math.Sin(angle) * len;
                Point parentPoint = VertexPositions[parent];
                VertexPositions[v] = new Point(parentPoint.X + xDelta, parentPoint.Y + yDelta);
                double vertexAngle = angle * (180 / Math.PI);
                if (vertexAngle > 90 && vertexAngle <= 270)
                    vertexAngle -= 180;
                VertexAngles[v] = vertexAngle;
            }
            double childWedgeBorderAngle = wedgeBorderAngle;
            foreach (TEdge edge in GetEdges(v).Where(e => !e.Equals(inEdge)))
            {
                TVertex child = edge.GetOtherVertex(v);
                double childWedgeSize = ((double) _leafCounts[child] / _leafCounts[_root]) * (2 * Math.PI);
                CalcPositions(edge, child, childWedgeSize, childWedgeBorderAngle);
                childWedgeBorderAngle += childWedgeSize;
            }
        }

        private double GetLength(TEdge edge)
        {
            double x = 1;
            var lengthEdge = edge as ILengthEdge<TVertex>;
            if (lengthEdge != null)
                x = lengthEdge.Length;
            return Math.Max(_slope * x, Parameters.MinimumLength);
        }

        private IEnumerable<TEdge> GetEdges(TVertex vertex)
        {
            IEnumerable<TEdge> inEdges;
            if (VisitedGraph.TryGetInEdges(vertex, out inEdges))
            {
                foreach (TEdge edge in inEdges)
                    yield return edge;
            }

            IEnumerable<TEdge> outEdges;
            if (VisitedGraph.TryGetOutEdges(vertex, out outEdges))
            {
                foreach (TEdge edge in outEdges)
                    yield return edge;
            }
        }
    }
}
