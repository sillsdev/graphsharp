using System.ComponentModel;

namespace GraphSharp.Algorithms.Highlight
{
	public class HighlightParameterBase : IHighlightParameters
	{
		#region ICloneable Members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged( string propertyName )
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}