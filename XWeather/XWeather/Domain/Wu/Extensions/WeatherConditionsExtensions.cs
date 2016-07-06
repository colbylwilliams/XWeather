using System;

using XWeather.Domain;

namespace XWeather
{
	public static class WeatherConditionsExtensions
	{
		static bool isImperial (this TemperatureUnits unit) => unit == TemperatureUnits.Fahrenheit;

		static bool isImperial (this DistanceUnits unit) => unit == DistanceUnits.Miles;

		static bool isImperial (this PressureUnits unit) => unit == PressureUnits.InchesOfMercury;

		static bool isImperial (this LengthUnits unit) => unit == LengthUnits.Inches;

		static bool isImperial (this SpeedUnits unit) => unit == SpeedUnits.MilesPerHour;



		static double getValueInUnits (TemperatureUnits units, double? imperial, double? metric, bool round = false)
			=> getValueInUnits (units.isImperial (), imperial, metric, round);

		static double getValueInUnits (DistanceUnits units, double? imperial, double? metric, bool round = false)
			=> getValueInUnits (units.isImperial (), imperial, metric, round);

		static double getValueInUnits (PressureUnits units, double? imperial, double? metric, bool round = false)
			=> getValueInUnits (units.isImperial (), imperial, metric, round);

		static double getValueInUnits (LengthUnits units, double? imperial, double? metric, bool round = false)
			=> getValueInUnits (units.isImperial (), imperial, metric, round);

		static double getValueInUnits (SpeedUnits units, double? imperial, double? metric, bool round = false)
			=> getValueInUnits (units.isImperial (), imperial, metric, round);



		static double getValueInUnits (bool isImperial, double? imperial, double? metric, bool round = false)
		{
			var val = (isImperial ? imperial : metric) ?? 0;

			return round ? Math.Round (val) : val;
		}



		static string getUnitString (this TemperatureUnits unit) => unit.isImperial () ? "F" : "C";

		static string getUnitString (this DistanceUnits unit) => unit.isImperial () ? "mi" : "km";

		static string getUnitString (this PressureUnits unit) => unit.isImperial () ? "inHg" : "mb";

		static string getUnitString (this LengthUnits unit) => unit.isImperial () ? "in" : "mm";

		static string getUnitString (this SpeedUnits unit) => unit.isImperial () ? "mph" : "kph";



		static string getTemperatureString (double value, bool degreeSymbol = false)
			=> degreeSymbol ? value.ToDegreesString () : value.ToString ();


		static string getWindString (double value, string direction, SpeedUnits unit)
			=> $"{direction} {value} {unit.getUnitString ()}";


		static string getValueStringInUnits (double value, DistanceUnits unit)
			=> $"{value} {unit.getUnitString ()}";


		static string getValueStringInUnits (double value, PressureUnits unit)
			=> $"{value} {unit.getUnitString ()}";


		static string getValueStringInUnits (double value, LengthUnits unit)
			=> $"{value} {unit.getUnitString ()}";



		public static double Temp (this CurrentObservation observation, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, observation?.temp_f, observation?.temp_c, round);


		public static double FeelsLike (this CurrentObservation observation, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, observation?.feelslike_f, observation?.feelslike_c, round);


		public static double DewPoint (this CurrentObservation observation, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, observation?.dewpoint_f, observation?.dewpoint_c, round);



		public static double Wind (this CurrentObservation observation, SpeedUnits units, bool round = false)
			=> getValueInUnits (units, observation?.wind_mph, observation?.wind_kph, round);


		public static double Gust (this CurrentObservation observation, SpeedUnits units, bool round = false)
			=> getValueInUnits (units, observation?.wind_gust_mph, observation?.wind_gust_kph, round);



		public static double PrecipToday (this CurrentObservation observation, LengthUnits units, bool round = false)
			=> getValueInUnits (units, observation?.precip_today_in, observation?.precip_today_metric, round);


		public static double Pressure (this CurrentObservation observation, PressureUnits units, bool round = false)
			=> getValueInUnits (units, observation?.pressure_in, observation?.pressure_mb, round);


		public static double Visibility (this CurrentObservation observation, DistanceUnits units, bool round = false)
			=> getValueInUnits (units, observation?.visibility_mi, observation?.visibility_km, round);



		public static double HighTemp (this ForecastDay forecast, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, forecast?.high?.FahrenheitValue, forecast?.high?.CelsiusValue, round);


		public static double LowTemp (this ForecastDay forecast, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, forecast?.low?.FahrenheitValue, forecast?.low?.CelsiusValue, round);


		public static double Temp (this HourlyForecast forecast, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, forecast?.temp?.english, forecast?.temp?.metric, round);




		public static string TempString (this WuLocation location, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (location.Conditions.Temp (units, round), degreeSymbol);


		public static string TempString (this CurrentObservation observation, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (observation.Temp (units, round), degreeSymbol);


		public static string FeelsLikeString (this CurrentObservation observation, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (observation.FeelsLike (units, round), degreeSymbol);


		public static string DewPointString (this CurrentObservation observation, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (observation.DewPoint (units, round), degreeSymbol);



		public static string WindString (this CurrentObservation observation, SpeedUnits units, bool round = false)
			=> getWindString (observation.Wind (units, round), observation.wind_dir, units);


		public static string GustString (this CurrentObservation observation, SpeedUnits units, bool round = false)
			=> getWindString (observation.Gust (units, round), observation.wind_dir, units);



		public static string PrecipTodayString (this CurrentObservation observation, LengthUnits units, bool round = false)
			=> getValueStringInUnits (observation.PrecipToday (units, round), units);


		public static string PressureString (this CurrentObservation observation, PressureUnits units, bool round = false)
			=> getValueStringInUnits (observation.Pressure (units, round), units);


		public static string VisibilityString (this CurrentObservation observation, DistanceUnits units, bool round = false)
			=> getValueStringInUnits (observation.Visibility (units, round), units);


		public static string HighTempString (this WuLocation location, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (location.TodayForecast.HighTemp (units, round), degreeSymbol);


		public static string HighTempString (this ForecastDay forecast, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (forecast.HighTemp (units, round), degreeSymbol);


		public static string LowTempString (this ForecastDay forecast, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (forecast.LowTemp (units, round), degreeSymbol);


		public static string LowTempString (this WuLocation location, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (location.TodayForecast.LowTemp (units, round), degreeSymbol);


		public static string TempString (this HourlyForecast forecast, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getTemperatureString (forecast.Temp (units, round), degreeSymbol);


		public static string HourString (this HourlyForecast forecast, bool lowercase = true)
			 => lowercase ? forecast.FCTTIME.civil.ToLower () : forecast.FCTTIME.civil;


		public static string ForecastString (this WuLocation location, TemperatureUnits unit)
		{
			var today = location?.TxtForecasts? [0];
			var tonight = location?.TxtForecasts? [1];

			var todayString = unit.isImperial () ? today?.fcttext : today.fcttext_metric;
			var tonightString = unit.isImperial () ? tonight?.fcttext : tonight.fcttext_metric;

			return $"{todayString}\n\nTonight: {tonightString}";
		}


		public static string SunriseString (this WuLocation location) => location?.Sunrise?.LocalDateTime.ToString ("t").ToLower ();

		public static string SunsetString (this WuLocation location) => location?.Sunset?.LocalDateTime.ToString ("t").ToLower ();


		public static string ProbabilityPercipString (this ForecastDay forecast) => forecast?.pop.ToPercentString ();


		public static string ProbabilityPercipString (this WuLocation location) => location?.TodayForecast?.ProbabilityPercipString ();
	}
}