using Foundation;
using UIKit;

using SettingsStudio;

namespace XWeather.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }

		public AppDelegate ()
		{
			Shared.Bootstrap.Run ();

			Analytics.Start ();
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{

#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
#endif
			return true;
		}

#if ENABLE_TEST_CLOUD
		[Export ("updateSettingsToImperial")]
		public NSString UpdateSettingsToImperial ()
		{
			Settings.UomTemperature = TemperatureUnits.Celsius;
			Settings.UomDistance = DistanceUnits.Kilometers;
			Settings.UomPressure = PressureUnits.Millibars;
			Settings.UomLength = LengthUnits.Millimeters;
			Settings.UomSpeed = SpeedUnits.KilometersPerHour;

			return new NSString ("done");
		}
#endif
	}
}