using System.Collections.Generic;

namespace XWeather.Domain
{
	public class WuWeather : WuObject
	{
		//public override string WuKey => "conditions/forecast10day/hourly10day/astronomy/tide/rawtide";
		//public override string WuKey => "conditions/forecast10day/hourly/astronomy";
		public override string WuKey => "conditions/forecast10day/hourly10day/astronomy";


		// Conditions
		public CurrentObservation current_observation { get; set; }


		// ForecastTenDay
		public ForecastDetail forecast { get; set; }

		public TxtForecast TextForecast => forecast?.txt_forecast;
		public SimpleForecast Simpleforecast => forecast?.simpleforecast;


		// Hourly / HourlyTenDay
		public List<HourlyForecast> hourly_forecast { get; set; }


		// Astronomy
		public MoonPhase moon_phase { get; set; }
		public AstronomyPhase sun_phase { get; set; }


		//// Tide
		//public TideDetail tide { get; set; }

		//public List<TideInfo> TideInfo => tide?.tideInfo;
		//public List<TideSummary> TideSummary => tide?.tideSummary;
		//public List<TideStat> TideSummaryStats => tide?.tideSummaryStats;

		//// RawTide
		//public RawTideDetail rawtide { get; set; }

		//public List<TideInfo> RawTideInfo => rawtide?.tideInfo;
		//public List<RawTideOb> RawTideObs => rawtide?.rawTideObs;
		//public List<TideStat> RawTideStats => rawtide?.rawTideStats;
	}
}