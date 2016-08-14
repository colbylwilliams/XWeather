using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;

using XWeather.Domain;


namespace XWeather.Droid
{
	public class LocationSearchRecyclerAdapter : BaseAdapter<WuAcLocation>, IFilterable
	{
		readonly Activity Activity;

		LocationSearchFilter _filter;
		public Filter Filter => _filter ?? (_filter = new LocationSearchFilter (Activity, this));


		public LocationSearchRecyclerAdapter (Activity activity)
		{
			Activity = activity;
		}


		public override WuAcLocation this [int position] => _filter.LocationResults.Count < position + 1 ? null : _filter.LocationResults [position];


		public override int Count => _filter.LocationResults.Count;


		public override long GetItemId (int position) => position;


		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View cell = convertView;

			if (cell == null || !(cell is LinearLayout)) {
				cell = Activity.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem1, parent, false);
				//cell.SetBackgroundColor (Activity.Resources.GetColor (Resource.Color.theme_grey19));
			}

			var span = _filter.ResultStrings.Count < position + 1 ? new SpannableString (" ") : _filter.ResultStrings [position];

			cell.FindViewById<TextView> (Android.Resource.Id.Text1).SetText (span, TextView.BufferType.Spannable);

			return cell;
		}
	}
}