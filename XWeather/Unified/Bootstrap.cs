using SettingsStudio;

using ServiceStack;

using ModernHttpClient;

using Plugin.VersionTracking;


#if __IOS__

using PclExportClient = ServiceStack.IosPclExportClient;
using LocationProvider = XWeather.iOS.LocationProvider;

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

			XWeather.Bootstrap.Run ();

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();
		}
	}
}