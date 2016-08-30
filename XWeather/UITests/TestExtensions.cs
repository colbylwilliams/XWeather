using System;

using Xamarin.UITest;

namespace XWeather.UITests
{
	public static class TestExtensions
	{
		public static void SearchFor (this IApp app, Platform platform, string searchString)
		{
			var ios = platform == Platform.iOS;

			app.Tap (x => x.Id (ios ? "button_add" : "action_search"));

			app.Screenshot ("Start Search");


			try {

				// type in each char one at a time
				foreach (var item in searchString)
					app.EnterText (item.ToString ());

			} catch (Exception) {

				// clear and try again
				app.ClearText ();

				foreach (var item in searchString)
					app.EnterText (ios ? "Search" : "search_src_text", item.ToString ());

			}

			app.Screenshot ($"Locations Search: '{searchString}'");
		}


		public static void SearchForAndSelect (this IApp app, Platform platform, string searchString, string selection, string waitFor)
		{
			app.SearchFor (platform, searchString);

			app.WaitForElement (x => x.Marked (selection));


			app.Tap (x => x.Marked (selection));

			app.WaitForElement (x => x.Marked (waitFor));


			app.Screenshot ($"Added Location: '{selection}'");
		}


		public static void UpdateAndroidSetting (this IApp app, string settingTitle, string selection)
		{
			try {

				app.Tap (x => x.Marked (settingTitle));

			} catch (Exception) {

				// may be below the fold, scroll down and try agian
				app.ScrollDown ();

				app.Tap (x => x.Marked (settingTitle));
			}

			app.Screenshot ($"Setting Change: '{settingTitle}'");

			app.Tap (x => x.Marked (selection));
		}


		public static void SwipeRightToLeftWithIdCheck (this IApp app, string waitFor)
		{
			app.SwipeRightToLeft ();

			try {

				app.WaitForElement (x => x.Id (waitFor).Index (2), $"Timed out waiting element with ID: {waitFor}");

			} catch (Exception) {

				app.SwipeRightToLeft (swipeSpeed: 800);

				app.WaitForElement (x => x.Id (waitFor).Index (2), $"Timed out waiting element with ID: {waitFor}");
			}
		}


		public static void SwipeLeftToRightWithIdCheck (this IApp app, string waitFor)
		{
			app.SwipeLeftToRight ();

			try {

				app.WaitForElement (x => x.Id (waitFor).Index (2), $"Timed out waiting element with ID: {waitFor}");

			} catch (Exception) {

				app.SwipeLeftToRight (swipeSpeed: 800);

				app.WaitForElement (x => x.Id (waitFor).Index (2), $"Timed out waiting element with ID: {waitFor}");
			}
		}
	}
}