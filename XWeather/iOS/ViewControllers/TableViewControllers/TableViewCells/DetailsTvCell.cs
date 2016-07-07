using System;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class DetailsTvCell : BaseTvCell
	{
		public DetailsTvCell (IntPtr handle) : base (handle) { }

		public void SetData (WeatherDetail detail)
		{
			itemLabel.Text = detail.DetailLabel.AppendColon ();
			valueLabel.Text = detail.DetailValue;
		}
	}
}