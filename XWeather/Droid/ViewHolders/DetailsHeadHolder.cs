using Android.Views;
using Android.Widget;

using SettingsStudio;

namespace XWeather.Droid
{
	public class DetailsHeadHolder : ViewHolder<WuLocation>
	{
		TextView locationLabel;
		TextView conditionLabel;


		public DetailsHeadHolder (View itemView) : base (itemView) { }


		public static DetailsHeadHolder Create (View rootView) => new DetailsHeadHolder (rootView);


		public override void FindViews (View rootView)
		{
			locationLabel = (TextView)rootView.FindViewById (Resource.Id.DetailsHeader_locationLabel);
			conditionLabel = (TextView)rootView.FindViewById (Resource.Id.DetailsHeader_conditionLabel);
		}


		public override void SetData (WuLocation data)
		{
			locationLabel.SetText (data?.Name, TextView.BufferType.Normal);
			conditionLabel.SetText (data?.ForecastString (Settings.UomTemperature), TextView.BufferType.Normal);
		}
	}
}