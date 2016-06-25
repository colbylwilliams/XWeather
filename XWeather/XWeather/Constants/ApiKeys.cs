namespace XWeather.Constants
{
	public static class ApiKeys
	{
		public const string AzureWebsite = "https://xweather.azurewebsites.net";

		public const string WuApiBasePath = "http://api.wunderground.com/api";

		public static string WuApiKeyedPath => $"{WuApiBasePath}/{PrivateKeys.WuApiKey}";
	}
}