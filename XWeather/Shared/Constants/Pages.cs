namespace XWeather
{
	public enum Pages
	{
		Unknown,
		LocationList,
		LocationSearch,
		WeatherDaily,
		WeatherHourly,
		WeatherDetails,
		WeatherRadar,
		Settings
	}


	public static class PageStrings
	{
		public const string LocationList = "Location List";
		public const string LocationSearch = "Location Search";
		public const string WeatherDaily = "Weather Daily";
		public const string WeatherHourly = "Weather Hourly";
		public const string WeatherDetails = "Weather Details";
		public const string WeatherRadar = "Weather Radar";
		public const string Settings = "Settings";
		public const string Unknown = "Unknown";
	}


	public static class PagesExtensions
	{
		public static string ViewString (this Pages page)
		{
			return $"Page View: {page.Name ()}";
		}


		public static string Name (this Pages page)
		{
			switch (page)
			{
				case Pages.Unknown: return PageStrings.Unknown;
				case Pages.LocationList: return PageStrings.LocationList;
				case Pages.LocationSearch: return PageStrings.LocationSearch;
				case Pages.WeatherDaily: return PageStrings.WeatherDaily;
				case Pages.WeatherHourly: return PageStrings.WeatherHourly;
				case Pages.WeatherDetails: return PageStrings.WeatherDetails;
				case Pages.WeatherRadar: return PageStrings.WeatherRadar;
				case Pages.Settings: return PageStrings.Settings;
				default: return PageStrings.Unknown;
			}
		}


		public static Pages Page (this string page)
		{
			switch (page)
			{
				case PageStrings.Unknown: return Pages.Unknown;
				case PageStrings.LocationList: return Pages.LocationList;
				case PageStrings.LocationSearch: return Pages.LocationSearch;
				case PageStrings.WeatherDaily: return Pages.WeatherDaily;
				case PageStrings.WeatherHourly: return Pages.WeatherHourly;
				case PageStrings.WeatherDetails: return Pages.WeatherDetails;
				case PageStrings.WeatherRadar: return Pages.WeatherRadar;
				case PageStrings.Settings: return Pages.Settings;
				default: return Pages.Unknown;
			}
		}
	}
}