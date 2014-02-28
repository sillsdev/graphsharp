using System;
using System.Collections.Generic;

namespace GraphSharp.Algorithms.Layout.Simple.Grid
{
	public enum GridHorizontalAlignment
	{
		Left,
		Center,
		Right
	}

	public enum GridVerticalAlignment
	{
		Top,
		Center,
		Bottom
	}

    public class GridLayoutRow
    {
        public bool AutoHeight { get; set; }
        public double Height { get; set; }
    }

    public class GridLayoutColumn
    {
        public bool AutoWidth { get; set; }
        public double Width { get; set; }
    }

    public class GridVertexInfo
    {
        public GridVertexInfo()
        {
            RowSpan = 1;
            ColumnSpan = 1;
			HorizontalAlignment = GridHorizontalAlignment.Center;
			VerticalAlignment = GridVerticalAlignment.Center;
        }

        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
        public GridHorizontalAlignment HorizontalAlignment { get; set; }
        public GridVerticalAlignment VerticalAlignment { get; set; }
    }

    public class GridVertexInfoEventArgs : EventArgs
    {
        private readonly object _vertex;
        private readonly GridVertexInfo _vertexInfo;

        public GridVertexInfoEventArgs(object vertex)
        {
            _vertex = vertex;
            _vertexInfo = new GridVertexInfo();
        }

        public object Vertex
        {
            get { return _vertex; }
        }

        public GridVertexInfo VertexInfo
        {
            get { return _vertexInfo; }
        }
    }

    public class GridLayoutParameters : LayoutParametersBase
    {
        public event EventHandler<GridVertexInfoEventArgs> VertexInfo;

        private readonly List<GridLayoutRow> _rows;
        private readonly List<GridLayoutColumn> _columns;
        private int _gridThickness;

        public GridLayoutParameters()
        {
            _rows = new List<GridLayoutRow>();
            _columns = new List<GridLayoutColumn>();
            _gridThickness = 1;
        }

        public List<GridLayoutRow> Rows
        {
            get { return _rows; }
        }

        public List<GridLayoutColumn> Columns
        {
            get { return _columns; }
        }

        public int GridThickness
        {
            get { return _gridThickness; }
            set
            {
                _gridThickness = value;
                NotifyPropertyChanged("GridThickness");
            }
        }

        internal GridVertexInfo GetVertexInfo(object vertex)
        {
            if (VertexInfo != null)
            {
                var e = new GridVertexInfoEventArgs(vertex);
                VertexInfo(this, e);
                return e.VertexInfo;
            }
            return null;
        }
    }
}
