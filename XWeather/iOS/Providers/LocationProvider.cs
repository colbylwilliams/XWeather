using System;
using System.Linq;
using System.Threading.Tasks;

using CoreLocation;

using XWeather.Domain;
using XWeather.Unified;

#if DEBUG
using static System.Diagnostics.Debug;
#else
using static System.Console;
#endif

namespace XWeather.iOS
{
	public class LocationProvider
	{
		TaskCompletionSource<CLLocation> ClLocationTcs;
		TaskCompletionSource<CLAuthorizationStatus> ClAuthTcs;


		readonly CLLocationManager ClLocationManager = new CLLocationManager { DesiredAccuracy = CLLocation.AccuracyHundredMeters, DistanceFilter = 100 /*meters*/};


		public CLLocation Location { get; set; }


		public LocationProvider ()
		{
			ClLocationManager.ShouldDisplayHeadingCalibration += m => false;
		}


		public async Task<LocationCoordinates> GetCurrentLocationCoordnatesAsync ()
		{
			var location = await GetCurrentLocationAsync ();

			if (location != null)
				return new LocationCoordinates { Latitude = location.Coordinate.Latitude, Longitude = location.Coordinate.Longitude };

			return null;
		}


		public async Task<CLLocation> GetCurrentLocationAsync ()
		{
			Log ("GetCurrentLocationAsync");

			if (!ClLocationTcs.IsNullFinishCanceledOrFaulted ()) {
				return await ClLocationTcs.Task;
			}

			ClLocationTcs = new TaskCompletionSource<CLLocation> ();


			var status = CLLocationManager.Status;

			if (status == CLAuthorizationStatus.NotDetermined) status = await getAuthorizationStatusAsync ();

			Log ($"status: {status}");


			if (CLLocationManager.LocationServicesEnabled && status == CLAuthorizationStatus.AuthorizedWhenInUse) {

				ClLocationManager.LocationsUpdated += handleLocationsUpdated;

				ClLocationManager.StartUpdatingLocation ();

				Log ("StartUpdatingLocation");

			} else {

				ClLocationTcs.SetResult (null);
			}

			return await ClLocationTcs.Task;
		}


		Task<CLAuthorizationStatus> getAuthorizationStatusAsync ()
		{
			Log ("getAuthorizationStatusAsync");

			if (!ClAuthTcs.IsNullFinishCanceledOrFaulted ()) {
				return ClAuthTcs.Task;
			}

			ClAuthTcs = new TaskCompletionSource<CLAuthorizationStatus> ();

			ClLocationManager.AuthorizationChanged += handleAuthorizationChanged;

			ClLocationManager.RequestWhenInUseAuthorization ();

			return ClAuthTcs.Task;
		}


		void handleLocationsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			Log ("handleLocationsUpdated");

			ClLocationManager.LocationsUpdated -= handleLocationsUpdated;
			ClLocationManager.StopUpdatingLocation ();

			Log ("StopUpdatingLocation");

			// If it's a relatively recent event, turn off updates to save power.
			var location = e.Locations.LastOrDefault ();

			if (location != null) {

				var timestamp = location.Timestamp.ToDateTime ();

				var timedelta = DateTime.UtcNow.Subtract (timestamp);

				var timeDeltaSeconds = Math.Abs (timedelta.TotalSeconds);

				Log ($"Location is {Math.Abs (timedelta.TotalSeconds)} seconds old");

				// location was retrieved less than 15 seconds ago
				if (timeDeltaSeconds < 15) {

					Location = location;

					if (!ClLocationTcs.TrySetResult (location)) {

						Log ("ClLocationTcs Failed to Set Result");

					} else {

						Log ("ClLocationTcs Set Result");
					}

				} else {

					Log ($"Location was too old: ({timedelta})");

					ClLocationManager.LocationsUpdated += handleLocationsUpdated;

					ClLocationManager.StartUpdatingLocation ();

					Log ("StartUpdatingLocation");
				}
			}
		}


		void handleAuthorizationChanged (object sender, CLAuthorizationChangedEventArgs e)
		{
			Log ("handleAuthorizationChanged");

			if (e.Status == CLAuthorizationStatus.NotDetermined) return;

			ClLocationManager.AuthorizationChanged -= handleAuthorizationChanged;

			if (!ClAuthTcs.TrySetResult (e.Status)) {
				Log ("ClAuthTcs Failed to Set Result");
			} else {
				Log ("ClAuthTcs Set Result");
			}
		}


		public async Task<CLPlacemark> ReverseGeocodeLocation (CLLocation location)
		{
			var geocoder = new CLGeocoder ();

			var placemarks = await geocoder.ReverseGeocodeLocationAsync (location);

			return placemarks?.FirstOrDefault ();
		}

		bool log = true;

		void Log (string message) { if (log) WriteLine ($"[LocationProvider] {message}"); }
	}
}