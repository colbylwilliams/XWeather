namespace XWeather.Constants
{
	public static class PrivateKeys
	{
		public const string WuApiKey = @"";              // https://github.com/colbylwilliams/XWeather#weather-underground

		//public const string HockeyApiKey_iOS = @"";      // https://github.com/colbylwilliams/XWeather#hockeyapp-optional

		//public const string HockeyApiKey_Droid = @"";    // https://github.com/colbylwilliams/XWeather#hockeyapp-optional

		public const string GoogleMapsApiKey = @"";      // https://github.com/colbylwilliams/XWeather#google-maps-api-key-android

		public static class MobileCenter
		{
#if __IOS__
			public const string AppSecret = @"";
#elif __ANDROID__
			public const string AppSecret = @"";
#endif
		}
	}
}