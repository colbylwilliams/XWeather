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


		//#if DEBUG

		[Test]
		public void AutoCompleteSearch ()
		{
			app.Tap (x => x.Id ("button_locations"));

			app.WaitForElement (x => x.Class ("LocationTvCell"));


			app.ScrollUp (x => x.Class ("UITableView").Index (1)); ;

			app.Screenshot ("Scroll Up to Search");

			app.EnterText ("Search", "San Fr");

			app.Screenshot ("Locations Search: 'San Fr'");

			app.Tap (x => x.Marked ("San Francisco, California"));

			app.WaitForElement (x => x.Marked ("San Francisco"));

			app.Screenshot ("Added Location: 'San Francisco, California'");


			app.ScrollUp (x => x.Class ("UITableView").Index (1));

			app.EnterText ("Search", "Atlan");

			app.Screenshot ("Locations Search: 'Atlan'");

			app.Tap (x => x.Marked ("Atlanta, Georgia"));

			app.WaitForElement (x => x.Marked ("Atlanta"));

			app.Screenshot ("Added Location: 'Atlanta, Georgia'");


			app.ScrollUp (x => x.Class ("UITableView").Index (1));

			app.EnterText ("Search", "Toron");

			app.Screenshot ("Locations Search: 'Toron'");

			app.Tap (x => x.Marked ("Toronto, Canada"));

			app.WaitForElement (x => x.Marked ("Toronto"));

			app.Screenshot ("Added Location: 'Toronto, Canada'");


			app.ScrollUp (x => x.Class ("UITableView").Index (1));

			app.EnterText ("Search", "Lond");

			app.Screenshot ("Locations Search: 'Lond'");

			app.Tap (x => x.Marked ("London, United Kingdom"));

			app.WaitForElement (x => x.Marked ("London"));

			app.Screenshot ("Added Location: 'London, United Kingdom'");



			//app.ScrollUp (x => x.Class ("UITableView").Index (1));

			//app.EnterText ("Search", "Sao Pa");

			//app.Screenshot ("Locations Search: 'Sao Pa'");

			//app.Tap (x => x.Marked ("Sao Paulo, Brazil"));

			//app.WaitForElement (x => x.Marked ("Sao Paulo"));

			//app.Screenshot ("Added Location: 'Sao Paulo, Brazil'");


			app.Tap (x => x.Marked ("London"));

			app.Screenshot ("Selected 'London'");
		}
		//#endif
	}
}