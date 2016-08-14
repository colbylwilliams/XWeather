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
	[Register ("LocationTvc")]
	partial class LocationTvc
	{
		[Outlet]
		UIKit.UIView tableHeader { get; set; }

		[Action ("addButtonClicked:")]
		partial void addButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (tableHeader != null) {
				tableHeader.Dispose ();
				tableHeader = null;
			}
		}
	}
}
