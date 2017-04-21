using Microsoft.Azure.Mobile;

using Plugin.VersionTracking;

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

			// Crashes.GetErrorAttachment = (report) => ErrorAttachment.AttachmentWithText (CrossVersionTracking.Current.ToString ());
#if __IOS__
			Microsoft.Azure.Mobile.Distribute.Distribute.DontCheckForUpdatesInDebug ();
#endif

			if (!string.IsNullOrEmpty (PrivateKeys.MobileCenter.AppSecret))
				MobileCenter.Start (PrivateKeys.MobileCenter.AppSecret,
					typeof (Microsoft.Azure.Mobile.Analytics.Analytics),
					typeof (Microsoft.Azure.Mobile.Crashes.Crashes),
					typeof (Microsoft.Azure.Mobile.Distribute.Distribute));

			PclExportClient.Configure ();

			// JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();

#if __ANDROID__
			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;

			Settings.RandomBackgrounds |= CrossVersionTracking.Current.IsFirstLaunchEver;
#endif
		}
	}
}