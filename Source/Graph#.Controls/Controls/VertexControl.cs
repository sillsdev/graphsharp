using System.Windows;
using System.Windows.Controls;
using GraphSharp.Helpers;
using System;

namespace GraphSharp.Controls
{
    /// <summary>
    /// Logical representation of a vertex.
    /// </summary>
    public class VertexControl : Control, IPoolObject, IDisposable
    {
        public object Vertex
        {
            get { return GetValue( VertexProperty ); }
            set { SetValue( VertexProperty, value ); }
        }

        public static readonly DependencyProperty VertexProperty =
            DependencyProperty.Register("Vertex", typeof(object), typeof(VertexControl), new UIPropertyMetadata(null));


        public GraphCanvas RootCanvas
        {
            get { return (GraphCanvas)GetValue(RootCanvasProperty); }
            set { SetValue(RootCanvasProperty, value); }
        }

        public static readonly DependencyProperty RootCanvasProperty =
            DependencyProperty.Register("RootCanvas", typeof(GraphCanvas), typeof(VertexControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(VertexControl), new UIPropertyMetadata(0.0));

        public double Angle
        {
            get { return (double) GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        private static readonly DependencyPropertyKey OriginalWidthPropertyKey =
            DependencyProperty.RegisterReadOnly("OriginalWidth", typeof(double), typeof(VertexControl), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty OriginalWidthProperty = OriginalWidthPropertyKey.DependencyProperty;

        public double OriginalWidth
        {
            get { return (double) GetValue(OriginalWidthProperty); }
            private set { SetValue(OriginalWidthPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey OriginalHeightPropertyKey =
            DependencyProperty.RegisterReadOnly("OriginalHeight", typeof(double), typeof(VertexControl), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty OriginalHeightProperty = OriginalHeightPropertyKey.DependencyProperty;

        public double OriginalHeight
        {
            get { return (double) GetValue(OriginalHeightProperty); }
            private set { SetValue(OriginalHeightPropertyKey, value); }
        }

        static VertexControl()
        {
            //override the StyleKey Property
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VertexControl), new FrameworkPropertyMetadata(typeof(VertexControl)));
        }

        public VertexControl()
        {
            SizeChanged += VertexControl_SizeChanged;
        }

        private void VertexControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var border = (FrameworkElement) Template.FindName("Border", this);
            if (border != null)
            {
                OriginalWidth = border.ActualWidth;
                OriginalHeight = border.ActualHeight;
            }
        }

        #region IPoolObject Members

        public void Reset()
        {
            Vertex = null;
        }

        public void Terminate()
        {
            //nothing to do, there are no unmanaged resources
        }

        public event DisposingHandler Disposing;

        public void Dispose()
        {
            if (Disposing != null)
                Disposing(this);
        }

        #endregion
    }
}