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
	[Register ("HourlyTvCell")]
	partial class HourlyTvCell
	{
		[Outlet]
		UIKit.UILabel hourLabel { get; set; }

		[Outlet]
		UIKit.UIImageView iconImageView { get; set; }

		[Outlet]
		UIKit.UILabel periodLabel { get; set; }

		[Outlet]
		UIKit.UILabel precipLabel { get; set; }

		[Outlet]
		UIKit.UILabel tempLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (periodLabel != null) {
				periodLabel.Dispose ();
				periodLabel = null;
			}

			if (hourLabel != null) {
				hourLabel.Dispose ();
				hourLabel = null;
			}

			if (iconImageView != null) {
				iconImageView.Dispose ();
				iconImageView = null;
			}

			if (precipLabel != null) {
				precipLabel.Dispose ();
				precipLabel = null;
			}

			if (tempLabel != null) {
				tempLabel.Dispose ();
				tempLabel = null;
			}
		}
	}
}
