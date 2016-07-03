using System;

using CoreAnimation;
using Foundation;
using UIKit;

namespace XWeather.iOS
{
	public class BaseTvCell : UITableViewCell
	{
		public BaseTvCell (IntPtr handle) : base (handle) { }


		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			BackgroundColor = UIColor.Clear;
		}
	}
}