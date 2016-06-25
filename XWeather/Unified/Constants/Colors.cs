using System;

#if __IOS__
using UNColor = UIKit.UIColor;
#else
using UNColor = AppKit.NSColor;
#endif

namespace XWeather.Unified
{

	public static class Colors
	{
		public static UNColor FromRgba (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
#if __IOS__
			return UNColor.FromRGBA (red, green, blue, alpha);
#else
			return UNColor.FromRgba (red, green, blue, alpha);
#endif
		}

		// sRGB
		public static UNColor ThemeDark = FromRgba (77f / 255f, 82f / 255f, 80f / 255f, 255f / 255f);

		public static UNColor SourceBackgroundSelected = FromRgba (51f / 255f, 54f / 255f, 53f / 255f, 255f / 255f);
		public static UNColor SourceBoarderTopSelected = FromRgba (42f / 255f, 43f / 255f, 45f / 255f, 255f / 255f);
		public static UNColor SourceBorderBottomSelected = FromRgba (100f / 255f, 101f / 255f, 102f / 255f, 255f / 255f);

		public static UNColor SourceFontColor = FromRgba (184f / 255f, 186f / 255f, 185f / 255f, 255f / 255f);
		public static UNColor SourceFontColorSelected = FromRgba (248f / 255f, 248f / 255f, 252f / 255f, 255f / 255f);

		public static UNColor MessageLinkColor = FromRgba (43f / 255f, 128f / 255f, 185f / 255f, 255f / 255f);
		public static UNColor MessageColor = FromRgba (44f / 255f, 45f / 255f, 48f / 255f, 255f / 255f);

		public static UNColor Green = UNColor.Green;
		public static UNColor White = UNColor.White;
		public static UNColor Clear = UNColor.Clear;
	}
}