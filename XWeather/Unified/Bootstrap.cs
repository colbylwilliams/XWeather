#if __IOS__
using PclExportClient = ServiceStack.IosPclExportClient;
#else
using PclExportClient = ServiceStack.MacPclExportClient;
#endif

using SettingsStudio;
using ServiceStack;
using ModernHttpClient;

namespace XWeather.Unified
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			XWeather.Bootstrap.Run ();

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();

#if __IOS__
			//Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init ();
#endif
		}
	}
}