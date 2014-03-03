using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using QuickGraph;
using QuickGraph.Algorithms;

namespace GraphSharp.Algorithms.EdgeRouting
{
	public class BundleEdgeRoutingAlgorithm<TVertex, TEdge, TGraph> : AlgorithmBase<TGraph>, IEdgeRoutingAlgorithm<TVertex, TEdge, TGraph>
		where TVertex : class
		where TEdge : IEdge<TVertex>
		where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
	{
		private readonly IDictionary<TVertex, Point> _vertexPositions;
		private readonly IDictionary<TVertex, Size> _vertexSizes;
		private readonly Dictionary<TEdge, Point[]> _edgeRoutes;
		private readonly BundleEdgeRoutingParameters _parameters;

		public BundleEdgeRoutingAlgorithm(TGraph visitedGraph, IDictionary<TVertex, Point> vertexPositions, IDictionary<TVertex, Size> vertexSizes, BundleEdgeRoutingParameters parameters)
			: base(visitedGraph)
		{
			_vertexPositions = vertexPositions;
			_vertexSizes = vertexSizes;
			_parameters = parameters;
			_edgeRoutes = new Dictionary<TEdge, Point[]>();
		}

		public IDictionary<TEdge, Point[]> EdgeRoutes
		{
			get { return _edgeRoutes; }
		}

		protected override void InternalCompute()
		{
			var visibilityGraph = new VisibilityGraph();
			foreach (TVertex vertex in VisitedGraph.Vertices)
			{
				Point pos = _vertexPositions[vertex];
				Size sz = _vertexSizes[vertex];
				var rect = new Rect(new Point(pos.X - (sz.Width / 2), pos.Y - (sz.Height / 2)), sz);
				rect.Inflate(_parameters.VertexMargin, _parameters.VertexMargin);
				visibilityGraph.Obstacles.Add(new Obstacle(rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft));
			}

			foreach (TEdge edge in VisitedGraph.Edges)
			{
				visibilityGraph.SinglePoints.Add(_vertexPositions[edge.Source]);
				visibilityGraph.SinglePoints.Add(_vertexPositions[edge.Target]);
			}

			var vertexPoints = new HashSet<PointVertex>(_vertexPositions.Select(kvp => new PointVertex(kvp.Value)));
			
			visibilityGraph.Compute();
			IUndirectedGraph<PointVertex, Edge<PointVertex>> graph = visibilityGraph.Graph;
			var usedEdges = new HashSet<Edge<PointVertex>>();
			foreach (TEdge edge in VisitedGraph.Edges)
			{
				var pos1 = new PointVertex(_vertexPositions[edge.Source]);
				var pos2 = new PointVertex(_vertexPositions[edge.Target]);
				TryFunc<PointVertex, IEnumerable<Edge<PointVertex>>> paths = graph.ShortestPathsDijkstra(e => GetWeight(vertexPoints, usedEdges, pos1, pos2, e), pos1);
				IEnumerable<Edge<PointVertex>> path;
				if (paths(pos2, out path))
				{
					var edgeRoute = new List<Point>();
					bool first = true;
					PointVertex point = pos1;
					foreach (Edge<PointVertex> e in path)
					{
						if (!first)
							edgeRoute.Add(point.Point);
						usedEdges.Add(e);
						point = e.GetOtherVertex(point);
						first = false;
					}
					_edgeRoutes[edge] = edgeRoute.ToArray();
				}
			}
		}

		protected virtual double GetWeight(HashSet<PointVertex> vertexPoints, HashSet<Edge<PointVertex>> usedEdges, PointVertex pos1, PointVertex pos2, Edge<PointVertex> edge)
		{
			if (vertexPoints.Contains(edge.Source) && (!edge.Source.Equals(pos1) && !edge.Source.Equals(pos2)))
				return double.PositiveInfinity;

			if (vertexPoints.Contains(edge.Target) && (!edge.Target.Equals(pos1) && !edge.Target.Equals(pos2)))
				return double.PositiveInfinity;

			double length = Math.Sqrt(Math.Pow(edge.Source.Point.X - edge.Target.Point.X, 2) + Math.Pow(edge.Source.Point.Y - edge.Target.Point.Y, 2));
			double ink = usedEdges.Contains(edge) ? 0 : length;
			return (_parameters.InkCoefficient * ink) + (_parameters.LengthCoefficient * length);
		}
	}
}
