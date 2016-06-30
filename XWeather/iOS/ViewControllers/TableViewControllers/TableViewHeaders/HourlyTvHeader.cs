using System;

using Foundation;
using UIKit;
using System.Linq;

namespace XWeather.iOS
{
	public partial class HourlyTvHeader : UIView
	{
		public HourlyTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			var observation = location?.Weather?.current_observation;
			var forecast = location?.Weather?.forecast?.simpleforecast?.forecastday.FirstOrDefault (f => f.period == 1);

			conditionLabel.Text = observation?.weather;
			dayLabel.Text = DateTime.Today.DayOfWeek.ToString ();
			highTempLabel.Text = forecast?.high?.FahrenheitValue.ToString ();
			locationLabel.Text = location?.Name;
			lowTempLabel.Text = forecast?.low?.FahrenheitValue.ToString ();
			precipLabel.Text = forecast?.pop.ToString (); ;
			tempLabel.Text = Math.Round (observation?.temp_f ?? 0).ToString ();
			todayLabel.Text = "Today";

			//precipIcon.Text = null;
			//wuIcon.Text = null;
		}
	}
}