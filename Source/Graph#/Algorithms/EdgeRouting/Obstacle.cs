using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphSharp.Algorithms.EdgeRouting
{
	public class Obstacle : IEquatable<Obstacle>
	{
		private readonly List<RotationTreeNode> _nodes;
		private readonly List<ObstacleSegment> _segments;
		private readonly List<Point> _points; 

		public Obstacle(params Point[] points)
			: this((IEnumerable<Point>) points)
		{
		}

		public Obstacle(IEnumerable<Point> points)
		{
			_points = points.ToList();
			_nodes = new List<RotationTreeNode>();
			_segments = new List<ObstacleSegment>();
			RotationTreeNode lastPoint = null;
			foreach (Point point in _points)
			{
				var newPoint = new RotationTreeNode(this, point, false);
				_nodes.Add(newPoint);
				if (lastPoint != null)
					_segments.Add(new ObstacleSegment(this, lastPoint, newPoint));

				lastPoint = newPoint;
			}

			if (_nodes.Count > 2)
				_segments.Add(new ObstacleSegment(this, _nodes[_nodes.Count - 1], _nodes[0]));
		}

		public IList<Point> Points
		{
			get { return _points; }
		}

		internal IList<RotationTreeNode> Nodes
		{
			get { return _nodes; }
		}

		internal IList<ObstacleSegment> Segments
		{
			get { return _segments; }
		}

		public bool Contains(Point p)
		{
			if (_points.Count < 3)
				return false;

			bool inside = false;
			for (int i = 0, j = _points.Count - 1; i < _points.Count; j = i++)
			{
				if (((_points[i].Y > p.Y) != (_points[j].Y > p.Y))
				    && (p.X < (_points[j].X - _points[i].X) * (p.Y - _points[i].Y) / (_points[j].Y - _points[i].Y) + _points[i].X))
				{
					inside = !inside;
				}
			}
			return inside;
		}

		public bool Equals(Obstacle other)
		{
			if (_points.Count != other._points.Count)
				return false;

			int index1 = GetMinIndex();
			int index2 = other.GetMinIndex();
			for (int i = 0; i < _points.Count; i++)
			{
				if (!_points[(i + index1) % _points.Count].Equals(other._points[(i + index2) % _points.Count]))
					return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			var other = obj as Obstacle;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			int index = GetMinIndex();
			int code = 23;
			for (int i = 0; i < _points.Count; i++)
				code = code * 31 + _points[(i + index) % _points.Count].GetHashCode();
			return code;
		}

		private int GetMinIndex()
		{
			RotationTreeNode minNode = null;
			int minIndex = -1;
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (minNode == null || _nodes[i].CompareTo(minNode) < 0)
                {
                    minNode = _nodes[i];
                    minIndex = i;
                }
            }
			return minIndex;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			bool first = true;
			sb.Append("[");
			foreach (Point point in _points)
			{
				if (!first)
					sb.Append(",");
				sb.Append(point);
				first = false;
			}
			sb.Append("]");
			return sb.ToString();
		}
	}
}
