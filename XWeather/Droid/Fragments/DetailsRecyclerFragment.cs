using XWeather.Domain;

namespace XWeather.Droid
{
	public class DetailsRecyclerFragment : RecyclerViewFragment<WeatherDetail, DetailsViewHolder, WuLocation, DetailsHeadHolder>
	{
		public static DetailsRecyclerFragment Create () => new DetailsRecyclerFragment ();

		protected override BaseHeadRecyclerAdapter<WeatherDetail, DetailsViewHolder, WuLocation, DetailsHeadHolder> GetAdapter ()
			=> new DetailsRecyclerAdapter (Resource.Layout.DetailsListItem, Resource.Layout.DetailsHeader);

	}
}