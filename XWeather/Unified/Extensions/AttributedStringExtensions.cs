using System;

using Foundation;

#if __IOS__

using UIKit;

#endif

namespace XWeather.Unified
{
	public static class AttributedStringExtensions
	{

#if __IOS__

		public static UIStringAttributes SearchResultStringAttributes = new UIStringAttributes {
			Font = UIFont.PreferredBody,
			ForegroundColor = Colors.SearchResultColor
		};


		public static UIStringAttributes SearchResultHighlightStringAttributes = new UIStringAttributes {
			Font = UIFont.PreferredBody,
			ForegroundColor = Colors.SearchResultHighlightColor
		};


		public static NSMutableAttributedString GetSearchResultAttributedString (this string result, string searchString)
		{
			var attrString = new NSMutableAttributedString (result);

			attrString.AddAttributes (SearchResultStringAttributes, new NSRange (0, result.Length));

			var index = result.IndexOf (searchString, StringComparison.OrdinalIgnoreCase);

			if (index >= 0 && index + searchString.Length <= result.Length) {

				var range = new NSRange (index, searchString.Length);

				attrString.AddAttributes (SearchResultHighlightStringAttributes, range);
			}

			return attrString;
		}

#endif

	}
}