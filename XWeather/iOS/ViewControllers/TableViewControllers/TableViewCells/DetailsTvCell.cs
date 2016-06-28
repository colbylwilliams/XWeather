using System;

namespace XWeather.iOS
{
	public partial class DetailsTvCell : BaseTvCell
	{
		public DetailsTvCell (IntPtr handle) : base (handle) { }

		public void SetData (string label, string value)
		{
			itemLabel.Text = label;
			valueLabel.Text = value;
		}
	}
}
