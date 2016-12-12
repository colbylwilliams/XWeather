//using Android.App;
//using Android.Content;

//using XWeather.Constants;

//namespace XWeather.Droid
//{
//	public class AnalyticsManager
//	{
//		static AnalyticsManager _shared;

//		public static AnalyticsManager Shared => _shared ?? (_shared = new AnalyticsManager ());


//		bool managerStarted;

//		public void ConfigureHockeyApp (Context context, Application application)
//		{
//			if (!string.IsNullOrEmpty (PrivateKeys.HockeyApiKey_Droid)) {

//				HockeyApp.Android.CrashManager.Register (context, PrivateKeys.HockeyApiKey_Droid);

//				HockeyApp.Android.Metrics.MetricsManager.Register (application, PrivateKeys.HockeyApiKey_Droid);

//				managerStarted = true;
//			}
//		}


//		public void RegisterForHockeyAppUpdates (Activity context)
//		{
//			if (managerStarted) {
//				HockeyApp.Android.UpdateManager.Register (context, PrivateKeys.HockeyApiKey_Droid);
//			}
//		}


//		public void UnregisterForHockeyAppUpdates ()
//		{
//			if (managerStarted) {
//				HockeyApp.Android.UpdateManager.Unregister ();
//			}
//		}


//		public void TrackEvent (string eventName)
//		{
//			if (managerStarted && HockeyApp.Android.Metrics.MetricsManager.IsUserMetricsEnabled)
//				HockeyApp.Android.Metrics.MetricsManager.TrackEvent (eventName);
//		}


//		public void StartUsage (Activity activity)
//		{
//			if (managerStarted)
//				HockeyApp.Android.Tracking.StartUsage (activity);
//		}

//		public void StopUsage (Activity activity)
//		{
//			if (managerStarted)
//				HockeyApp.Android.Tracking.StopUsage (activity);
//		}

//	}
//}
