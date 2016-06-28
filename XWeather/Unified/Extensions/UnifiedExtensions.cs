#if __IOS__

using CoreGraphics;
using Foundation;

#else

using AppKit;

#endif

namespace XWeather.Unified
{
	public static class UnifiedExtensions
	{
#if __IOS__

		public static CGRect BoundingRectWithSize (this NSAttributedString attrString, CGSize size, NSStringDrawingOptions options)
			=> attrString.GetBoundingRect (size, options, null);

#else

		public static NSViewController InstantiateViewController (this NSStoryboard storyboard, string name)
			=> storyboard.InstantiateControllerWithIdentifier (name) as NSViewController;

#endif
	}
}