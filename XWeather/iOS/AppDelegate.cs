using Foundation;
using UIKit;

namespace XWeather.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }

		public AppDelegate ()
		{
			#region Configure HockeyApp

			//var manager = BITHockeyManager.SharedHockeyManager;

			//manager.Configure (AppKeys.HockeyAppKeyiOS);

			//manager.DisableUpdateManager = true;

			//manager.StartManager ();

			#endregion

			Unified.Bootstrap.Run ();
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
#endif

			return true;
		}
	}
}