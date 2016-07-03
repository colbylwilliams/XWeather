using System;

using UIKit;

namespace XWeather.iOS
{
	public partial class DetailsTvHeader : UIView
	{
		public DetailsTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			var dayForecast = location?.Weather?.forecast?.txt_forecast.forecastday? [0];
			var nightForecast = location?.Weather?.forecast?.txt_forecast.forecastday? [1];

			conditionLabel.Text = $"{dayForecast?.fcttext}\n\nTonight: {nightForecast?.fcttext}";
			locationLabel.Text = location?.Name;
			//wuIcon.Text = null;
		}
	}
}