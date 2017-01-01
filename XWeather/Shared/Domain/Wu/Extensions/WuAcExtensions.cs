using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using ServiceStack;

namespace XWeather.Domain
{
	public static class WuAcExtensions
	{
		public static string [] GetCityStateArray (this WuAcLocation location)
			=> string.IsNullOrEmpty (location.name) ? null : Regex.Split (location.name, @"\s*,\s*");


		public static string GetZip (this WuAcLocation location)
			=> string.IsNullOrEmpty (location.zmw) ? string.Empty : location.zmw.Substring (0, location.zmw.IndexOf ('.'));


		public static WuAcLocation ToWuAcLocation (this GeoLookup lookup, bool current = true)
		{
			var location = lookup.location;

			return new WuAcLocation {
				name = $"{location.city}, {location.state}",
				type = location.type,
				c = location.country,
				zmw = $"{location.zip}.{location.magic}.{location.wmo}",
				tz = location.tz_long,
				tzs = location.tz_short,
				l = location.l,
				ll = $"{location.lat}, {location.lon}",
				lat = location.lat,
				lon = location.lon,
				Current = current
			};
		}


		public static string GetLocationsJson (this List<WuLocation> locations)
			=> locations.Select (l => l.Location).ToList ().ToJson ();


		public static List<WuAcLocation> GetLocations (this string json)
			=> json?.FromJson<List<WuAcLocation>> () ?? new List<WuAcLocation> ();
	}
}