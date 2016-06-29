using System;
using UIKit;
using CoreAnimation;
using Foundation;

namespace XWeather.iOS
{
	public class BaseTvCell : UITableViewCell
	{
		public BaseTvCell (IntPtr handle) : base (handle) { }

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			BackgroundColor = UIColor.Clear;
		}


		public void SetCellMask (nfloat top, nfloat bottom)
		{
			if (top > Frame.Height || bottom > Frame.Height) {

				Layer.Mask = visibilityMaskWithLocation (1);

			} else if (top >= 0 && top <= Frame.Height) {

				var location = top / Frame.Height;

				Layer.Mask = visibilityMaskWithLocation (location);

			} else if (bottom >= 0 && bottom <= Frame.Height) {

				var location = NMath.Abs (1 - (bottom / Frame.Height));

				Layer.Mask = visibilityMaskWithLocation (location, true);

			} else {

				Layer.Mask = null;
			}

			Layer.MasksToBounds = true;
		}


		CAGradientLayer visibilityMaskWithLocation (nfloat location, bool reverse = false)
		{
			var mask = new CAGradientLayer ();

			mask.Frame = Bounds;

			if (reverse) {

				mask.Colors = new [] { UIColor.FromWhiteAlpha (1, 1).CGColor, UIColor.FromWhiteAlpha (1, 0).CGColor };

			} else {

				mask.Colors = new [] { UIColor.FromWhiteAlpha (1, 0).CGColor, UIColor.FromWhiteAlpha (1, 1).CGColor };
			}

			mask.Locations = new [] { NSNumber.FromNFloat (location), NSNumber.FromNFloat (location) };

			return mask;
		}

		//		void maskCellFromTop:(CGFloat)margin 
		//		{
		//		    Layer..mask = [self visibilityMaskWithLocation:margin/self.frame.size.height];
		//    self.layer.masksToBounds = YES;
		//}

		//- (CAGradientLayer*)visibilityMaskWithLocation:(CGFloat)location {
		//    CAGradientLayer* mask = [CAGradientLayer layer];
		//	mask.frame = self.bounds;
		//    mask.colors = [NSArray arrayWithObjects:(id)[[UIColor colorWithWhite:1 alpha:0]
		//	CGColor], (id)[[UIColor colorWithWhite:1 alpha:1]
		//	CGColor], nil];
		//    mask.locations = [NSArray arrayWithObjects:[NSNumber numberWithFloat:location], [NSNumber numberWithFloat:location], nil];
		//    return mask;
		//}
	}
}