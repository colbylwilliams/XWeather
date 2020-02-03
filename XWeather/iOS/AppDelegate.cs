using Foundation;
using Microsoft.AppCenter.Distribute;
using UIKit;

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

#if DEBUG
			Xamarin.Calabash.Start ();
#endif
			return true;
		}

		public override bool OpenUrl (UIApplication app, NSUrl url, NSDictionary options)
		{
			Distribute.OpenUrl (url);

			return true;
		}

#if DEBUG
		[Export ("updateSettingsToImperial")]
		public NSString UpdateSettingsToImperial ()
		{
			SettingsStudio.Settings.UomTemperature = TemperatureUnits.Celsius;
			SettingsStudio.Settings.UomDistance = DistanceUnits.Kilometers;
			SettingsStudio.Settings.UomPressure = PressureUnits.Millibars;
			SettingsStudio.Settings.UomLength = LengthUnits.Millimeters;
			SettingsStudio.Settings.UomSpeed = SpeedUnits.KilometersPerHour;

			return new NSString ("done");
		}
#endif
	}
}