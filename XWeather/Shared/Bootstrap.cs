using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Plugin.VersionTracking;
using SettingsStudio;

using XWeather.Constants;


namespace XWeather.Shared
{
	public static class Bootstrap
	{
		public static async void Run ()
		{
			CrossVersionTracking.Current.Track ();

			// Crashes.GetErrorAttachment = (report) => ErrorAttachment.AttachmentWithText (CrossVersionTracking.Current.ToString ());
#if __IOS__
			Distribute.DontCheckForUpdatesInDebug ();
#endif

			if (!string.IsNullOrEmpty (PrivateKeys.MobileCenter.AppSecret))
				AppCenter.Start (PrivateKeys.MobileCenter.AppSecret,
									typeof (Analytics),
									typeof (Crashes),
									typeof (Distribute));

			// JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();
			Settings.SetUomDefaults (CrossVersionTracking.Current.IsFirstLaunchEver);

			var appcenterInstallId = await AppCenter.GetInstallIdAsync ();
			Settings.UserReferenceKey = appcenterInstallId?.ToString ("N");

#if __ANDROID__
			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;

			Settings.RandomBackgrounds |= CrossVersionTracking.Current.IsFirstLaunchEver;
#endif
		}
	}
}