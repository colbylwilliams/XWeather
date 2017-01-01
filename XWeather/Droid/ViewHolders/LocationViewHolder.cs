using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

using SettingsStudio;

namespace XWeather.Droid
{
	public class LocationViewHolder : SelectableViewHolder<WuLocation>
	{
		TextView nameLabel;
		TextView tempLabel;
		TextView timeLabel;
		LinearLayout container;


		public LocationViewHolder (View itemView) : base (itemView) { }


		public static LocationViewHolder Create (View rootView) => new LocationViewHolder (rootView);


		public override void FindViews (View rootView)
		{
			nameLabel = (TextView)rootView.FindViewById (Resource.Id.LocationListItem_nameLabel);
			tempLabel = (TextView)rootView.FindViewById (Resource.Id.LocationListItem_tempLabel);
			timeLabel = (TextView)rootView.FindViewById (Resource.Id.LocationListItem_timeLabel);
			container = (LinearLayout)rootView.FindViewById (Resource.Id.LocationListItem_container);
		}


		public override void SetData (WuLocation data)
		{
			nameLabel.SetText (data?.Name, TextView.BufferType.Normal);
			tempLabel.SetText (data?.TempString (Settings.UomTemperature, true), TextView.BufferType.Normal);
			timeLabel.SetText (data?.LocalTimeString (), TextView.BufferType.Normal);
		}

		Drawable defaultDrawable;

		public override void SetSelected (bool selected)
		{
			if (defaultDrawable == null)
			{
				defaultDrawable = container.Background;
			}

			if (selected)
			{
				container.SetBackgroundResource (Resource.Color.theme_white_30);
			}
			else
			{
				container.Background = defaultDrawable;
			}

			//System.Diagnostics.Debug.WriteLine (selected ? "Selected" : "Deselected");
		}
	}
}