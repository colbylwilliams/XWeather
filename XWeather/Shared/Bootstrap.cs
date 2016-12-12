using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

using ModernHttpClient;

using Plugin.VersionTracking;

using ServiceStack;

using SettingsStudio;

using XWeather.Constants;


#if __IOS__

using PclExportClient = ServiceStack.IosPclExportClient;

#elif __ANDROID__

using PclExportClient = ServiceStack.AndroidPclExportClient;

#endif


namespace XWeather.Shared
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			CrossVersionTracking.Current.Track ();

			if (!string.IsNullOrEmpty (PrivateKeys.HockeyApiKey_iOS))
				MobileCenter.Start (PrivateKeys.HockeyApiKey_iOS, typeof (Analytics), typeof (Crashes));

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();

#if __ANDROID__
			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;
#endif
		}
	}
}