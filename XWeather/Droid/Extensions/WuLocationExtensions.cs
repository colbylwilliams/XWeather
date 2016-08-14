using System;

namespace XWeather.Droid
{
	public static class WuLocationExtensions
	{
		public static DateTime LocalTime (this WuLocation location)
		{
			var tzId = location?.Location?.tz ?? location?.Conditions?.local_tz_long ?? location?.TodayForecast?.date?.tz_long;

			var now = DateTime.Now;

			if (string.IsNullOrWhiteSpace (tzId)) return now;

			try {

				//var tzInfo = TimeZoneInfo.FindSystemTimeZoneById (location.Location.tz);

				return TimeZoneInfo.ConvertTimeBySystemTimeZoneId (now, tzId);

			} catch (TimeZoneNotFoundException tzEx) {

				System.Diagnostics.Debug.WriteLine (tzEx.Message);
				System.Diagnostics.Debug.WriteLine (tzId);

				return now;

			} catch (Exception ex) {

				System.Diagnostics.Debug.WriteLine (ex.Message);

				return now;
			}
		}


		public static string LocalTimeString (this WuLocation location, bool lowercase = true)
			=> lowercase ? location?.LocalTime ().ToShortTimeString ().ToLower () : location?.LocalTime ().ToShortTimeString ();
	}
}