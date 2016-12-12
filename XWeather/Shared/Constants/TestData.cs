using System;
using XWeather.Domain;
using System.Collections.Generic;
using ServiceStack;

namespace XWeather
{
	public static class TestData
	{

		static WuAcLocation Dallas = new WuAcLocation {
			name = "Dallas, Texas",
			type = "city",
			c = "US",
			zmw = "75201.1.99999",
			tz = "America/Chicago",
			tzs = "CDT",
			l = "/q/zmw:75201.1.99999",
			ll = "32.786999 -96.798347",
			lat = 32.786999,
			lon = -96.798347
		};

		static WuAcLocation Denver = new WuAcLocation {
			name = "Denver, Colorado",
			type = "city",
			c = "US",
			zmw = "80202.1.99999",
			tz = "America/Denver",
			tzs = "MDT",
			l = "/q/zmw:80202.1.99999",
			ll = "39.749840 -104.995598",
			lat = 39.749840,
			lon = -104.995598
		};

		static WuAcLocation LasVegas = new WuAcLocation {
			name = "Las Vegas, Nevada",
			type = "city",
			c = "US",
			zmw = "89044.2.99999",
			tz = "America/Los_Angeles",
			tzs = "PDT",
			l = "/q/zmw:89044.2.99999",
			ll = "35.994629 -115.118973",
			lat = 35.994629,
			lon = -115.118973
		};

		static WuAcLocation Miami = new WuAcLocation {
			name = "Miami, Florida",
			type = "city",
			c = "US",
			zmw = "33010.2.99999",
			tz = "America/New_York",
			tzs = "EDT",
			l = "/q/zmw:33010.2.99999",
			ll = "25.828979 -80.285637",
			lat = 25.828979,
			lon = -80.285637
		};

		static WuAcLocation NewOrleans = new WuAcLocation {
			name = "New Orleans, Louisiana",
			type = "city",
			c = "US",
			zmw = "70112.1.99999",
			tz = "America/Chicago",
			tzs = "CDT",
			l = "/q/zmw:70112.1.99999",
			ll = "29.957520 -90.076859",
			lat = 29.957520,
			lon = -90.076859
		};

		//public static string LocationsJson => (new List<WuAcLocation> { atlanta }).ToJson ();

		public static List<WuAcLocation> Locations => new List<WuAcLocation> { Dallas, Denver, LasVegas, Miami, NewOrleans };

		public static string LocationsJson => Locations.ToJson ();
	}
}