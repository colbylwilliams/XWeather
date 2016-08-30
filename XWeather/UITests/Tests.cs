using NUnit.Framework;

using Xamarin.UITest;

namespace XWeather.UITests
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	public class Tests
	{
		bool iOS => platform == Platform.iOS;

		IApp app;

		readonly Platform platform;

		public Tests (Platform platform)
		{
			this.platform = platform;
		}


		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);
		}


		//[Test]
		//public void repl () => app.Repl ();


		[Test]
		public void AppLoadsWeatherData ()
		{
			app.Screenshot ("App Launched");

			app.WaitForElement (x => x.Id (iOS ? "DailyTvCell_dayLabel" : "DailyListItem_dayLabel").Index (2), "Timed out waiting for weather data from Weather Underground");

			app.Screenshot ("Daily Forecast (Weather Data Loaded)");

			app.SwipeRightToLeftWithIdCheck (iOS ? "HourlyTvCell_hourLabel" : "HourlyListItem_hourLabel");

			app.Screenshot ("Hourly Forecast");

			app.SwipeRightToLeftWithIdCheck (iOS ? "DetailsTvCell_valueLabel" : "DetailsListItem_valueLabel");

			app.Screenshot ("Detailed Conditions ");

			app.Tap (x => x.Id (iOS ? "button_locations" : "floatingButton"));

			app.Screenshot ("Locations Selection");

			app.SearchFor (platform, "San Fr");

			if (iOS)
				app.Tap (x => x.Marked ("Cancel"));
			else {
				app.Back (); // dismiss keyboard
				app.Back (); // cancel search
			}

			app.Screenshot ("Locations Selection");

			if (iOS)
				app.Tap (x => x.Id ("button_close"));
			else
				app.Back ();

			app.Screenshot ("Detailed Conditions");
		}


		[Test]
		public void AutoCompleteSearch ()
		{
			app.Screenshot ("App Launched");

			app.WaitForElement (x => x.Id (iOS ? "DailyTvCell_dayLabel" : "DailyListItem_dayLabel").Index (2), "Timed out waiting for weather data from Weather Underground");

			app.Tap (x => x.Id (iOS ? "button_locations" : "floatingButton"));

			app.SearchForAndSelect (platform, "San Fr", "San Francisco, California", "San Francisco");

			app.SearchForAndSelect (platform, "Atlan", "Atlanta, Georgia", "Atlanta");

			app.SearchForAndSelect (platform, "Toron", "Toronto, Canada", "Toronto");

			app.SearchForAndSelect (platform, "Lond", "London, United Kingdom", "London");

			app.Tap (x => x.Marked ("London"));

			app.Screenshot ("Selected 'London'");
		}


		[Test]
		public void ChangeUomSettings ()
		{
			app.Screenshot ("App Launched");

			app.WaitForElement (x => x.Id (iOS ? "DailyTvCell_dayLabel" : "DailyListItem_dayLabel").Index (2), "Timed out waiting for weather data from Weather Underground");

			app.Screenshot ("Daily Forecast (Weather Data Loaded) (Imperial)");

			app.SwipeRightToLeftWithIdCheck (iOS ? "HourlyTvCell_hourLabel" : "HourlyListItem_hourLabel");

			app.Screenshot ("Hourly Forecast (Imperial)");

			app.SwipeRightToLeftWithIdCheck (iOS ? "DetailsTvCell_valueLabel" : "DetailsListItem_valueLabel");

			app.Screenshot ("Detailed Conditions (Imperial)");

			app.Tap (x => x.Id (iOS ? "button_locations" : "floatingButton"));


			if (iOS) {

				app.Invoke ("updateSettingsToImperial");

			} else {

				app.Tap (x => x.Id (iOS ? "button_settings" : "action_settings"));

				app.Screenshot ("Settings (Imperial)");

				app.UpdateAndroidSetting ("Temperature", "Celsius");

				app.UpdateAndroidSetting ("Distance", "Kilometers");

				app.UpdateAndroidSetting ("Pressure", "Millibars");

				app.UpdateAndroidSetting ("Length", "Millimeters");

				app.UpdateAndroidSetting ("Speed", "Kilometers per hour");

				app.Screenshot ("Settings (Metric)");

				app.Back ();
			}

			if (iOS)
				app.Tap (x => x.Id ("button_close"));
			else
				app.Back ();


			app.Screenshot ("Detailed Conditions (Metric)");

			app.SwipeLeftToRightWithIdCheck (iOS ? "HourlyTvCell_hourLabel" : "HourlyListItem_hourLabel");

			app.Screenshot ("Hourly Forecast (Metric)");

			app.SwipeLeftToRightWithIdCheck (iOS ? "DailyTvCell_dayLabel" : "DailyListItem_dayLabel");

			app.Screenshot ("Daily Forecast (Metric)");
		}
	}
}