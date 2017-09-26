using MobileAnalytics = Microsoft.Azure.Mobile.Analytics.Analytics;
using Microsoft.Azure.Mobile.Crashes;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


#if __IOS__
using Foundation;
using UIKit;
using TView = UIKit.UIViewController;
#elif __ANDROID__
// something random that includes both Activity and Fragment
using TView = Android.Views.View.IOnCreateContextMenuListener;
#endif


namespace XWeather
{
	public static partial class Analytics
	{
		static int hashCache;


		static ConcurrentDictionary<int, double> pageTime = new ConcurrentDictionary<int, double> ();

		static ConcurrentDictionary<int, string> pageNames = new ConcurrentDictionary<int, string> ();


		static ConcurrentDictionary<int, double> pausedPages = new ConcurrentDictionary<int, double> ();

		static ConcurrentDictionary<int, double> pausedPageDurations = new ConcurrentDictionary<int, double> ();


		static ConcurrentDictionary<int, IDictionary<string, string>> pageProperties = new ConcurrentDictionary<int, IDictionary<string, string>> ();


#if __IOS__


		static NSObject foregroundNotificationToken, backgroundNotificationToken, terminateNotificationToken;


		public static void Start ()
		{
			log ("Start");

			//Microsoft.Azure.Mobile.MobileCenter.LogLevel = Microsoft.Azure.Mobile.LogLevel.Debug;

			if (backgroundNotificationToken == null)
				backgroundNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver (
					UIApplication.DidEnterBackgroundNotification, handleBackgroundNotification);

			if (terminateNotificationToken == null)
				terminateNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver (
					UIApplication.WillTerminateNotification, handleTerminatNotitication);
		}


		static void handleForegroundNotification (NSNotification notification)
		{
			log ("WillEnterForegroundNotification");

			foregroundNotificationToken?.Dispose ();
			foregroundNotificationToken = null;

			if (backgroundNotificationToken == null)
				backgroundNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver (
					UIApplication.DidEnterBackgroundNotification, handleBackgroundNotification);

			StartLastPageEnd ();
		}


		static void handleBackgroundNotification (NSNotification notification)
		{
			log ("DidEnterBackgroundNotification");

			backgroundNotificationToken?.Dispose ();
			backgroundNotificationToken = null;

			if (foregroundNotificationToken == null)
				foregroundNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver (
					UIApplication.WillEnterForegroundNotification, handleForegroundNotification);

			EndLastPageStart ();
		}


		static void handleTerminatNotitication (NSNotification notification)
		{
			log ("WillTerminateNotification");

			backgroundNotificationToken?.Dispose ();
			backgroundNotificationToken = null;

			foregroundNotificationToken?.Dispose ();
			foregroundNotificationToken = null;

			terminateNotificationToken?.Dispose ();
			terminateNotificationToken = null;
		}


#endif


		#region Track Page Views & Duration


		public static void TrackPageView (string pageName, IDictionary<string, string> properties = null)
		{
			trackEvent (viewString (pageName), properties);
			trackBasicPageView (pageName);
		}


		#region Track Page View Start


		public static void TrackPageViewStart<T> (T page, string pageName, IDictionary<string, string> properties = null)
			where T : TView
		{
			var hash = page.GetHashCode ();

			trackPageViewStart (hash, pageName, properties);
		}


		static void trackPageViewStart (int hash, string pageName, IDictionary<string, string> properties = null)
		{
			double time = 0;

			if (pageTime.TryGetValue (hash, out time) && time > 0)
			{
				log ($"TrackEvent :: name: {viewString (pageName)} - 'TrackPageViewStart' was already called on this page.  Make sure you're calling 'TrackPageViewStart' in 'ViewDidAppear' and 'TrackPageViewEnd' in 'ViewDidDisappear'");
				return;
				//throw new InvalidOperationException ("'TrackPageViewStart' was already called on this page.  Make sure you're calling 'TrackPageViewStart' in 'ViewDidAppear' and 'TrackPageViewEnd' in 'ViewDidDisappear'");
			}

			pageNames [hash] = pageName;

			if (properties?.Count > 0)
			{
				pageProperties [hash] = properties;
			}

			pageTime [hash] = Environment.TickCount;
		}


		#endregion


		#region Track Page View End


		public static void TrackPageViewEnd<T> (T page, IDictionary<string, string> properties = null)
			where T : TView
		{
			var hash = page.GetHashCode ();

			trackPageViewEnd (hash, properties);
		}


		static void trackPageViewEnd (int hash, IDictionary<string, string> properties = null)
		{
			var name = string.Empty;

			if (pageNames.TryGetValue (hash, out name))
			{
				var viewName = viewString (name);

				double start, duration, pauseDuration = 0;

				if (pageTime.TryGetValue (hash, out start))
				{
					duration = Environment.TickCount - start;

					// if the page was paused by the user, update the duration
					if (pausedPageDurations.TryGetValue (hash, out pauseDuration) && pauseDuration > 0)
					{
						duration -= pauseDuration;

						// then clear the value
						pausedPageDurations [hash] = 0;
					}

					// convert the duration to seconds
					var seconds = Math.Round ((duration / 1000), 7);


					IDictionary<string, string> allProperties;

					// get the properties passed in when we started tracking the page view
					if (pageProperties.TryGetValue (hash, out allProperties) && allProperties.Any ())
					{
						if (properties?.Count > 0)
						{
							// override values in the dictionary passed in when we started 
							// tracking the page view with values passed in here
							properties.ToList ().ForEach (p => allProperties [p.Key] = p.Value);
						}
					}
					else if (properties?.Count > 0)
					{
						allProperties = properties;
					}
					else
					{
						allProperties = new Dictionary<string, string> ();
					}

					// add the duration property
					allProperties ["duration"] = durationString (seconds);

					// start actually tracking the duration value onec we can pass a value instead of a string
					// allProperties ["duration"] = seconds.ToString ();

					// track the detailed page view
					trackEvent (viewName, allProperties);

					// track a generic page view so we can compare the page views a single event's properties
					trackBasicPageView (name);
				}
				else
				{
					log ($"TrackEvent :: name: {viewName} - 'TrackPageViewStart' was not called on this page.  Make sure you're calling 'TrackPageViewStart' in 'ViewDidAppear' and 'TrackPageViewEnd' in 'ViewDidDisappear'");
					//throw new InvalidOperationException ("'TrackPageViewStart' was not called on this page.  Make sure you're calling 'TrackPageViewStart' in 'ViewDidAppear' and 'TrackPageViewEnd' in 'ViewDidDisappear'");
				}
			}
			else
			{
				log ($"TrackEvent :: name: ??? - A valid page name wasn't provided when 'TrackPageViewStart' was called on this page.  Make sure you're calling 'TrackPageViewStart' in 'ViewDidAppear' and 'TrackPageViewEnd' in 'ViewDidDisappear'");
				//throw new InvalidOperationException ("A valid page name wasn't provided when 'TrackPageViewStart' was called on this page.  Make sure you're calling 'TrackPageViewStart' in 'ViewDidAppear' and 'TrackPageViewEnd' in 'ViewDidDisappear'");
			}

			pageTime [hash] = 0;
		}


		#endregion


		#region Start / End Tracking on Last Page


		public static void StartLastPageEnd ()
		{
			//var mostRecent = pageTime.FirstOrDefault (x => x.Value == pageTime.Values.Max ()).Key;

			if (hashCache != 0)
			{
				var name = string.Empty;

				if (pageNames.TryGetValue (hashCache, out name))
				{
					IDictionary<string, string> properties;

					pageProperties.TryGetValue (hashCache, out properties);

					trackPageViewStart (hashCache, name, properties);
				}
				else
				{
					log ("Could not get Name from hashCashe");
				}
			}
			else
			{
				log ("No hashCashe saved");
			}
		}


		public static void EndLastPageStart ()
		{
			//var mostRecent = pageTime.FirstOrDefault (x => Math.Abs (x.Value - pageTime.Values.Max ()) < double.Epsilon).Key;
			var mostRecent = pageTime.FirstOrDefault (x => x.Value == pageTime.Values.Max ()).Key;

			hashCache = mostRecent;

			trackPageViewEnd (mostRecent);
		}


		#endregion


		#region Pause / Resume Page View Tracking


		public static void PausePageTracking<T> (T page)
			where T : TView
		{
			var hash = page.GetHashCode ();

			pausedPages [hash] = Environment.TickCount;
		}


		public static void ResumePageTracking<T> (T page)
			where T : TView
		{
			var hash = page.GetHashCode ();

			double ticks;

			if (pausedPages.TryGetValue (hash, out ticks) && ticks > 0)
			{
				pausedPageDurations [hash] = Environment.TickCount - ticks;

				pausedPages [hash] = 0;
			}
		}


		#endregion


		static void trackBasicPageView (string pageName)
		{
			trackEvent ("Page View", new Dictionary<string, string> { { "page", pageName } });
		}


		#endregion



		#region Utilities


		static void trackEvent (string name, IDictionary<string, string> properties = null)
		{
			if (!string.IsNullOrEmpty (Constants.PrivateKeys.MobileCenter.AppSecret) && MobileAnalytics.Enabled)
			{
				MobileAnalytics.TrackEvent (name, properties);
			}
			else
			{
				var props = (properties?.Count > 0) ? string.Join (" | ", properties.Select (p => $"{p.Key} = {p.Value}")) : "empty";

				log ($"TrackEvent :: name: {name.PadRight (30)} properties: {props}");
			}
		}


		static string viewString (string pageName) => $"Page View: {pageName}";


		static string durationString (double seconds)
		{
			if (seconds >= 60)
			{
				return "60+ Seconds";
			}
			if (seconds >= 30)
			{
				return "30 - 60 Seconds";
			}
			if (seconds >= 15)
			{
				return "15 - 30 Seconds";
			}
			if (seconds >= 10)
			{
				return "10 - 15 Seconds";
			}
			if (seconds >= 5)
			{
				return "5 - 10 Seconds";
			}
			return "0 - 5 Seconds";
		}


#if DEBUG

		public static void GenerateTestCrash () => Crashes.GenerateTestCrash ();

#endif

		static bool verboseLogging = false;

		static void log (string message, bool onlyVerbose = false)
		{
			if (!onlyVerbose || verboseLogging)
			{
				System.Diagnostics.Debug.WriteLine ($"[Analytics] {message}");
			}
		}

		#endregion
	}
}
