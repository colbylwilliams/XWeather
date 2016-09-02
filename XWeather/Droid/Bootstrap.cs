using Android.App;

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

			XWeather.Bootstrap.Run ();

			ServiceStack.AndroidPclExportClient.Configure ();

			ServiceStack.JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new ModernHttpClient.NativeMessageHandler ();

			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;
		}
	}
}