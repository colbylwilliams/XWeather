using System;

using UIKit;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class DailyTvCell : BaseTvCell
	{
		public DailyTvCell (IntPtr handle) : base (handle) { }

		public void SetData (ForecastDay forecast)
		{
			dayLabel.Text = forecast.date.weekday;
			precipLabel.Text = forecast.pop.ToPercentString ();
			highTempLabel.Text = forecast.high.FahrenheitValue.ToDegreesString ();
			lowTempLabel.Text = forecast.low.FahrenheitValue.ToDegreesString ();
			iconImageView.Image = UIImage.FromBundle (forecast.icon);
		}
	}
}