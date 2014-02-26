namespace GraphSharp.Algorithms.Highlight
{
	public class UndirectedHighlightParameters : HighlightParameterBase
	{
		private double _weightFilter;

		public double WeightFilter
		{
			get { return _weightFilter; }
			set
			{
				_weightFilter = value;
				OnPropertyChanged("WeightFilter");
			}
		}
	}
}
