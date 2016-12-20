using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XWeather.Clients;
using XWeather.Domain;
using XWeather.UWP.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XWeather.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

			getData ();
        }


		void getData ()
		{
			//if (LocationProvider == null) LocationProvider = new LocationProvider ();

			//UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

			System.Threading.Tasks.Task.Run (async () => {

				//var location = await LocationProvider.GetCurrentLocationCoordnatesAsync ();
				var location = new LocationCoordinates { Latitude = 30.332313, Longitude = -87.13817 };

				//await WuClient.Shared.GetLocations (Settings.LocationsJson, location);
				await WuClient.Shared.GetLocations (null, location);

				await Dispatcher.RunAsync (Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
				{
					DailyLv.ItemsSource = WuClient.Shared.Selected.Forecasts.Select(f => new DailyForecastItem(f)).ToList();
					HourlyLv.ItemsSource = WuClient.Shared.Selected.HourlyForecast (DateTime.Now.Day).Select(h => new HourlyForecastItem(h)).ToList();
					DetailsLv.ItemsSource = WeatherDetails.GetDetails (WuClient.Shared.Selected, TemperatureUnits.Fahrenheit, SpeedUnits.MilesPerHour, LengthUnits.Inches, DistanceUnits.Miles, PressureUnits.InchesOfMercury).Select(d => new ForecastDetailsItem(d)).ToList();
				});
			});
		}
	}
}
