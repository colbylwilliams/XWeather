using Android.Views;
using Android.Widget;

using SettingsStudio;

namespace XWeather.Droid
{
	public class LocationViewHolder : ViewHolder<WuLocation>
	{
		TextView nameLabel;
		TextView tempLabel;
		TextView timeLabel;

		public LocationViewHolder (View itemView) : base (itemView) { }


		public static LocationViewHolder Create (View rootView) => new LocationViewHolder (rootView);


		public override void FindViews (View rootView)
		{
			nameLabel = (TextView)rootView.FindViewById (Resource.Id.LocationListItem_nameLabel);
			tempLabel = (TextView)rootView.FindViewById (Resource.Id.LocationListItem_tempLabel);
			timeLabel = (TextView)rootView.FindViewById (Resource.Id.LocationListItem_timeLabel);
		}


		public override void SetData (WuLocation data)
		{
			nameLabel.SetText (data?.Name, TextView.BufferType.Normal);
			tempLabel.SetText (data?.TempString (Settings.UomTemperature, true), TextView.BufferType.Normal);
			timeLabel.SetText (data?.LocalTimeString (), TextView.BufferType.Normal);
		}
	}
}