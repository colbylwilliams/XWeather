using System;
using System.Collections.Generic;

using CoreGraphics;

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

		public static UNColor White = UNColor.White;
		public static UNColor Clear = UNColor.Clear;

		// sRGB
		public static UNColor ThemeDark = FromRgba (77f / 255f, 82f / 255f, 80f / 255f, 255f / 255f);

		public static UNColor SearchResultColor = FromRgba (255f / 255f, 255f / 255f, 255f / 255f, 125f / 255f);
		public static UNColor SearchResultHighlightColor = FromRgba (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

		public static UNColor TintGray = FromRgba (138f / 255f, 138f / 255f, 138f / 255f, 255f / 255f);

		public static List<CGColor []> Gradients = new List<CGColor []> {
			new CGColor[] { FromRgba (1f   / 255f, 22f  / 255f, 47f  / 255f, 255f / 255f).CGColor, FromRgba (22f  / 255f, 19f  / 255f, 76f  / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (1f   / 255f, 32f  / 255f, 64f  / 255f, 255f / 255f).CGColor, FromRgba (44f  / 255f, 5f   / 255f, 84f  / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (22f  / 255f, 19f  / 255f, 76f  / 255f, 255f / 255f).CGColor, FromRgba (64f  / 255f, 4f   / 255f, 100f / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (44f  / 255f, 5f   / 255f, 84f  / 255f, 255f / 255f).CGColor, FromRgba (116f / 255f, 4f   / 255f, 120f / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (78f  / 255f, 4f   / 255f, 110f / 255f, 255f / 255f).CGColor, FromRgba (218f / 255f, 78f  / 255f, 80f  / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (97f  / 255f, 4f   / 255f, 120f / 255f, 255f / 255f).CGColor, FromRgba (248f / 255f, 103f / 255f, 67f  / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (97f  / 255f, 4f   / 255f, 120f / 255f, 255f / 255f).CGColor, FromRgba (248f / 255f, 103f / 255f, 67f  / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (116f / 255f, 4f   / 255f, 120f / 255f, 255f / 255f).CGColor, FromRgba (255f / 255f, 111f / 255f, 64f  / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (218f / 255f, 78f  / 255f, 80f  / 255f, 255f / 255f).CGColor, FromRgba (255f / 255f, 141f / 255f, 64f  / 255f, 255f / 255f).CGColor },
			new CGColor[] { FromRgba (36f  / 255f, 178f / 255f, 244f / 255f, 255f / 255f).CGColor, FromRgba (36f  / 255f, 178f / 255f, 244f / 255f, 255f / 255f).CGColor }
		};
	}
}