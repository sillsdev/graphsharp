using QuickGraph;

namespace GraphSharp
{
	public enum EdgeTypes
	{
		General,
		Hierarchical
	}

	public interface ITypedEdge<TVertex> : IEdge<TVertex>
	{
		EdgeTypes Type { get; }
	}
}