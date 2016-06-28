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
		}

		public override void DidFinishLaunching (NSNotification notification) { }

		public override void WillTerminate (NSNotification notification) { }
	}
}