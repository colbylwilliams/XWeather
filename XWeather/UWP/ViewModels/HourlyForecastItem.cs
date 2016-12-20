using XWeather.Domain;

namespace XWeather.UWP.ViewModels
{
	public class HourlyForecastItem
	{
		public HourlyForecastItem (HourlyForecast forecast)
		{
			Hour = forecast.HourString ();
			Period = forecast.PeriodString ();
			Temp = forecast.TempString (TemperatureUnits.Fahrenheit, true, true);
			Pop = forecast.ProbabilityPercipString ();
			Image = forecast.icon;
		}

		public string Hour { get; private set; }

		public string Period { get; private set; }

		public string Temp { get; private set; }

		public string Pop { get; private set; }

		public string Image { get; private set; }
	}
}
