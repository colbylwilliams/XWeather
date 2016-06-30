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

			app.WaitForElement (x => x.Text ("Wednesday"), "Timed out waiting for weather data from Weather Underground");

			app.Screenshot ("Daily Forecast (Weather Data Loaded)");

			app.SwipeRightToLeft ();

			app.Screenshot ("Hourly Forecast");

			app.SwipeRightToLeft ();

			app.Screenshot ("Detailed Conditions");
		}
	}
}