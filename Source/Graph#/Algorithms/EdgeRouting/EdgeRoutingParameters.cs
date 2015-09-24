namespace GraphSharp.Algorithms.EdgeRouting
{
	public class EdgeRoutingParameters : IEdgeRoutingParameters
	{
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		protected void NotifyChanged( string propertyName )
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	}
}