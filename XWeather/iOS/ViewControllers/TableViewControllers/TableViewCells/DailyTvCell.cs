using System;

using UIKit;

using SettingsStudio;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class DailyTvCell : BaseTvCell
	{
		public DailyTvCell (IntPtr handle) : base (handle) { }

		public void SetData (ForecastDay forecast)
		{
			dayLabel.Text = forecast.date.weekday;
			precipLabel.Text = forecast.ProbabilityPercipString ();
			highTempLabel.Text = forecast.HighTempString (Settings.UomTemperature, true, true);
			lowTempLabel.Text = forecast.LowTempString (Settings.UomTemperature, true, true);
			iconImageView.Image = UIImage.FromBundle (forecast.icon);
		}
	}
}