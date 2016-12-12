using System.Collections.Generic;

namespace XWeather.Domain
{
	public class GeoLookup : WuObject
	{
		public override string WuKey => "geolookup";

		public GeoLookupLocation location { get; set; }
	}


	public class WeatherStation : GeoLocation
	{
		public string icao { get; set; }
	}


	public class PersonalWeatherStation : WeatherStation
	{
		public string neighborhood { get; set; }
		public string id { get; set; }
		public int distance_km { get; set; }
		public int distance_mi { get; set; }
	}


	public class NearbyWeatherStations
	{
		public Airport airport { get; set; }
		public Pws pws { get; set; }
	}


	public class Airport
	{
		public List<WeatherStation> station { get; set; }
	}


	public class Pws
	{
		public List<PersonalWeatherStation> station { get; set; }
	}


	public class GeoLookupLocation : GeoLocation
	{
		public string type { get; set; }
		public string country_name { get; set; }
		public string tz_short { get; set; }
		public string tz_long { get; set; }
		public string zip { get; set; }
		public string magic { get; set; }
		public string wmo { get; set; }
		public string l { get; set; }
		public string requesturl { get; set; }
		public string wuiurl { get; set; }
		public NearbyWeatherStations nearby_weather_stations { get; set; }
	}
}