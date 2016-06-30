using System;

using Foundation;
using UIKit;

namespace XWeather.iOS
{
	public partial class DailyTvHeader : UIView
	{
		public DailyTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			locationLabel.Text = location?.Name; ;
			//wuIcon.Text = null;
		}
	}
}