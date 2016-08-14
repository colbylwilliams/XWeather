using Android.App;
using Android.Content;
using Android.Content.PM;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using SettingsStudio;
using XWeather.Constants;

namespace XWeather.Droid
{
	public static class Bootstrap
	{
		public static void Run (Context context, Application application)
		{
			configureHockeyApp (context, application);

			XWeather.Bootstrap.Run ();

			ServiceStack.AndroidPclExportClient.Configure ();

			ServiceStack.JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new ModernHttpClient.NativeMessageHandler ();

			Settings.VersionNumber = application.PackageManager.GetPackageInfo (application.PackageName, 0).VersionName;

			Settings.BuildNumber = application.PackageManager.GetPackageInfo (application.PackageName, 0).VersionCode.ToString ();

			//Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init ();
		}


		static void configureHockeyApp (Context context, Application application)
		{
			CrashManager.Register (context, PrivateKeys.HockeyApiKey_Droid);

			// UpdateManager.Register(this, PrivateKeys.HockeyApiKey_Droid);

			MetricsManager.Register (context, application, PrivateKeys.HockeyApiKey_Droid);
		}
	}
}