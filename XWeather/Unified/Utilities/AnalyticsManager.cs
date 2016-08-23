using XWeather.Constants;

namespace XWeather.Unified
{
	public class AnalyticsManager
	{
		static AnalyticsManager _shared;

		public static AnalyticsManager Shared => _shared ?? (_shared = new AnalyticsManager ());


		bool managerStarted;

		public void ConfigureHockeyApp ()
		{
#if __IOS__
			if (!string.IsNullOrEmpty (PrivateKeys.HockeyApiKey_iOS)) {

				var manager = HockeyApp.iOS.BITHockeyManager.SharedHockeyManager;

				manager.Configure (PrivateKeys.HockeyApiKey_iOS);

				manager.DisableUpdateManager = true;

				manager.StartManager ();

				managerStarted = true;
			}
#endif
		}

		public void TrackEvent (string eventName)
		{
#if __IOS__
			if (managerStarted)
				HockeyApp.iOS.BITHockeyManager.SharedHockeyManager.MetricsManager.TrackEvent (eventName);
#endif
		}
	}
}