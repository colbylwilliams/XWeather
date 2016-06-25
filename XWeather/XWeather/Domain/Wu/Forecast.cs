using System.Collections.Generic;

namespace XWeather.Domain
{
	public class Forecast : WuObject
	{
		public override string WuKey => "forecast";

		public ForecastDetail forecast { get; set; }

		public TxtForecast TextForecast => forecast?.txt_forecast;
		public SimpleForecast Simpleforecast => forecast?.simpleforecast;
	}


	public class ForecastDetail
	{
		public TxtForecast txt_forecast { get; set; }
		public SimpleForecast simpleforecast { get; set; }
	}


	public class TxtForecastDay
	{
		public int period { get; set; }
		public string icon { get; set; }
		public string icon_url { get; set; }
		public string title { get; set; }
		public string fcttext { get; set; }
		public string fcttext_metric { get; set; }
		public string pop { get; set; }
	}


	public class ForecastDay
	{
		public WuDate date { get; set; }
		public int period { get; set; }
		public Temperature high { get; set; }
		public Temperature low { get; set; }
		public string conditions { get; set; }
		public string icon { get; set; }
		public string icon_url { get; set; }
		public string skyicon { get; set; }
		public int pop { get; set; }
		public Precipitation qpf_allday { get; set; }
		public Precipitation qpf_day { get; set; }
		public Precipitation qpf_night { get; set; }
		public Precipitation snow_allday { get; set; }
		public Precipitation snow_day { get; set; }
		public Precipitation snow_night { get; set; }
		public Wind maxwind { get; set; }
		public Wind avewind { get; set; }
		public int avehumidity { get; set; }
		public int maxhumidity { get; set; }
		public int minhumidity { get; set; }
	}


	public class TxtForecast
	{
		public string date { get; set; }
		public List<TxtForecastDay> forecastday { get; set; }
	}


	public class SimpleForecast
	{
		public List<ForecastDay> forecastday { get; set; }
	}
}