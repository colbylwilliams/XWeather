using System;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class HourlyTvCell : BaseTvCell
	{
		public HourlyTvCell (IntPtr handle) : base (handle) { }

		public void SetData (HourlyForecast forecast)
		{
			hourLabel.Text = forecast.FCTTIME.civil;
			tempLabel.Text = forecast.temp.english.ToDegreesString ();
		}
	}
}