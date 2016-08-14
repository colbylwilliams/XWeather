using Android.Views;
using XWeather.Clients;

namespace XWeather.Droid
{
	public class LocationsRecyclerFragment : RecyclerViewFragment<WuLocation, LocationViewHolder, string, LocationsHeadHolder>
	{

		public static LocationsRecyclerFragment Create () => new LocationsRecyclerFragment ();


		protected override BaseRecyclerAdapter<WuLocation, LocationViewHolder, string, LocationsHeadHolder> GetAdapter ()
			=> new LocationsRecyclerAdapter (Resource.Layout.LocationListItem, Resource.Layout.LocationsHeader);


		protected override void OnItemClick (View view, int position)
		{
			// set location as the selected location
			WuClient.Shared.Selected = WuClient.Shared.Locations [position];

			//DismissViewController (true, null);
		}

		//public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		//{
		//	base.OnCreateOptionsMenu (menu, inflater);
		//}
	}
}