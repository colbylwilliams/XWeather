using Android.Views;
using Android.Widget;

using SettingsStudio;

using XWeather.Domain;

namespace XWeather.Droid
{
	public class DailyViewHolder : ViewHolder<ForecastDay>
	{
		TextView dayLabel;
		TextView precipLabel;
		TextView highTempLabel;
		TextView lowTempLabel;
		ImageView iconImageView;


		public DailyViewHolder (View itemView) : base (itemView) { }


		public static DailyViewHolder Create (View rootView) => new DailyViewHolder (rootView);


		public override void FindViews (View rootView)
		{
			dayLabel = (TextView)rootView.FindViewById (Resource.Id.DailyListItem_dayLabel);
			precipLabel = (TextView)rootView.FindViewById (Resource.Id.DailyListItem_precipLabel);
			highTempLabel = (TextView)rootView.FindViewById (Resource.Id.DailyListItem_highTempLabel);
			lowTempLabel = (TextView)rootView.FindViewById (Resource.Id.DailyListItem_lowTempLabel);
			iconImageView = (ImageView)rootView.FindViewById (Resource.Id.DailyListItem_iconImageView);
		}


		public override void SetData (ForecastDay data)
		{
			dayLabel.SetText (data.date.weekday, TextView.BufferType.Normal);
			precipLabel.SetText (data.ProbabilityPercipString (), TextView.BufferType.Normal);
			highTempLabel.SetText (data.HighTempString (Settings.UomTemperature, true, true), TextView.BufferType.Normal);
			lowTempLabel.SetText (data.LowTempString (Settings.UomTemperature, true, true), TextView.BufferType.Normal);
			iconImageView.SetImageResource (ImageProvider.ResourceForString (data.icon));
		}
	}
}