using System;

using UIKit;

namespace XWeather.iOS
{
	public partial class DetailsTvHeader : UIView
	{
		public DetailsTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			conditionLabel.Text = location?.ForecastString;
			locationLabel.Text = location?.Name;
			//wuIcon.Text = null;
		}
	}
}