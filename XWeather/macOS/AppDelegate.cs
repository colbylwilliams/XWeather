using AppKit;
using Foundation;

namespace XWeather.macOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate ()
		{
			Unified.Bootstrap.Run ();
		}

		public override void DidFinishLaunching (NSNotification notification) { }

		public override void WillTerminate (NSNotification notification) { }
	}
}