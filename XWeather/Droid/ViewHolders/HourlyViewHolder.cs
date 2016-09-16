using System;

using Android.Views;
using Android.Widget;

using SettingsStudio;

using XWeather.Domain;


namespace XWeather.Droid
{
	public class HourlyViewHolder : ViewHolder<HourlyForecast>
	{
		TextView hourLabel;
		TextView periodLabel;
		TextView tempLabel;
		TextView precipLabel;
		ImageView iconImageView;

		TextView tomorrowLabel;


		static readonly int day = DateTime.Now.Day;


		public HourlyViewHolder (View itemView) : base (itemView) { }


		public static HourlyViewHolder Create (View rootView) => new HourlyViewHolder (rootView);


		public override void FindViews (View rootView)
		{
			hourLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyListItem_hourLabel);
			periodLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyListItem_periodLabel);

			tempLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyListItem_tempLabel);
			precipLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyListItem_precipLabel);
			iconImageView = (ImageView)rootView.FindViewById (Resource.Id.HourlyListItem_iconImageView);

			tomorrowLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyListItem_tomorrowLabel);
		}


		public override void SetData (HourlyForecast data)
		{
			hourLabel.SetText (data.HourString (), TextView.BufferType.Normal);
			periodLabel.SetText (data.PeriodString (), TextView.BufferType.Normal);

			tempLabel.SetText (data.TempString (Settings.UomTemperature, true, true), TextView.BufferType.Normal);
			precipLabel.SetText (data.ProbabilityPercipString (), TextView.BufferType.Normal);
			iconImageView.SetImageResource (ImageProvider.ResourceForString (data.icon));

			tomorrowLabel.Visibility = data.FCTTIME.mday == day ? ViewStates.Invisible : ViewStates.Visible;
		}
	}
}