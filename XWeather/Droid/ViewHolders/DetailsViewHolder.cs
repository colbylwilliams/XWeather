using Android.Views;
using Android.Widget;

using XWeather.Domain;

namespace XWeather.Droid
{
	public class DetailsViewHolder : ViewHolder<WeatherDetail>
	{
		TextView itemLabel;
		TextView valueLabel;

		public DetailsViewHolder (View itemView) : base (itemView) { }


		public static DetailsViewHolder Create (View rootView) => new DetailsViewHolder (rootView);


		public override void FindViews (View rootView)
		{
			itemLabel = (TextView)rootView.FindViewById (Resource.Id.DetailsListItem_itemLabel);
			valueLabel = (TextView)rootView.FindViewById (Resource.Id.DetailsListItem_valueLabel);
		}


		public override void SetData (WeatherDetail data)
		{
			itemLabel.SetText (data.DetailLabel.AppendColon (), TextView.BufferType.Normal);
			valueLabel.SetText (data.DetailValue, TextView.BufferType.Normal);
		}
	}
}