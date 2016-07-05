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
	[Register ("WeatherPvc")]
	partial class WeatherPvc
	{
		[Outlet]
		UIKit.UIView emptyView { get; set; }

		[Outlet]
		UIKit.UIView loadingContainerView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView loadingIndicatorView { get; set; }

		[Outlet]
		UIKit.UIPageControl pageIndicator { get; set; }

		[Outlet]
		UIKit.UIButton[] toolbarButtons { get; set; }

		[Outlet]
		UIKit.UIView toolbarView { get; set; }

		[Action ("closeClicked:")]
		partial void closeClicked (Foundation.NSObject sender);

		[Action ("settingsClicked:")]
		partial void settingsClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (emptyView != null) {
				emptyView.Dispose ();
				emptyView = null;
			}

			if (loadingContainerView != null) {
				loadingContainerView.Dispose ();
				loadingContainerView = null;
			}

			if (loadingIndicatorView != null) {
				loadingIndicatorView.Dispose ();
				loadingIndicatorView = null;
			}

			if (pageIndicator != null) {
				pageIndicator.Dispose ();
				pageIndicator = null;
			}

			if (toolbarView != null) {
				toolbarView.Dispose ();
				toolbarView = null;
			}
		}
	}
}
