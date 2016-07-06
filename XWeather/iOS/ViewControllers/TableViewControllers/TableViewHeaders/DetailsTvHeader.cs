using System;

using UIKit;

using SettingsStudio;

namespace XWeather.iOS
{
	public partial class DetailsTvHeader : UIView
	{
		public DetailsTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			conditionLabel.Text = location?.ForecastString (Settings.UomTemperature);
			locationLabel.Text = location?.Name;
			//wuIcon.Text = null;
		}
	}
}