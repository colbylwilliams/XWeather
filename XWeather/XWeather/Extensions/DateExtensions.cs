using System;

namespace XWeather
{
	public static class DateExtensions
	{
		public static bool IsBetween (this DateTime date, DateTime start, DateTime end) => start < date && date < end;

		public static bool IsOutside (this DateTime date, DateTime start, DateTime end) => start > date || date > end;
	}
}