using System;

using NotificationCenter;
using Foundation;

using UIKit;
using CoreGraphics;

namespace XWeather.iOS.Today
{
	public partial class TodayViewController : UIViewController, INCWidgetProviding
	{
		// Note: this .ctor should not contain any initialization logic.
		protected TodayViewController (IntPtr handle) : base (handle) { }


		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Tell widget it can be expanded
			ExtensionContext.SetWidgetLargestAvailableDisplayMode (NCWidgetDisplayMode.Expanded);

			// Get the maximum size
			var maxSize = ExtensionContext.GetWidgetMaximumSize (NCWidgetDisplayMode.Expanded);

			Console.WriteLine (maxSize);

			// Do any additional setup after loading the view.
		}


		[Export ("widgetPerformUpdateWithCompletionHandler:")]
		public void WidgetPerformUpdate (Action<NCUpdateResult> completionHandler)
		{
			// Perform any setup necessary in order to update the view.

			// Take action based on the display mode
			switch (ExtensionContext.GetWidgetActiveDisplayMode ()) {
				case NCWidgetDisplayMode.Compact:
					//Content.Text = "Let's Monkey About!";
					break;
				case NCWidgetDisplayMode.Expanded:
					//Content.Text = "Gorilla!!!!";
					break;
			}

			// Report results
			// If an error is encoutered, use NCUpdateResultFailed
			// If there's no update required, use NCUpdateResultNoData
			// If there's an update, use NCUpdateResultNewData

			completionHandler (NCUpdateResult.NewData);
		}

		[Export ("widgetActiveDisplayModeDidChange:withMaximumSize:")]
		public void WidgetActiveDisplayModeDidChange (NCWidgetDisplayMode activeDisplayMode, CGSize maxSize)
		{
			// Take action based on the display mode
			switch (activeDisplayMode) {
				case NCWidgetDisplayMode.Compact:
					PreferredContentSize = maxSize;
					//Content.Text = "Let's Monkey About!";
					break;
				case NCWidgetDisplayMode.Expanded:
					PreferredContentSize = new CGSize (0, 200);
					//Content.Text = "Gorilla!!!!";
					break;
			}
		}

	}
}