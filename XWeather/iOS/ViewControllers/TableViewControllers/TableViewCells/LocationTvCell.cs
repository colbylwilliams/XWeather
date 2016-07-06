using System;

using SettingsStudio;

using XWeather.Unified;

namespace XWeather.iOS
{
	public partial class LocationTvCell : BaseTvCell
	{
		public LocationTvCell (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			nameLabel.Text = location.Name;
			timeLabel.Text = location?.LocalTimeString ();
			tempLabel.Text = location?.TempString (Settings.UomTemperature, true);
		}
	}
}