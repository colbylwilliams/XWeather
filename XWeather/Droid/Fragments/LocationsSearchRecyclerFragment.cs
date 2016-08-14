using System.Threading.Tasks;

using Android.App;
using Android.Views;
using Android.Widget;

using XWeather.Clients;
using XWeather.Domain;

using SettingsStudio;

namespace XWeather.Droid
{
	public class LocationsSearchRecyclerFragment : ListFragment
	{

		public LocationSearchRecyclerAdapter FilterAdapter { get; set; }


		public LocationsSearchRecyclerFragment (Activity activity)
		{
			FilterAdapter = new LocationSearchRecyclerAdapter (activity);
		}


		public override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			ListAdapter = FilterAdapter;
		}


		public override void OnListItemClick (ListView l, View v, int position, long id)
		{
			var location = FilterAdapter [position];

			Task.Run (async () => {

				await WuClient.Shared.AddLocation (location);

				Settings.LocationsJson = WuClient.Shared.Locations.GetLocationsJson ();

				Activity?.RunOnUiThread (() => { });
			});


			base.OnListItemClick (l, v, position, id);
		}
	}
}