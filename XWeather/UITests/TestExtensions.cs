using System;

using Xamarin.UITest;

namespace XWeather.UITests
{
	public static class TestExtensions
	{
		public static void SearchFor (this IApp app, Platform platform, string searchString)
		{
			//app.ScrollUp (x => x.Class ("UITableView").Index (1), ScrollStrategy.Gesture);

			var ios = platform == Platform.iOS;

			app.Tap (x => x.Id (ios ? "button_add" : "action_search"));

			app.Screenshot ("Start Search");

			try {

				app.EnterText (searchString);

			} catch (Exception) {

				app.EnterText (ios ? "Search" : "search_src_text", searchString);

			}

			app.Screenshot ($"Locations Search: '{searchString}'");
		}


		public static void SearchForAndSelect (this IApp app, Platform platform, string searchString, string selection, string waitFor)
		{
			app.SearchFor (platform, searchString);

			app.Tap (x => x.Marked (selection));

			app.WaitForElement (x => x.Marked (waitFor));

			app.Screenshot ($"Added Location: '{selection}'");
		}


		public static void UpdateSetting (this IApp app, Platform platform, string settingTitle, string selection)
		{
			var ios = platform == Platform.iOS;

			try {

				app.Tap (x => x.Marked (settingTitle));

			} catch (Exception) {

				app.ScrollDown ();

				app.Tap (x => x.Marked (settingTitle));
			}

			app.Screenshot ($"Setting Change: '{settingTitle}'");

			app.Tap (x => x.Marked (selection));
		}
	}
}