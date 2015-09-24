using System.ComponentModel;

namespace GraphSharp.Algorithms.Layout
{
	public abstract class LayoutParametersBase : ILayoutParameters
	{
		#region ICloneable Members

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(string propertyName)
		{
			//delegating to the event...
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}