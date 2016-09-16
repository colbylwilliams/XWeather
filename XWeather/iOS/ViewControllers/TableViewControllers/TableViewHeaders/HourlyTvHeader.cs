using System;

using UIKit;

using SettingsStudio;

namespace XWeather.iOS
{
	public partial class HourlyTvHeader : UIView
	{
		public HourlyTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			conditionLabel.Text = location?.Conditions?.weather;
			dayLabel.Text = DateTime.Today.DayOfWeek.ToString ();
			highTempLabel.Text = location?.HighTempString (Settings.UomTemperature);
			locationLabel.Text = location?.Name;
			lowTempLabel.Text = location?.LowTempString (Settings.UomTemperature);
			precipLabel.Text = location.ProbabilityPercipString ();
			tempLabel.Text = location?.TempString (Settings.UomTemperature, true);
			todayLabel.Text = "Today";

			//precipIcon.Text = null;
			//wuIcon.Text = null;
		}
	}
}