using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

using ServiceStack;

using ModernHttpClient;

using SettingsStudio;

using Plugin.VersionTracking;

using XWeather.Constants;

namespace XWeather.Droid
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			CrossVersionTracking.Current.Track ();

			if (!string.IsNullOrEmpty (PrivateKeys.HockeyApiKey_Droid))
				MobileCenter.Start (PrivateKeys.HockeyApiKey_Droid, typeof (Analytics), typeof (Crashes));

			//AnalyticsManager.Shared.ConfigureHockeyApp (context, application);

			AndroidPclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;
		}
	}
}