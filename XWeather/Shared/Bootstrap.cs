using System.Threading.Tasks;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Crashes;

using ModernHttpClient;

using Plugin.VersionTracking;

using ServiceStack;

using SettingsStudio;

using NomadCode.Azure;

using XWeather.Constants;
using XWeather.Domain;


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

			Crashes.GetErrorAttachment = (report) => ErrorAttachment.AttachmentWithText (CrossVersionTracking.Current.ToString ());

			if (!string.IsNullOrEmpty (PrivateKeys.MobileCenter.AppSecret))
				MobileCenter.Start (PrivateKeys.MobileCenter.AppSecret,
									typeof (Microsoft.Azure.Mobile.Analytics.Analytics),
									typeof (Microsoft.Azure.Mobile.Crashes.Crashes));

			Settings.AzureStoreEnabled = !string.IsNullOrEmpty (PrivateKeys.MobileCenter.ServiceUrl);

			PclExportClient.Configure ();

			JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler ();

			Settings.RegisterDefaultSettings ();

#if __ANDROID__
			Settings.VersionNumber = CrossVersionTracking.Current.CurrentVersion;

			Settings.BuildNumber = CrossVersionTracking.Current.CurrentBuild;

			Settings.RandomBackgrounds |= CrossVersionTracking.Current.IsFirstLaunchEver;
#endif
		}

		public static async Task InitializeDataStore ()
		{
			if (Settings.AzureStoreEnabled)
			{
				AzureClient.Shared.RegisterTable<WuAcLocation> ();

				await AzureClient.Shared.InitializeAzync (PrivateKeys.MobileCenter.ServiceUrl);
			}
		}
	}
}