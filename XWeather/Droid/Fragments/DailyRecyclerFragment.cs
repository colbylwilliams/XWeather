using XWeather.Domain;

namespace XWeather.Droid
{
	public class DailyRecyclerFragment : RecyclerViewFragment<ForecastDay, DailyViewHolder, WuLocation, DailyHeadHolder>
	{
		public static DailyRecyclerFragment Create () => new DailyRecyclerFragment ();

		protected override BaseHeadRecyclerAdapter<ForecastDay, DailyViewHolder, WuLocation, DailyHeadHolder> GetAdapter ()
			=> new DailyRecyclerAdapter (Resource.Layout.DailyListItem, Resource.Layout.DailyHeader);

	}
}