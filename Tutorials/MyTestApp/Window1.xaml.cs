using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickGraph;
using MyTestApp.ViewModels;
using GraphSharp.Controls;

namespace MyTestApp
{

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            CreateGraphToVisualize();
            this.DataContext = this;
            InitializeComponent();
        }

        private UMLGraph _graphToVisualize;
        public UMLGraph GraphToVisualize
        {
            get
            {
                if (_graphToVisualize == null)
                {
                    CreateGraphToVisualize();
                }
                return _graphToVisualize;
            }
        }

        private void CreateGraphToVisualize()
        {
            var g = new UMLGraph();

            //add the vertices to the graph
            ClassViewModel[] vertices = new ClassViewModel[10];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new ClassViewModel() { Name = "Class " + (i + 1), IsAbstract = i % 3 == 0 };
                g.AddVertex(vertices[i]);
            }

            var rnd = new Random();
            for (int i = 1; i < vertices.Length; i++)
            {
                g.AddEdge(new TaggedEdge<ClassViewModel, string>(vertices[rnd.Next(vertices.Length - 1)], vertices[rnd.Next(vertices.Length - 1)], "hello"));
            }

            _graphToVisualize = g;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.graphLayout.Relayout();
        }
    }
}
