using QuickGraph;

namespace GraphSharp
{
	public interface ILengthEdge<TVertex> : IEdge<TVertex>
	{
		double Length { get; }
	}
}
