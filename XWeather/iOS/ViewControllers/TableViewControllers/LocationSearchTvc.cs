using System;

using Foundation;
using UIKit;

namespace XWeather.iOS
{
	public partial class LocationSearchTvc : BaseTvc<LocationSearchTvCell>
	{
		public LocationSearchTvc (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (TableView.ContentInset.Top > 0) return;

			//TableView.ContentInset = new UIEdgeInsets (44.0f, 0.0f, 0.0f, 0.0f);
			//TableView.ContentOffset = new CoreGraphics.CGPoint (0.0f, 44.0f);

			//if (!UIAccessibility.IsReduceTransparencyEnabled) {

			//	TableView.BackgroundColor = UIColor.Clear;
			//	var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Light);
			//	TableView.BackgroundView = new UIVisualEffectView (blur);
			//}

			//TableView.BackgroundView = new UIView { Frame = View.Frame, BackgroundColor = UIColor.Black };
		}
	}
}