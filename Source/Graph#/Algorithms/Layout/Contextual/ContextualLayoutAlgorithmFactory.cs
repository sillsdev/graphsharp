using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using GraphSharp.Algorithms.Layout.Simple.Tree;

namespace GraphSharp.Algorithms.Layout.Contextual
{
    public class ContextualLayoutAlgorithmFactory<TVertex, TEdge, TGraph> : IContextualLayoutAlgorithmFactory<TVertex, TEdge, TGraph>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {
        public IEnumerable<string> AlgorithmTypes
        {
            get { return new[] {"DoubleTree", "BalloonTree", "RadialTree"}; }
        }

        public ILayoutAlgorithm<TVertex, TEdge, TGraph> CreateAlgorithm( string newAlgorithmType, ILayoutContext<TVertex, TEdge, TGraph> context, ILayoutParameters parameters )
        {
            var layoutContext = (ContextualLayoutContext<TVertex, TEdge, TGraph>) context;

            switch (newAlgorithmType)
            {
                case "DoubleTree":
                    return new DoubleTreeLayoutAlgorithm<TVertex, TEdge, TGraph>(layoutContext.Graph, layoutContext.Positions, layoutContext.Sizes, parameters as DoubleTreeLayoutParameters, layoutContext.SelectedVertex);
                case "BalloonTree":
                    return new BalloonTreeLayoutAlgorithm<TVertex, TEdge, TGraph>(layoutContext.Graph, layoutContext.Positions, parameters as BalloonTreeLayoutParameters, layoutContext.SelectedVertex);
                case "RadialTree":
                    return new RadialTreeLayoutAlgorithm<TVertex, TEdge, TGraph>(layoutContext.Graph, layoutContext.Positions, layoutContext.Sizes, parameters as RadialTreeLayoutParameters, layoutContext.SelectedVertex);
                default:
                    return null;
            }
        }

        public ILayoutParameters CreateParameters(string algorithmType, ILayoutParameters oldParameters)
        {
            switch (algorithmType)
            {
                case "DoubleTree":
                    return oldParameters.CreateNewParameter<DoubleTreeLayoutParameters>();
                case "BalloonTree":
                    return oldParameters.CreateNewParameter<BalloonTreeLayoutParameters>();
                case "RadialTree":
                    return oldParameters.CreateNewParameter<RadialTreeLayoutParameters>();
                default:
                    return null;
            }
        }

        public string GetAlgorithmType(ILayoutAlgorithm<TVertex, TEdge, TGraph> algorithm)
        {
            if (algorithm == null)
                return string.Empty;

            int index = algorithm.GetType().Name.IndexOf("LayoutAlgorithm", StringComparison.Ordinal);
            if (index == -1)
                return string.Empty;

            string algoType = algorithm.GetType().Name;
            return algoType.Substring(0, algoType.Length - index);
        }

        public bool IsValidAlgorithm(string algorithmType)
        {
            return AlgorithmTypes.Contains(algorithmType);
        }

        public bool NeedEdgeRouting(string algorithmType)
        {
            switch (algorithmType)
            {
                case "DoubleTree":
                case "BallonTree":
                    return true;
            }
            return false;
        }

        public bool NeedOverlapRemoval( string algorithmType )
        {
            return false;
        }
    }
}