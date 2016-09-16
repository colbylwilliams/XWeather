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
			periodLabel.Text = forecast.PeriodString ();
			tempLabel.Text = forecast.TempString (Settings.UomTemperature, true, true);
			precipLabel.Text = forecast.ProbabilityPercipString ();
			iconImageView.Image = UIImage.FromBundle (forecast.icon);
		}
	}
}