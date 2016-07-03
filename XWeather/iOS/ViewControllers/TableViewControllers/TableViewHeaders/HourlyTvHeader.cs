using System;

using UIKit;

namespace XWeather.iOS
{
	public partial class HourlyTvHeader : UIView
	{
		public HourlyTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			conditionLabel.Text = location?.Conditions?.weather;
			dayLabel.Text = DateTime.Today.DayOfWeek.ToString ();
			highTempLabel.Text = location?.TodayForecast?.high?.FahrenheitValue.ToString ();
			locationLabel.Text = location?.Name;
			lowTempLabel.Text = location?.TodayForecast?.low?.FahrenheitValue.ToString ();
			precipLabel.Text = location?.TodayForecast?.pop.ToString (); ;
			tempLabel.Text = Math.Round (location.Conditions?.temp_f ?? 0).ToString ();
			todayLabel.Text = "Today";

			//precipIcon.Text = null;
			//wuIcon.Text = null;
		}
	}
}