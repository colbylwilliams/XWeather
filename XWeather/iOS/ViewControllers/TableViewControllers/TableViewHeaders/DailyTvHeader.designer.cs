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
	[Register ("DailyTvHeader")]
	partial class DailyTvHeader
	{
		[Outlet]
		XWeather.iOS.DailyGraphView graphView { get; set; }

		[Outlet]
		UIKit.UILabel locationLabel { get; set; }

		[Outlet]
		UIKit.UIImageView wuIcon { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (locationLabel != null) {
				locationLabel.Dispose ();
				locationLabel = null;
			}

			if (wuIcon != null) {
				wuIcon.Dispose ();
				wuIcon = null;
			}

			if (graphView != null) {
				graphView.Dispose ();
				graphView = null;
			}
		}
	}
}
