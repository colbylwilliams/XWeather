namespace XWeather.Droid
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			XWeather.Bootstrap.Run ();

			ServiceStack.AndroidPclExportClient.Configure ();

			ServiceStack.JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new ModernHttpClient.NativeMessageHandler ();

			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init ();
		}
	}
}