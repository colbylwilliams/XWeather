namespace XWeather.Constants
{
	public static class ApiKeys
	{
		public const string AzureWebsite = "https://xweather.azurewebsites.net";

		public const string WuApiBasePath = "http://api.wunderground.com/api";

		public static string WuApiKeyedPath => $"{WuApiBasePath}/{PrivateKeys.WuApiKey}";

		public static string WuApiKeyedQueryFmt => $"{WuApiKeyedPath}/{{0}}/{{1}}";

		public static string WuApiKeyedQueryJsonFmt => $"{WuApiKeyedPath}/{{0}}{{1}}.json";

		public const string WuAcQueryFmt = "http://autocomplete.wunderground.com/aq?query={0}";
	}
}