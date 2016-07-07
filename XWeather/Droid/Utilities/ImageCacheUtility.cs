//using System;
//using Android.Graphics;
//using XWeather;
//using Android.Content.Res;
//using Android.Content;

//namespace XWeather.Droid
//{
//	public enum ImageCaches
//	{
//		WeatherIcons
//	}

//	public static class ImageCacheUtility
//	{
//		static LruCacheUtility<string, Bitmap> weatherIconCache = LruCacheUtility<string, Bitmap>.Instance (ImageCaches.WeatherIcons, 20);

//		public static Bitmap WeatherIcon (Context context, string icon)
//		{
//			var image = weatherIconCache [icon];

//			if (image != null) return image;

//			image = BitmapFactory.DecodeResource (context.Resources, ResourceForString (icon));

//			weatherIconCache [icon] = image;

//			return image;
//		}

//	}
//}