using System.Collections.Generic;

namespace XWeather.Domain
{
	public class Hourly : WuObject
	{
		public override string WuKey => "hourly";

		public List<HourlyForecast> hourly_forecast { get; set; }
	}

	public class HourlyForecast
	{
		public FCTTIME FCTTIME { get; set; }
		public Measurement temp { get; set; }
		public Measurement dewpoint { get; set; }
		public string condition { get; set; }
		public string icon { get; set; }
		public string icon_url { get; set; }
		public string fctcode { get; set; }
		public string sky { get; set; }
		public Measurement wspd { get; set; }
		public Measurement wdir { get; set; }
		public string wx { get; set; }
		public string uvi { get; set; }
		public string humidity { get; set; }
		public Measurement windchill { get; set; }
		public Measurement heatindex { get; set; }
		public Measurement feelslike { get; set; }
		public Measurement qpf { get; set; }
		public Measurement snow { get; set; }
		public double pop { get; set; }
		public Measurement mslp { get; set; }
	}
}