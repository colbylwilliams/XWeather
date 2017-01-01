using System.Collections.Generic;

#if __IOS__
using TView = UIKit.UIViewController;
#elif __ANDROID__
// something random that includes both Activity and Fragment
using TView = Android.Views.View.IOnCreateContextMenuListener;
#endif


namespace XWeather
{
	public static partial class Analytics
	{

		public static void TrackEvent (Events evnt, IDictionary<string, string> properties = null)
		{
			trackEvent (evnt.Name (), properties);
		}


		public static void TrackPageViewStart<T> (T page, Pages pageType, IDictionary<string, string> properties = null)
			where T : TView
		{
			TrackPageViewStart (page, pageType.Name (), properties);
		}


		public static void TrackPageViewStart<T> (T page, Pages pageType, WuLocation location)
			where T : TView
		{
			TrackPageViewStart (page, pageType.Name (), location == null ? null : new Dictionary<string, string> {
				//{ "location", location.Name },
				{ "current" , location.Current.ToString() }
			});
		}


		public static void TrackPageViewEnd<T> (T page, WuLocation location)
			where T : TView
		{
			TrackPageViewEnd (page, location == null ? null : new Dictionary<string, string> {
				//{ "location", location.Name },
				{ "current" , location.Current.ToString() }
			});
		}
	}
}