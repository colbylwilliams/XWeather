namespace XWeather
{
	public enum Events
	{
		Unknown,
		LocationAdded,
		LocaitonDeleted,
		LocationSearchSelected,
		LocationSearchCanceled,
		RadarPanned,
		RadarZoomed
	}


	public static class EventStrings
	{
		public const string Unknown = "Unknown";
		public const string LocationAdded = "Location Added";
		public const string LocaitonDeleted = "Locaiton Deleted";
		public const string LocationSearchSelected = "LocationSearch Selected";
		public const string LocationSearchCanceled = "LocationSearch Canceled";
		public const string RadarPanned = "Radar Panned";
		public const string RadarZoomed = "Radar Zoomed";
	}


	public static class EventsExtensions
	{
		//public static string ViewString (this Pages page)
		//{
		//	return $"Page View: {page.Name ()}";
		//}


		public static string Name (this Events e)
		{
			switch (e)
			{
				case Events.Unknown: return EventStrings.Unknown;
				case Events.LocationAdded: return EventStrings.LocationAdded;
				case Events.LocaitonDeleted: return EventStrings.LocaitonDeleted;
				case Events.LocationSearchSelected: return EventStrings.LocationSearchSelected;
				case Events.LocationSearchCanceled: return EventStrings.LocationSearchCanceled;
				case Events.RadarPanned: return EventStrings.RadarPanned;
				case Events.RadarZoomed: return EventStrings.RadarZoomed;
				default: return EventStrings.Unknown;
			}
		}


		public static Events Event (this string e)
		{
			switch (e)
			{
				case EventStrings.Unknown: return Events.Unknown;
				case EventStrings.LocationAdded: return Events.LocationAdded;
				case EventStrings.LocaitonDeleted: return Events.LocaitonDeleted;
				case EventStrings.LocationSearchSelected: return Events.LocationSearchSelected;
				case EventStrings.LocationSearchCanceled: return Events.LocationSearchCanceled;
				case EventStrings.RadarPanned: return Events.RadarPanned;
				case EventStrings.RadarZoomed: return Events.RadarZoomed;
				default: return Events.Unknown;
			}
		}
	}
}