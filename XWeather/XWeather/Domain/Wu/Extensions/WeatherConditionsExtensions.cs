using System;

using XWeather.Domain;

namespace XWeather
{
	public static class WeatherConditionsExtensions
	{
		static double getValueInUnits (TemperatureUnits units, double? fahrenheit, double? celcius, bool round = false)
		{
			var val = (units == TemperatureUnits.Fahrenheit ? fahrenheit : celcius) ?? 0;

			return round ? Math.Round (val) : val;
		}


		static string getValueInUnitsString (double value, bool degreeSymbol = false)
			=> degreeSymbol ? value.ToDegreesString () : value.ToString ();


		public static double Temp (this CurrentObservation observation, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, observation?.temp_f, observation?.temp_c, round);


		public static double FeelsLike (this CurrentObservation observation, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, observation?.feelslike_f, observation?.feelslike_c, round);


		public static double DewPoint (this CurrentObservation observation, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, observation?.dewpoint_f, observation?.dewpoint_c, round);


		public static double HighTemp (this ForecastDay forecast, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, forecast?.high?.FahrenheitValue, forecast?.high?.CelsiusValue, round);


		public static double LowTemp (this ForecastDay forecast, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, forecast?.low?.FahrenheitValue, forecast?.low?.CelsiusValue, round);


		public static double Temp (this HourlyForecast forecast, TemperatureUnits units, bool round = false)
			=> getValueInUnits (units, forecast?.temp?.english, forecast?.temp?.metric, round);



		public static string TempString (this CurrentObservation observation, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getValueInUnitsString (observation.Temp (units, round), degreeSymbol);


		public static string FeelsLikeString (this CurrentObservation observation, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getValueInUnitsString (observation.FeelsLike (units, round), degreeSymbol);


		public static string DewPointString (this CurrentObservation observation, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getValueInUnitsString (observation.DewPoint (units, round), degreeSymbol);


		public static string HighTempString (this ForecastDay forecast, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getValueInUnitsString (forecast.HighTemp (units, round), degreeSymbol);


		public static string LowTempString (this ForecastDay forecast, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getValueInUnitsString (forecast.LowTemp (units, round), degreeSymbol);


		public static string TempString (this HourlyForecast forecast, TemperatureUnits units, bool round = false, bool degreeSymbol = false)
			=> getValueInUnitsString (forecast.Temp (units, round), degreeSymbol);


		public static string HourString (this HourlyForecast forecast, bool lowercase = true)
			 => lowercase ? forecast.FCTTIME.civil.ToLower () : forecast.FCTTIME.civil;


	}
}