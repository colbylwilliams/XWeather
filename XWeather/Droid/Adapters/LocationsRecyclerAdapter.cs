using System.Collections.Generic;

using Android.Views;

using XWeather.Clients;

namespace XWeather.Droid
{
	public class LocationsRecyclerAdapter : BaseRecyclerAdapter<WuLocation, LocationViewHolder, string, LocationsHeadHolder>
	{
		public override IList<WuLocation> DataSet => WuClient.Shared.Locations;

		public override string HeadData => "Search to add locaitons";

		public LocationsRecyclerAdapter (int cellResource, int headResource) : base (cellResource, headResource) { }

		protected override LocationViewHolder CreateCellViewHolder (View rootView) => LocationViewHolder.Create (rootView);

		protected override LocationsHeadHolder CreateHeadViewHolder (View rootView) => LocationsHeadHolder.Create (rootView);
	}
}