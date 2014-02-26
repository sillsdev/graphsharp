using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace GraphSharp.Algorithms.Highlight
{
	public class StandardHighlightAlgorithmFactory<TVertex, TEdge, TGraph> : IHighlightAlgorithmFactory<TVertex, TEdge, TGraph>
		where TVertex : class
		where TEdge : IEdge<TVertex>
		where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
	{
		public IEnumerable<string> HighlightModes
		{
			get { return new[] {"Simple", "Hierarchical", "Undirected"}; }
		}

		public bool IsValidMode( string mode )
		{
			return string.IsNullOrEmpty( mode ) || HighlightModes.Contains( mode );
		}

		public IHighlightAlgorithm<TVertex, TEdge, TGraph> CreateAlgorithm(
			string highlightMode,
			IHighlightContext<TVertex, TEdge, TGraph> context,
			IHighlightController<TVertex, TEdge, TGraph> controller,
			IHighlightParameters parameters )
		{
			switch (highlightMode)
			{
				case "Simple":
					return new SimpleHighlightAlgorithm<TVertex, TEdge, TGraph>(controller, parameters);
				case "Hierarchical":
					return new HierarchicalHighlightAlgorithm<TVertex, TEdge, TGraph>(controller, parameters);
				case "Undirected":
					return new UndirectedHighlightAlgorithm<TVertex, TEdge, TGraph>(controller, parameters as UndirectedHighlightParameters);
				default:
					return null;
			}
		}

		public IHighlightParameters CreateParameters( string highlightMode, IHighlightParameters oldParameters )
		{
			switch (highlightMode)
			{
				case "Simple":
					return new HighlightParameterBase();
				case "Hierarchical":
					return new HighlightParameterBase();
				case "Undirected":
					return oldParameters.CreateNewParameter<UndirectedHighlightParameters>();
				default:
					return new HighlightParameterBase();
			}
		}

		public string GetHighlightMode( IHighlightAlgorithm<TVertex, TEdge, TGraph> algorithm )
		{
            if (algorithm == null)
                return string.Empty;

            int index = algorithm.GetType().Name.IndexOf("HighlightAlgorithm", StringComparison.Ordinal);
            if (index == -1)
                return string.Empty;

            string algoType = algorithm.GetType().Name;
            return algoType.Substring(0, algoType.Length - index);
		}
	}
}