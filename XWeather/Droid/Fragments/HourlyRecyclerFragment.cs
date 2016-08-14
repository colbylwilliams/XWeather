using XWeather.Domain;

namespace XWeather.Droid
{
	public class HourlyRecyclerFragment : RecyclerViewSupportFragment<HourlyForecast, HourlyViewHolder, WuLocation, HourlyHeadHolder>
	{
		public static HourlyRecyclerFragment Create () => new HourlyRecyclerFragment ();

		protected override BaseRecyclerAdapter<HourlyForecast, HourlyViewHolder, WuLocation, HourlyHeadHolder> GetAdapter ()
			=> new HourlyRecyclerAdapter (Resource.Layout.HourlyListItem, Resource.Layout.HourlyHeader);
	}
}