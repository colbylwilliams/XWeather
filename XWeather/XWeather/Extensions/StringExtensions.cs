using System;

namespace XWeather
{
	public static class StringExtensions
	{
		public static string ToDegreesString (this int val) => $"{val}°";

		public static string ToDegreesString (this double val) => $"{val}°";

		public static string ToPercentString (this int val) => $"{val}%";

		public static string ToPercentString (this double val) => $"{val}%";

		public static string ToUnitString (this int val, string unit) => $"{val} {unit}";

		public static string ToUnitString (this double val, string unit) => $"{val} {unit}";

		public static string AppendColon (this string val) => $"{val}:";
	}
}