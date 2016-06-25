#if __IOS__

using UNStoryboard = UIKit.UIStoryboard;
using UNViewController = UIKit.UIViewController;

#else

using UNStoryboard = AppKit.NSStoryboard;
using UNViewController = AppKit.NSViewController;

#endif

namespace XWeather.Unified
{
	public static class StoryboardExtensions
	{
		public static T Instantiate<T> (this UNStoryboard storyboard)
			where T : UNViewController
			=> storyboard.InstantiateViewController (typeof (T).Name) as T;


		public static T Instantiate<T> (this UNStoryboard storyboard, string name)
			where T : UNViewController
			=> storyboard.InstantiateViewController (name) as T;


		public static UNViewController Instantiate (this UNStoryboard storyboard, string name)
			=> storyboard.InstantiateViewController (name) as UNViewController;
	}
}