using System;

using XWeather.Domain;
using System.Collections.Generic;

namespace XWeather
{
	public class WuLocation
	{
		public WuLocation (WuAcLocation location)
		{
			Location = location;
		}


		public WuLocation (WuAcLocation location, WuWeather weather)
		{
			Location = location;
			Weather = weather;
			Updated = DateTime.UtcNow;
		}


		public DateTime Updated { get; set; }

		public WuWeather Weather { get; set; }

		public WuAcLocation Location { get; set; }


		public bool Current { get { return Location.Current; } set { Location.Current = value; } }

		public bool Selected { get { return Location.Selected; } set { Location.Selected = value; } }

		public string Name => Weather?.current_observation?.display_location?.city ?? Location?.name;

		public bool HasSunTimes => CurrentTime != null && Sunset != null && Sunrise != null;


		public CurrentObservation Conditions => Weather?.current_observation;

		public List<ForecastDay> Forecasts => Weather?.Simpleforecast?.forecastday ?? new List<ForecastDay> ();



		public AstronomyTime CurrentTime => Weather?.moon_phase?.current_time;

		public AstronomyTime Sunset => Weather?.moon_phase?.sunset ?? Weather?.sun_phase?.sunset;

		public AstronomyTime Sunrise => Weather?.moon_phase?.sunrise ?? Weather?.sun_phase?.sunrise;
	}
}