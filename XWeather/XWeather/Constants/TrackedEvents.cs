using System;
namespace XWeather
{
	public static class TrackedEvents
	{
		public static class LocationList
		{
			public const string Opened = "Location List - Opened";

			public const string Selected = "Location List - Selected";

			public const string Added = "Location List - Added";

			public const string Removed = "Location List - Removed";
		}


		public static class LocationSearch
		{
			public const string Opened = "Location Search - Opened";

			public const string Canceled = "Location Search - Canceled";

			public const string Selected = "Location Search - Selected";
		}


		public static class WeatherDaily
		{
			public const string Opened = "Weather Daily - Opened";
		}


		public static class WeatherHourly
		{
			public const string Opened = "Weather Hourly - Opened";
		}


		public static class WeatherDetails
		{
			public const string Opened = "Weather Details - Opened";
		}


		public static class WeatherRadar
		{
			public const string Opened = "Weather Radar - Opened";
			public const string Zoomed = "Weather Radar - Zoomed";
		}


		public static class Settings
		{
			public const string Opened = "Settings - Opened";
		}
	}
}