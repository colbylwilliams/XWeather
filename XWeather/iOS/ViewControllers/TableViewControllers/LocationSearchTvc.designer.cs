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
	[Register ("LocationSearchTvc")]
	partial class LocationSearchTvc
	{
		[Outlet]
		UIKit.UIVisualEffectView emptyView { get; set; }

		[Action ("emptyViewClicked:")]
		partial void emptyViewClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (emptyView != null) {
				emptyView.Dispose ();
				emptyView = null;
			}
		}
	}
}
