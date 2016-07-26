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


			app.SearchFor ("San Fr");


			app.Tap (x => x.Marked ("Cancel"));

			app.Screenshot ("Locations Selection");


			app.Tap (x => x.Id (UIElements.WeatherPvc.button_close));

			app.Screenshot ("Detailed Conditions");
		}


		[Test]
		public void AutoCompleteSearch ()
		{
			app.Tap (x => x.Id ("button_locations"));


			app.WaitForElement (x => x.Class ("LocationTvCell"));


			app.SearchForAndSelect ("San Fr", "San Francisco, California", "San Francisco");


			app.SearchForAndSelect ("Atlan", "Atlanta, Georgia", "Atlanta");


			app.SearchForAndSelect ("Toron", "Toronto, Canada", "Toronto");


			app.SearchForAndSelect ("Lond", "London, United Kingdom", "London");


			//app.SearchForAndSelect ("Sao Pa", "Sao Paulo, Brazil", "Sao Paulo");


			app.Tap (x => x.Marked ("London"));

			app.Screenshot ("Selected 'London'");
		}
	}
}