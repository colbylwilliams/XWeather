using System;

using UIKit;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class DailyTvCell : BaseTvCell
	{
		public DailyTvCell (IntPtr handle) : base (handle) { }

		public void SetData (ForecastDay forecast, TemperatureUnits units)
		{
			dayLabel.Text = forecast.date.weekday;
			precipLabel.Text = forecast.pop.ToPercentString ();
			highTempLabel.Text = forecast.HighTempString (units, true, true);
			lowTempLabel.Text = forecast.LowTempString (units, true, true);
			iconImageView.Image = UIImage.FromBundle (forecast.icon);
		}
	}
}