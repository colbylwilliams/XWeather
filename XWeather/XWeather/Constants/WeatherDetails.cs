namespace XWeather
{
	public static class WeatherDetails
	{
		public static int Count = 11;

		public static string GetLabel (int row)
		{
			switch (row) {
				case 0: return "Sunrise";
				case 1: return "Sunset";
				case 2: return "Chance of Rain";
				case 3: return "Humidity";
				case 4: return "Wind";
				case 5: return "Gust";
				case 6: return "Feels Like";
				case 7: return "Precipitation";
				case 8: return "Pressure";
				case 9: return "Visibility";
				case 10: return "UV Index";
				default: return string.Empty;
			}
		}

		public static string GetValue (int row, WuLocation location)
		{
			if (location?.Conditions == null) return string.Empty;

			switch (row) {
				case 0: return location.Sunrise.LocalDateTime.ToString ("t");
				case 1: return location.Sunset.LocalDateTime.ToString ("t");
				case 2: return $"{location?.Forecasts? [0].pop}%";
				case 3: return location.Conditions.relative_humidity;
				case 4: return $"{location.Conditions.wind_dir} {location.Conditions.wind_mph} mph";
				case 5: return $"{location.Conditions.wind_dir} {location.Conditions.wind_gust_mph} mph";
				case 6: return location.Conditions.feelslike_f.WithDegreeSymbol ();
				case 7: return $"{location.Conditions.precip_today_in} in";
				case 8: return $"{location.Conditions.pressure_in} inHg";
				case 9: return $"{location.Conditions.visibility_mi} mi";
				case 10: return location.Conditions.UV.ToString ();
				default: return string.Empty;
			}
		}
	}
}