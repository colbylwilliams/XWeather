using Android.Views;
using Android.Widget;

namespace XWeather.Droid
{
	public class LocationsHeadHolder : ViewHolder<string>
	{
		TextView calloutLabel;

		public LocationsHeadHolder (View itemView) : base (itemView) { }


		public static LocationsHeadHolder Create (View rootView) => new LocationsHeadHolder (rootView);


		public override void FindViews (View rootView)
		{
			calloutLabel = (TextView)rootView.FindViewById (Resource.Id.LocationsHeader_calloutLabel);
		}

		public override void SetData (string data)
		{
			calloutLabel.SetText (data, TextView.BufferType.Normal);
		}
	}
}