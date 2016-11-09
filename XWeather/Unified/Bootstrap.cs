using SettingsStudio;

using ServiceStack;

using ModernHttpClient;

using Plugin.VersionTracking;


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
			CrossVersionTracking.Current.Track ();

			AnalyticsManager.Shared.ConfigureHockeyApp ();

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();
		}
	}
}