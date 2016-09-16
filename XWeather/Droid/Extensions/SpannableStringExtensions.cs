using System;

using Android.Text;
using Android.Text.Style;

namespace XWeather.Droid
{
	public static class SpannableStringExtensions
	{
		public static SpannableString GetSearchResultSpannableString (this string result, string searchString)
		{
			var spannableString = new SpannableString (result);

			spannableString.SetSpan (new ForegroundColorSpan (Colors.SearchResultColor), 0, result.Length, SpanTypes.ExclusiveExclusive);

			var index = result.IndexOf (searchString, StringComparison.OrdinalIgnoreCase);

			if (index >= 0 && index + searchString.Length <= result.Length) {

				spannableString.SetSpan (new ForegroundColorSpan (Colors.SearchResultHighlightColor), index, searchString.Length, SpanTypes.ExclusiveExclusive);
			}

			return spannableString;
		}
	}
}