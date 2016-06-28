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
		UIKit.UIPageControl pageIndicator { get; set; }

		[Outlet]
		UIKit.UIView toolbarView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (toolbarView != null) {
				toolbarView.Dispose ();
				toolbarView = null;
			}

			if (pageIndicator != null) {
				pageIndicator.Dispose ();
				pageIndicator = null;
			}
		}
	}
}
