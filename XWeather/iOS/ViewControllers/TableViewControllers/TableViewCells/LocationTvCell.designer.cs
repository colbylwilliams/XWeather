// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace XWeather.iOS
{
	[Register ("LocationTvCell")]
	partial class LocationTvCell
	{
		[Outlet]
		UIKit.UILabel nameLabel { get; set; }

		[Outlet]
		UIKit.UILabel tempLabel { get; set; }

		[Outlet]
		UIKit.UILabel timeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (nameLabel != null) {
				nameLabel.Dispose ();
				nameLabel = null;
			}

			if (tempLabel != null) {
				tempLabel.Dispose ();
				tempLabel = null;
			}

			if (timeLabel != null) {
				timeLabel.Dispose ();
				timeLabel = null;
			}
		}
	}
}
