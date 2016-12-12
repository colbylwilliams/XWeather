using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

using SettingsStudio;

using ServiceStack;

using ModernHttpClient;

using Plugin.VersionTracking;

using XWeather.Constants;

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

			if (!string.IsNullOrEmpty (PrivateKeys.HockeyApiKey_iOS))
				MobileCenter.Start (PrivateKeys.HockeyApiKey_iOS, typeof (Analytics), typeof (Crashes));

			//AnalyticsManager.Shared.ConfigureHockeyApp ();

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();
		}
	}
}