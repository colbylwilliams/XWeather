using System;

namespace XWeather
{
	public static class StringExtensions
	{
		public static string WithDegreeSymbol (this int val) => $"{val}°";

		public static string WithDegreeSymbol (this double val) => $"{val}°";
	}
}