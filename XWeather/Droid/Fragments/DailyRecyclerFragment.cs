using XWeather.Domain;

namespace XWeather.Droid
{
	public class DailyRecyclerFragment : RecyclerViewSupportFragment<ForecastDay, DailyViewHolder, WuLocation, DailyHeadHolder>
	{
		public static DailyRecyclerFragment Create () => new DailyRecyclerFragment ();

		protected override BaseRecyclerAdapter<ForecastDay, DailyViewHolder, WuLocation, DailyHeadHolder> GetAdapter ()
			=> new DailyRecyclerAdapter (Resource.Layout.DailyListItem, Resource.Layout.DailyHeader);

	}
}