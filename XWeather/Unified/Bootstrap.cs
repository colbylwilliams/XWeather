using SettingsStudio;

using ServiceStack;

using ModernHttpClient;


#if __IOS__

using PclExportClient = ServiceStack.IosPclExportClient;

#else

using PclExportClient = ServiceStack.MacPclExportClient;

#endif

namespace XWeather.Unified
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			AnalyticsManager.Shared.ConfigureHockeyApp ();

			XWeather.Bootstrap.Run ();

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();
		}
	}
}