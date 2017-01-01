using System.Collections.Generic;
namespace XWeather.Domain
{
	public static class WeatherDetails
	{
		public static List<WeatherDetail> GetDetails (WuLocation location, TemperatureUnits temp, SpeedUnits speed, LengthUnits length, DistanceUnits distance, PressureUnits pressure)
		{
			var list = new List<WeatherDetail> ();

			for (int i = 0; i < Count; i++) list.Add (GetDetail (i, location, temp, speed, length, distance, pressure));

			return list;
		}

		public static int Count = 11;

		public static WeatherDetail GetDetail (int row, WuLocation location, TemperatureUnits temp, SpeedUnits speed, LengthUnits length, DistanceUnits distance, PressureUnits pressure)
		{
			return new WeatherDetail {
				DetailLabel = GetLabel (row),
				DetailValue = GetValue (row, location, temp, speed, length, distance, pressure),
				IsSectionTop = IsSectionTop (row)
			};
		}


		public static string GetLabel (int row)
		{
			switch (row) {
				case 0: return "Feels Like";
				case 1: return "Sunrise";
				case 2: return "Sunset";
				case 3: return "Chance of Rain";
				case 4: return "Humidity";
				case 5: return "Wind";
				case 6: return "Gust";
				case 7: return "Precipitation";
				case 8: return "Pressure";
				case 9: return "Visibility";
				case 10: return "UV Index";
				default: return string.Empty;
			}
		}


		public static string GetValue (int row, WuLocation location, TemperatureUnits temp, SpeedUnits speed, LengthUnits length, DistanceUnits distance, PressureUnits pressure)
		{
			var conditions = location?.Conditions;

			if (conditions == null) return string.Empty;

			switch (row) {
				case 0: return conditions.FeelsLikeString (temp, true, true);
				case 1: return location.SunriseString ();
				case 2: return location.SunsetString ();
				case 3: return location.ProbabilityPercipString ();
				case 4: return conditions.relative_humidity;
				case 5: return conditions.WindString (speed);
				case 6: return conditions.GustString (speed);
				case 7: return conditions.PrecipTodayString (length);
				case 8: return conditions.PressureString (pressure);
				case 9: return conditions.VisibilityString (distance);
				case 10: return location.Conditions.UV.ToString ();
				default: return string.Empty;
			}
		}


		public static bool IsSectionTop (int row)
		{
			switch (row) {
				case 0:
				case 1:
				case 3:
				case 5:
				case 7:
				case 9: return true;
				default: return false;
			}
		}
	}
}