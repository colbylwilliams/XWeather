using Foundation;

#if __IOS__

using NSImage = UIKit.UIImage;

#else

using AppKit;

#endif

namespace XWeather.Unified
{
	public static class ImageExtensions
	{
		public static NSImage GetImageFromBytes (this byte [] imageData)
		{
			if (imageData != null) {
				NSData data = NSData.FromArray (imageData);
				return new NSImage (data);
			}
			return null;
		}
	}
}