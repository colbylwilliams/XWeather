// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace XWeather.iOS
{
	[Register ("RadarMapVc")]
	partial class RadarMapVc
	{
		[Outlet]
		UIKit.UIButton closeButton { get; set; }

		[Outlet]
		UIKit.UIVisualEffectView closeVisualEffectView { get; set; }

		[Outlet]
		MapKit.MKMapView mapView { get; set; }

		[Action ("closeClicked:")]
		partial void closeClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (closeButton != null) {
				closeButton.Dispose ();
				closeButton = null;
			}

			if (mapView != null) {
				mapView.Dispose ();
				mapView = null;
			}

			if (closeVisualEffectView != null) {
				closeVisualEffectView.Dispose ();
				closeVisualEffectView = null;
			}
		}
	}
}
