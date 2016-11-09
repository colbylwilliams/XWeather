using Android.App;

using ServiceStack;

using ModernHttpClient;

using SettingsStudio;

using Plugin.VersionTracking;


namespace XWeather.Droid
{
	public static class Bootstrap
	{
		public static void Run (Activity context, Application application)
		{
			CrossVersionTracking.Current.Track ();

			AnalyticsManager.Shared.ConfigureHockeyApp (context, application);

			AndroidPclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;
		}
	}
}