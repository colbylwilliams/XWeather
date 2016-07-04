using System;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using ImageIO;

namespace XWeather.iOS
{
	public static class ImageExtensions
	{
		public static List<CGImage> SplitAnimatedGifImages (this byte [] gifData)
		{
			using (var data = NSData.FromArray (gifData))
			using (var src = CGImageSource.FromData (data)) {

				var images = new List<CGImage> ();

				for (int i = 0; i < src.ImageCount; i++)
					images.Add (src.CreateImage (i, null));

				return images;
			}
		}
	}
}