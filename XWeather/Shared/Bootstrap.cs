using Plugin.VersionTracking;

using ServiceStack;

using SettingsStudio;

using XWeather.Constants;


#if __IOS__

using PclExportClient = ServiceStack.IosPclExportClient;

#elif __ANDROID__

using PclExportClient = ServiceStack.AndroidPclExportClient;

#elif WINDOWS_UWP

using PclExportClient = ServiceStack.WinStorePclExportClient;

#endif

namespace XWeather.Shared
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			CrossVersionTracking.Current.Track ();

			PclExportClient.Configure ();

#if !WINDOWS_UWP

			Microsoft.Azure.Mobile.Crashes.Crashes.GetErrorAttachment = (report) =>
				Microsoft.Azure.Mobile.Crashes.ErrorAttachment.AttachmentWithText (CrossVersionTracking.Current.ToString ());

			if (!string.IsNullOrEmpty (PrivateKeys.MobileCenter.AppSecret))
				Microsoft.Azure.Mobile.MobileCenter.Start (PrivateKeys.MobileCenter.AppSecret,
									typeof (Microsoft.Azure.Mobile.Analytics.Analytics),
									typeof (Microsoft.Azure.Mobile.Crashes.Crashes));

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new ModernHttpClient.NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();

#endif

#if __ANDROID__

			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;

#endif
		}
	}
}