using System;

using UIKit;

using SettingsStudio;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class HourlyTvCell : BaseTvCell
	{
		public HourlyTvCell (IntPtr handle) : base (handle) { }

		public void SetData (HourlyForecast forecast)
		{
			hourLabel.Text = forecast.HourString ();
			tempLabel.Text = forecast.TempString (Settings.UomTemperature, true, true);
			iconImageView.Image = UIImage.FromBundle (forecast.icon);
		}
	}
}