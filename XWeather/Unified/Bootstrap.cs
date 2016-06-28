#if __IOS__

using HockeyApp.iOS;
using PclExportClient = ServiceStack.IosPclExportClient;

#else

using PclExportClient = ServiceStack.MacPclExportClient;

#endif

using SettingsStudio;

using ServiceStack;

using ModernHttpClient;
using XWeather.Constants;

namespace XWeather.Unified
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			configureHockeyApp ();

			XWeather.Bootstrap.Run ();

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();
		}


		static void configureHockeyApp ()
		{
			var manager = BITHockeyManager.SharedHockeyManager;

			manager.Configure (PrivateKeys.HockeyApiKey_iOS);

			manager.DisableUpdateManager = true;

			manager.StartManager ();
		}
	}
}