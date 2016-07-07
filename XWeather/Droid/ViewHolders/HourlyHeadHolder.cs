using System;

using Android.Views;
using Android.Widget;

using SettingsStudio;

namespace XWeather.Droid
{
	public class HourlyHeadHolder : ViewHolder<WuLocation>
	{
		TextView conditionLabel;
		TextView dayLabel;
		TextView highTempLabel;
		TextView locationLabel;
		TextView lowTempLabel;
		TextView precipLabel;
		TextView tempLabel;
		TextView todayLabel;

		public HourlyHeadHolder (View itemView) : base (itemView) { }


		public static HourlyHeadHolder Create (View rootView) => new HourlyHeadHolder (rootView);


		public override void FindViews (View rootView)
		{
			conditionLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_conditionLabel);
			dayLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_dayLabel);
			highTempLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_highTempLabel);
			locationLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_locationLabel);
			lowTempLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_lowTempLabel);
			precipLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_precipLabel);
			tempLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_tempLabel);
			todayLabel = (TextView)rootView.FindViewById (Resource.Id.HourlyHeader_todayLabel);
		}


		public override void SetData (WuLocation data)
		{
			conditionLabel.SetText (data?.Conditions?.weather, TextView.BufferType.Normal);
			dayLabel.SetText (DateTime.Today.DayOfWeek.ToString (), TextView.BufferType.Normal);
			highTempLabel.SetText (data?.HighTempString (Settings.UomTemperature), TextView.BufferType.Normal);
			locationLabel.SetText (data?.Name, TextView.BufferType.Normal);
			lowTempLabel.SetText (data?.LowTempString (Settings.UomTemperature), TextView.BufferType.Normal);
			precipLabel.SetText (data?.ProbabilityPercipString (), TextView.BufferType.Normal);
			tempLabel.SetText (data?.TempString (Settings.UomTemperature, true), TextView.BufferType.Normal);
			todayLabel.SetText ("Today", TextView.BufferType.Normal);
		}
	}
}