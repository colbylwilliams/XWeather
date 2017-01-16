using System;

using XWeather.Domain;
using System.Collections.Generic;
using System.Linq;

namespace XWeather
{
	public class WuLocation : IComparable<WuLocation>//, IEquatable<WuLocation>
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


		public bool Current => Location.Current;

		public bool Selected { get { return Location.Selected; } set { Location.Selected = value; } }

		public string Name => Weather?.current_observation?.display_location?.city ?? Location?.name;

		public bool HasSunTimes => CurrentTime != null && Sunset != null && Sunrise != null;


		public CurrentObservation Conditions => Weather?.current_observation;

		public List<ForecastDay> Forecasts => Weather?.Simpleforecast?.forecastday ?? new List<ForecastDay> ();

		public ForecastDay TodayForecast => Weather?.Simpleforecast?.forecastday?.FirstOrDefault ();

		public List<TxtForecastDay> TxtForecasts => Weather?.TextForecast?.forecastday ?? new List<TxtForecastDay> ();

		public List<HourlyForecast> HourlyForecasts => Weather?.hourly_forecast ?? new List<HourlyForecast> ();


		Dictionary<int, List<HourlyForecast>> hourlyForecastCache = new Dictionary<int, List<HourlyForecast>> ();

		public List<HourlyForecast> HourlyForecast (int day, bool nextDay = true)
			=> hourlyForecastCache.ContainsKey (day) ?
						   hourlyForecastCache [day] :
						  (hourlyForecastCache [day] = Weather?.hourly_forecast?.Where (f => f.FCTTIME.mday == day || (nextDay && f.FCTTIME.mday == day + 1))?.ToList () ?? new List<HourlyForecast> ());


		public AstronomyTime CurrentTime => Weather?.moon_phase?.current_time;

		public AstronomyTime Sunset => Weather?.moon_phase?.sunset ?? Weather?.sun_phase?.sunset;

		public AstronomyTime Sunrise => Weather?.moon_phase?.sunrise ?? Weather?.sun_phase?.sunrise;


		public int CompareTo (WuLocation other)
		{
			if (Current) return -1;

			if (other.Current) return 1;

			return Name.CompareTo (other.Name);
		}
	}
}