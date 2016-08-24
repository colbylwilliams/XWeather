using System;

using Xamarin.UITest;

namespace XWeather.UITests
{
	public static class TestExtensions
	{

		public static void SearchFor (this IApp app, string searchString)
		{
			app.ScrollUp (x => x.Class ("UITableView").Index (1), ScrollStrategy.Gesture);

			app.Screenshot ("Scroll Up to Search");

			try {

				app.EnterText (searchString);

			} catch (Exception) {

				app.EnterText ("Search", searchString);

			}

			app.Screenshot ($"Locations Search: '{searchString}'");
		}


		public static void SearchForAndSelect (this IApp app, string searchString, string selection, string waitFor)
		{
			app.SearchFor (searchString);

			app.Tap (x => x.Marked (selection));

			app.WaitForElement (x => x.Marked (waitFor));

			app.Screenshot ($"Added Location: '{selection}'");
		}
	}
}