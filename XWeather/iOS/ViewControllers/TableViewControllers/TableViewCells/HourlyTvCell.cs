using System;

using UIKit;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class HourlyTvCell : BaseTvCell
	{
		public HourlyTvCell (IntPtr handle) : base (handle) { }

		public void SetData (HourlyForecast forecast, TemperatureUnits units)
		{
			hourLabel.Text = forecast.FCTTIME.civil;
			tempLabel.Text = forecast.TempString (units, true, true);
			iconImageView.Image = UIImage.FromBundle (forecast.icon);
		}
	}
}