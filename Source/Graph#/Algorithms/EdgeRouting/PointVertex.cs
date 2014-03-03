using System;
using System.Windows;

namespace GraphSharp.Algorithms.EdgeRouting
{
    /// <summary>
    /// We use a wrapper class for vertices instead of just the Point class, because
    /// Quickgraph requires that a vertex implement IEquatable for some operations.
    /// </summary>
    public class PointVertex : IEquatable<PointVertex>
    {
        private readonly Point _point;

        public PointVertex(Point point)
        {
            _point = point;
        }

        public Point Point
        {
            get { return _point; }
        }

		public override bool Equals(object obj)
		{
			var otherPoint = obj as PointVertex;
			return otherPoint != null && Equals(otherPoint);
		}

		public bool Equals(PointVertex other)
		{
			return other != null && _point == other._point;
		}

		public override int GetHashCode()
		{
		    return _point.GetHashCode();
		}

		public override string ToString()
		{
		    return _point.ToString();
		}
    }
}
