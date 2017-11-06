using System.Threading.Tasks;

using Android.App;
using Android.OS;
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


		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			ListAdapter = FilterAdapter;
		}


		public override void OnResume ()
		{
			base.OnResume ();

			Analytics.TrackPageViewStart (this, Pages.LocationSearch);
		}


		public override void OnPause ()
		{
			Analytics.TrackPageViewEnd (this);

			base.OnPause ();
		}


		public override void OnListItemClick (ListView l, View v, int position, long id)
		{
			var location = FilterAdapter [position];

			Task.Run (async () =>
			{
				await WuClient.Shared.AddLocation (location);

				Activity?.RunOnUiThread (() => { });
			});


			base.OnListItemClick (l, v, position, id);
		}
	}
}