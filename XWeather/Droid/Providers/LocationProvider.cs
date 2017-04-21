using System;
using System.Threading.Tasks;

using Android.Gms.Location;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Locations;
using Android.App;

using XWeather.Domain;

#if DEBUG
using static System.Diagnostics.Debug;
#else
using static System.Console;
#endif

namespace XWeather.Droid
{
	public class LocationProvider : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
	{
		TaskCompletionSource<bool> ConnectTcs;
		TaskCompletionSource<Location> LocationTcs;

		readonly GoogleApiClient apiClient;

		LocationRequest locRequest;

		public Location Location { get; set; }

		Activity Context;

		public LocationProvider (Activity context)
		{
			Context = context;

			//apiClient = new GoogleApiClient.Builder (Android.App.Application.Context, this, this).AddApi (LocationServices.API).Build ();
			apiClient = new GoogleApiClient.Builder (Android.App.Application.Context, this, this).AddApi (LocationServices.API).Build ();

			locRequest = LocationRequest.Create ();

			// Setting location priority to PRIORITY_HIGH_ACCURACY (100)
			locRequest.SetPriority (100);

			// Setting interval between updates, in milliseconds
			// NOTE: the default FastestInterval is 1 minute. If you want to receive location updates more than 
			// once a minute, you _must_ also change the FastestInterval to be less than or equal to your Interval
			locRequest.SetFastestInterval (500);
			locRequest.SetInterval (1000);

			WriteLine ($"Request priority set to status code {locRequest.Priority}, interval set to {locRequest.Interval} ms");
		}


		public async Task<LocationCoordinates> GetCurrentLocationCoordnatesAsync ()
		{
			var location = await GetCurrentLocationAsync ();

			if (location != null)
				return new LocationCoordinates { Latitude = location.Latitude, Longitude = location.Longitude };

			return null;
		}


		public async Task<Location> GetCurrentLocationAsync ()
		{
			if (!LocationTcs.IsNullFinishCanceledOrFaulted ()) {
				return await LocationTcs.Task;
			}

			LocationTcs = new TaskCompletionSource<Location> ();

			var connected = apiClient.IsConnected ? apiClient.IsConnected : await connect ();

			if (connected) {
				
				var lastLocation = LocationServices.FusedLocationApi.GetLastLocation (apiClient);

				if (lastLocation?.CheckLocationTime (15) ?? false) {
					
					LocationTcs.SetResult (lastLocation);

					//return await Task.FromResult (lastLocation);
				}

				// pass in a location request and LocationListener
				Context.RunOnUiThread (async () => await LocationServices.FusedLocationApi.RequestLocationUpdates (apiClient, locRequest, this));
			
			} else {
			
				LocationTcs.SetResult (null);

				WriteLine ("Unable to connect, returning null");
			}

			return await LocationTcs.Task;
		}


		Task<bool> connect ()
		{
			if (!ConnectTcs.IsNullFinishCanceledOrFaulted ()) {
				return ConnectTcs.Task;
			}

			ConnectTcs = new TaskCompletionSource<bool> ();

			if (apiClient.IsConnected) {
				
				ConnectTcs.SetResult (true);
			
			} else {
			
				apiClient.Connect ();
			}

			return ConnectTcs.Task;
		}



		public void OnConnected (Bundle connectionHint)
		{
			ConnectTcs.SetResult (apiClient.IsConnected);

			WriteLine ("Now connected to client");
		}

		public void OnConnectionFailed (ConnectionResult result)
		{
			ConnectTcs.SetResult (apiClient.IsConnected);

			WriteLine ("Connection failed, attempting to reach google play services");
		}

		public void OnConnectionSuspended (int cause)
		{
			WriteLine ("Connection suspended");
		}


		public void OnLocationChanged (Location location)
		{
			WriteLine ("Location changed");

			if (location != null) {
		
				// location was retrieved less than 15 seconds ago
				if (location.CheckLocationTime (15)) {

					Location = location;
		
					if (!LocationTcs.IsNullFinishCanceledOrFaulted ()) {
		
						if (!LocationTcs.TrySetResult (location)) {
		
							WriteLine ("LocationTcs Failed to Set Result");
		
						} else {
		
							WriteLine ("LocationTcs Set Result");
						}
					}

					if (apiClient.IsConnected) {
						
						// stop location updates, passing in the LocationListener
						Task.Run (async () => {
							await LocationServices.FusedLocationApi.RemoveLocationUpdates (apiClient, this);
							apiClient.Disconnect ();
						});
					}
				}
			}
		}
	}

	public static class LocationExtensions
	{
		static readonly DateTime baseTime = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static bool CheckLocationTime (this Location location, double maxSeconds)
		{
			var timestamp = baseTime.AddMilliseconds (location.Time);

			var timedelta = DateTime.UtcNow.Subtract (timestamp);

			var timeDeltaSeconds = Math.Abs (timedelta.TotalSeconds);

			WriteLine ($"Location is {timeDeltaSeconds} seconds old");

			return timeDeltaSeconds <= maxSeconds;
		}
	}
}