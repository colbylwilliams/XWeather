using System;

using CoreAnimation;
using Foundation;
using UIKit;

namespace XWeather.iOS
{
	public static class ViewExtensions
	{

		public static void SetTransparencyMask (this UIView view, nfloat top, nfloat bottom)
		{
			if (top > view.Frame.Height || bottom > view.Frame.Height) {

				view.Layer.Mask = view.visibilityMaskWithLocation (1);

			} else if (top >= 0 && top <= view.Frame.Height) {

				var location = top / view.Frame.Height;

				view.Layer.Mask = view.visibilityMaskWithLocation (location);

			} else if (bottom >= 0 && bottom <= view.Frame.Height) {

				var location = NMath.Abs (1 - (bottom / view.Frame.Height));

				view.Layer.Mask = view.visibilityMaskWithLocation (location, true);

			} else {

				view.Layer.Mask = null;
			}

			view.Layer.MasksToBounds = true;
		}


		static CAGradientLayer visibilityMaskWithLocation (this UIView view, nfloat location, bool reverse = false)
		{
			var mask = new CAGradientLayer ();

			mask.Frame = view.Bounds;

			if (reverse) {

				mask.Colors = new [] { UIColor.FromWhiteAlpha (1, 1).CGColor, UIColor.FromWhiteAlpha (1, 0).CGColor };

			} else {

				mask.Colors = new [] { UIColor.FromWhiteAlpha (1, 0).CGColor, UIColor.FromWhiteAlpha (1, 1).CGColor };
			}

			mask.Locations = new [] { NSNumber.FromNFloat (location), NSNumber.FromNFloat (location) };

			return mask;
		}
	}
}