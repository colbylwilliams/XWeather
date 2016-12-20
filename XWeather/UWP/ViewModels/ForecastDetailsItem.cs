using XWeather.Domain;

namespace XWeather.UWP.ViewModels
{
	public class ForecastDetailsItem
	{
		public ForecastDetailsItem(WeatherDetail detail)
		{
			Label = detail.DetailLabel;
			Value = detail.DetailValue;
		}

		public string Label { get; private set; }

		public string Value { get; private set;  }
	}
}