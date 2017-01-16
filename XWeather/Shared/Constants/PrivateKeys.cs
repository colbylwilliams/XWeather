namespace XWeather.Constants
{
	public static class PrivateKeys
	{
		public const string WuApiKey = @"";              // https://github.com/colbylwilliams/XWeather#weather-underground

		public const string GoogleMapsApiKey = @"";      // https://github.com/colbylwilliams/XWeather#google-maps-api-key-android

		public static class MobileCenter                 // https://github.com/colbylwilliams/XWeather#mobile-center#visual-studio-mobile-center-optional
		{
#if __IOS__
			public const string AppSecret = @"";

			public const string ServiceUrl = @"";
#elif __ANDROID__
			public const string AppSecret = @"";

			public const string ServiceUrl = @"";
#endif
		}
	}
}