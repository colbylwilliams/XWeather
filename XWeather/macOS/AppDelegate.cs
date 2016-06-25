using AppKit;
using Foundation;

namespace XWeather.macOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate ()
		{
			#region Configure HockeyApp

			//var manager = BITHockeyManager.SharedHockeyManager;

			//manager.Configure (AppKeys.HockeyAppKeyiOS);

			//manager.DisableUpdateManager = true;

			//manager.StartManager ();

			#endregion

			Unified.Bootstrap.Run ();

			//TinyIoCContainer.Current.BuildUp (this);

			//InitData ();
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			// Insert code here to initialize your application
		}

		public override void WillTerminate (NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}