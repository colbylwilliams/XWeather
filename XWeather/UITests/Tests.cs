using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XWeather.UITests
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	public class Tests
	{
		bool iOS => platform == Platform.iOS;

		IApp app;
		Platform platform;


		public Tests (Platform platform)
		{
			this.platform = platform;
		}


		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);
		}


		[Test]
		public void AppLoadsWeatherData ()
		{
			app.Screenshot ("App Launched");

			app.WaitForElement (x => x.Id (iOS ? "label_1" : "DailyListItem_dayLabel").Index (2), "Timed out waiting for weather data from Weather Underground");

			app.Screenshot ("Daily Forecast (Weather Data Loaded)");

			app.SwipeRightToLeft ();

			app.Screenshot ("Hourly Forecast");

			app.SwipeRightToLeft ();

			app.Screenshot ("Detailed Conditions ");


			if (platform == Platform.Android) return;

			// comment
			app.Tap (x => x.Id (UIElements.WeatherPvc.button_locations));

			app.Screenshot ("Locations Selection");

			app.ScrollUp (x => x.Class ("UITableView").Index (1));

			app.Screenshot ("Locations Search");

			app.EnterText ("Search", "San Fr");

			app.Screenshot ("Locations Search: 'San Fr'");

			app.Tap (x => x.Marked ("Cancel"));

			app.Screenshot ("Locations Selection");

			app.Tap (x => x.Id (UIElements.WeatherPvc.button_close));

			app.Screenshot ("Detailed Conditions");
		}
	}
}