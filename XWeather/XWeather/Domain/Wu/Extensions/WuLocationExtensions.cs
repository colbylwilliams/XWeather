using System;

namespace XWeather
{
	public static class WuLocationExtensions
	{
		public static string GetForecastStrings (this WuLocation location)
		{
			return null;
		}

		//public static DateTime LocalTime (this WuLocation location)
		//{
		//	var offset = (location?.Conditions?.local_tz_offset / 100) ?? 0;

		//	TimeZoneInfo.ConvertTime(DateTime.UtcNow)

		//	return new DateTimeOffset (DateTime.Now, TimeSpan.FromHours (offset)).DateTime;
		//}
	}
}