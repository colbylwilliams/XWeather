using System;
using System.Threading.Tasks;

using Android.App;
using Android.Gms.Maps;
using Android.OS;
using Android.Gms.Maps.Model;

using XWeather.Clients;

namespace XWeather.Droid
{
	public class RadarMapActivity : BaseActivity, IOnMapReadyCallback
	{
		WuLocation Location => WuClient.Shared.Selected;

		// boom shakalaka
		GoogleMap map;
		MapFragment mapFragment;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.MapRadarActivity);
		}


		protected override void OnResume ()
		{
			base.OnResume ();

			Analytics.TrackPageViewStart (this, Pages.WeatherRadar);

			setupMapIfNeeded ();
		}


		protected override void OnPause ()
		{
			Analytics.TrackPageViewEnd (this);

			base.OnPause ();

			map.MyLocationEnabled = false;

		}


		void initMapFragment ()
		{
			mapFragment = FragmentManager.FindFragmentByTag ("map") as MapFragment;

			if (mapFragment == null)
			{
				GoogleMapOptions mapOptions = new GoogleMapOptions ()
					.InvokeMapType (GoogleMap.MapTypeSatellite)
					.InvokeZoomControlsEnabled (false)
					.InvokeCompassEnabled (true);

				FragmentTransaction fragTx = FragmentManager.BeginTransaction ();

				mapFragment = MapFragment.NewInstance (mapOptions);

				fragTx.Add (Resource.Id.MapRadar_container, mapFragment, "map").Commit ();
			}
		}


		void setupMapIfNeeded ()
		{
			if (map == null)
			{
				mapFragment.GetMapAsync (this);
			}
			else
			{
				//map.MyLocationEnabled = true;
			}
		}


		public void OnMapReady (GoogleMap googleMap)
		{
			if (map != null)
			{
				// Animate the move on the map so that it is showing the markers we added above.
				map.AnimateCamera (CameraUpdateFactory.NewLatLngZoom (new LatLng (Location.Location.lat, Location.Location.lon), 2));

				//map.MyLocationEnabled = true;
			}
		}
	}
}