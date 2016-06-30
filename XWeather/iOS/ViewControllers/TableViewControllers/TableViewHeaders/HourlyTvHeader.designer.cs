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
	[Register ("HourlyTvHeader")]
	partial class HourlyTvHeader
	{
		[Outlet]
		UIKit.UILabel conditionLabel { get; set; }

		[Outlet]
		UIKit.UILabel dayLabel { get; set; }

		[Outlet]
		UIKit.UILabel highTempLabel { get; set; }

		[Outlet]
		UIKit.UILabel locationLabel { get; set; }

		[Outlet]
		UIKit.UILabel lowTempLabel { get; set; }

		[Outlet]
		UIKit.UIImageView precipIcon { get; set; }

		[Outlet]
		UIKit.UILabel precipLabel { get; set; }

		[Outlet]
		UIKit.UILabel tempLabel { get; set; }

		[Outlet]
		UIKit.UILabel todayLabel { get; set; }

		[Outlet]
		UIKit.UIImageView wuIcon { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (locationLabel != null) {
				locationLabel.Dispose ();
				locationLabel = null;
			}

			if (conditionLabel != null) {
				conditionLabel.Dispose ();
				conditionLabel = null;
			}

			if (tempLabel != null) {
				tempLabel.Dispose ();
				tempLabel = null;
			}

			if (lowTempLabel != null) {
				lowTempLabel.Dispose ();
				lowTempLabel = null;
			}

			if (highTempLabel != null) {
				highTempLabel.Dispose ();
				highTempLabel = null;
			}

			if (dayLabel != null) {
				dayLabel.Dispose ();
				dayLabel = null;
			}

			if (todayLabel != null) {
				todayLabel.Dispose ();
				todayLabel = null;
			}

			if (precipLabel != null) {
				precipLabel.Dispose ();
				precipLabel = null;
			}

			if (precipIcon != null) {
				precipIcon.Dispose ();
				precipIcon = null;
			}

			if (wuIcon != null) {
				wuIcon.Dispose ();
				wuIcon = null;
			}
		}
	}
}
