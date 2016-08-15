using System.Collections.Generic;

using Android.Views;

using XWeather.Clients;


namespace XWeather.Droid
{
	public class LocationsRecyclerAdapter : BaseSelectableRecyclerAdapter<WuLocation, LocationViewHolder>
	{
		public override IList<WuLocation> DataSet => WuClient.Shared.Locations;


		public LocationsRecyclerAdapter (int cellResource) : base (cellResource) { }


		protected override LocationViewHolder CreateCellViewHolder (View rootView) => LocationViewHolder.Create (rootView);


		public override void RemoveSelectedItems ()
		{
			foreach (var item in SelectedDataSet) {

				if (DataSet.Contains (item)) {

					if (!item.Current) {

						DataSet.Remove (item);
					}
				}
			}

			NotifyDataSetChanged ();
		}
	}
}