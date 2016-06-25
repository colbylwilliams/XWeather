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

			Bootstrap.Run ();

			//TinyIoCContainer.Current.BuildUp (this);

			//Settings.RegisterDefaultSettings ();

			//InitData ();
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{

			// Code to start the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
#endif

			return true;
		}
	}
}