using XWeather.Domain;

namespace XWeather.UWP.ViewModels
{
	public class DailyForecastItem
	{
		public DailyForecastItem(ForecastDay forecast)
		{
			Day = forecast.date.weekday;
			Pop = forecast.ProbabilityPercipString ();
			High = forecast.HighTempString (TemperatureUnits.Fahrenheit, true, true);
			Low = forecast.LowTempString (TemperatureUnits.Fahrenheit, true, true);
			Image = forecast.icon;
		}

		public string Day { get; private set; }

		public string Pop { get; private set; }

		public string High { get; private set; }

		public string Low { get; private set; }

		public string Image { get; private set; }
	}
}
