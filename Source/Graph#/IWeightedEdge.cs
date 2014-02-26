using QuickGraph;

namespace GraphSharp
{
	public interface IWeightedEdge<TVertex> : IEdge<TVertex>
	{
		double Weight { get; }
	}
}
