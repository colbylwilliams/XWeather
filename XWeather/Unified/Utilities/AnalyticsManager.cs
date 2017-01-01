//using XWeather.Constants;


//namespace XWeather.Unified
//{
//	public class AnalyticsManager
//	{
//		static AnalyticsManager _shared;

//		public static AnalyticsManager Shared => _shared ?? (_shared = new AnalyticsManager ());

//#if __IOS__
//		HockeyApp.iOS.BITHockeyManager manager => HockeyApp.iOS.BITHockeyManager.SharedHockeyManager;
//#endif

//		bool managerStarted;

//		public void Configure ()
//		{
//#if __IOS__
//			if (!string.IsNullOrEmpty (PrivateKeys.HockeyApiKey_iOS)) {

//				manager.Configure (PrivateKeys.HockeyApiKey_iOS);

//				manager.DisableCrashManager = false;
//				manager.DisableUpdateManager = false;
//				manager.DisableMetricsManager = false;
//				manager.DisableInstallTracking = false;

//				manager.StartManager ();

//				manager.Authenticator.AuthenticateInstallation ();

//				managerStarted = true;
//			}
//#endif
//		}

//		public void TrackEvent (string eventName)
//		{
//#if __IOS__
//			if (managerStarted)
//				manager.MetricsManager.TrackEvent (eventName);

//			System.Diagnostics.Debug.WriteLine ($"[AnalyticsManager] TrackEvent: '{eventName}'");
//#endif
//		}
//	}
//}