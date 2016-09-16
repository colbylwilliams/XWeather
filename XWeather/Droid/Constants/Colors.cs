using System.Collections.Generic;

using Android.Graphics;

namespace XWeather.Droid
{
	public static class Colors
	{
		public static Color White = Color.White;
		public static Color Clear = Color.Transparent;

		// sRGB
		public static Color ThemeDark = Color.Rgb (77, 82, 80);

		public static Color SearchResultColor = Color.Argb (125, 255, 255, 255);
		public static Color SearchResultHighlightColor = Color.Argb (255, 255, 255, 255);


		public static List<Color []> Gradients = new List<Color []> {
			new Color[] { Color.Rgb (1, 22, 47), Color.Rgb (22, 19, 76) },
			new Color[] { Color.Rgb (1, 22, 47), Color.Rgb (22, 19, 76) },
			new Color[] { Color.Rgb (1, 32, 64), Color.Rgb (44, 5, 84) },
			new Color[] { Color.Rgb (22, 19, 76), Color.Rgb (64, 4, 100) },
			new Color[] { Color.Rgb (44, 5, 84), Color.Rgb (116, 4, 120) },
			new Color[] { Color.Rgb (78, 4, 110), Color.Rgb (218, 78, 80) },
			new Color[] { Color.Rgb (97, 4, 120), Color.Rgb (248, 103, 67) },
			new Color[] { Color.Rgb (116, 4, 120), Color.Rgb (255, 111, 64) },
			new Color[] { Color.Rgb (218, 78, 80), Color.Rgb (255, 141, 64) },
			new Color[] { Color.Rgb (36, 178, 244), Color.Rgb (36, 178, 244) }
		};


		public static int [] ToArray (this Color [] colors)
		{
			return new int [] { colors [0].ToArgb (), colors [1].ToArgb () };
		}
	}
}