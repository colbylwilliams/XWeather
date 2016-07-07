using Android.Views;
using Android.Widget;

namespace XWeather.Droid
{
	public class DailyHeadHolder : ViewHolder<WuLocation>
	{
		TextView locationLabel;


		public DailyHeadHolder (View itemView) : base (itemView) { }


		public static DailyHeadHolder Create (View rootView) => new DailyHeadHolder (rootView);


		public override void FindViews (View rootView)
		{
			locationLabel = (TextView)rootView.FindViewById (Resource.Id.DailyHeader_locationLabel);
		}


		public override void SetData (WuLocation data)
		{
			locationLabel.SetText (data?.Name, TextView.BufferType.Normal);
		}
	}
}