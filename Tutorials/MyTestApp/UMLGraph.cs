using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using GraphSharp.Controls;
using MyTestApp.ViewModels;

namespace MyTestApp
{
    public class UMLGraph : BidirectionalGraph<ClassViewModel, IEdge<ClassViewModel>>
    {
        public UMLGraph() { }

        public UMLGraph(bool allowParallelEdges)
            : base(allowParallelEdges) { }

        public UMLGraph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity) { }
    }

    public class UMLGraphLayout : GraphLayout<ClassViewModel, IEdge<ClassViewModel>, UMLGraph> { }

}
