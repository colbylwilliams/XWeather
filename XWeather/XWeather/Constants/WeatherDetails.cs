namespace XWeather
{
	public static class WeatherDetails
	{
		public static int Count = 11;

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

		public static string GetValue (int row, WuLocation location, TemperatureUnits units)
		{
			if (location?.Conditions == null) return string.Empty;

			switch (row) {
				case 0: return location.Conditions.FeelsLikeString (units, true, true);
				case 1: return location.Sunrise.LocalDateTime.ToString ("t").ToLower ();
				case 2: return location.Sunset.LocalDateTime.ToString ("t").ToLower ();
				case 3: return $"{location?.Forecasts? [0].pop}%";
				case 4: return location.Conditions.relative_humidity;
				case 5: return $"{location.Conditions.wind_dir} {location.Conditions.wind_mph} mph";
				case 6: return $"{location.Conditions.wind_dir} {location.Conditions.wind_gust_mph} mph";
				case 7: return $"{location.Conditions.precip_today_in} in";
				case 8: return $"{location.Conditions.pressure_in} inHg";
				case 9: return $"{location.Conditions.visibility_mi} mi";
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