using System;
using XWeather.Domain;
using System.Collections.Generic;
using ServiceStack;

namespace XWeather
{
	public static class TestData
	{
		static WuAcLocation sanfrancisco = new WuAcLocation {
			name = "San Francisco, California",
			type = "city",
			c = "US",
			zmw = "94101.1.99999",
			tz = "America/Los_Angeles",
			tzs = "PST",
			l = "/q/zmw:94101.1.99999",
			lat = "37.775009",
			lon = " -122.418259"
		};

		static WuAcLocation boulder = new WuAcLocation {
			name = "Boulder, Colorado",
			type = "city",
			c = "US",
			zmw = "80301.1.99999",
			tz = "America/Denver",
			tzs = "MDT",
			l = "/q/zmw:80301.1.99999",
			ll = "40.046795 -105.212502",
			lat = "40.046795",
			lon = "-105.212502"
		};

		static WuAcLocation tulsa = new WuAcLocation {
			name = "Tulsa, Oklahoma",
			type = "city",
			c = "US",
			zmw = "74101.1.99999",
			tz = "America/Chicago",
			tzs = "CDT",
			l = "/q/zmw:74101.1.99999",
			ll = "36.071289 -95.904274",
			lat = "36.071289",
			lon = "-95.904274"
		};

		static WuAcLocation atlanta = new WuAcLocation {
			name = "Atlanta, Georgia",
			type = "city",
			c = "US",
			zmw = "30301.1.99999",
			tz = "America/New_York",
			tzs = "EDT",
			l = "/q/zmw:30301.1.99999",
			ll = "33.855007 -84.395988",
			lat = "33.855007",
			lon = "-84.395988"
		};

		public static string LocationsJson => (new List<WuAcLocation> { sanfrancisco, boulder, tulsa, atlanta }).SerializeToString ();
	}
}