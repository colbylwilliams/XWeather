using System;

using UIKit;

namespace XWeather.iOS
{
	public partial class HourlyTvHeader : UIView
	{
		public HourlyTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location, TemperatureUnits units)
		{
			conditionLabel.Text = location?.Conditions?.weather;
			dayLabel.Text = DateTime.Today.DayOfWeek.ToString ();
			highTempLabel.Text = location?.TodayForecast.HighTempString (units);
			locationLabel.Text = location?.Name;
			lowTempLabel.Text = location?.TodayForecast.LowTempString (units);
			precipLabel.Text = location?.TodayForecast?.pop.ToPercentString ();
			tempLabel.Text = location?.Conditions.TempString (units, true);
			todayLabel.Text = "Today";

			//precipIcon.Text = null;
			//wuIcon.Text = null;
		}
	}
}