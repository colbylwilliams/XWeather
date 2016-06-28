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
	[Register ("DailyTvCell")]
	partial class DailyTvCell
	{
		[Outlet]
		UIKit.UILabel dayLabel { get; set; }

		[Outlet]
		UIKit.UILabel highTempLabel { get; set; }

		[Outlet]
		UIKit.UIImageView iconImageView { get; set; }

		[Outlet]
		UIKit.UILabel lowTempLabel { get; set; }

		[Outlet]
		UIKit.UILabel precipLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (dayLabel != null) {
				dayLabel.Dispose ();
				dayLabel = null;
			}

			if (highTempLabel != null) {
				highTempLabel.Dispose ();
				highTempLabel = null;
			}

			if (lowTempLabel != null) {
				lowTempLabel.Dispose ();
				lowTempLabel = null;
			}

			if (precipLabel != null) {
				precipLabel.Dispose ();
				precipLabel = null;
			}

			if (iconImageView != null) {
				iconImageView.Dispose ();
				iconImageView = null;
			}
		}
	}
}
