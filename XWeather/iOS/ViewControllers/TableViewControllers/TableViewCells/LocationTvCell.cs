using System;

namespace XWeather.iOS
{
	public partial class LocationTvCell : BaseTvCell
	{
		public LocationTvCell (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			nameLabel.Text = location.Name;
			timeLabel.Text = location.CurrentTime.LocalDateTime.ToShortDateString ();
			tempLabel.Text = Math.Round (location.Conditions?.temp_f ?? 0).ToString ();
		}
	}
}