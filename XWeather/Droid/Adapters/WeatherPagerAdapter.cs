using Android.Support.V4.App;

namespace XWeather.Droid
{
	public class WeatherPagerAdapter : BasePagerAdapter
	{

		public WeatherPagerAdapter (FragmentManager manager) : base (manager) { }


		public override int Count => 3;


		public override Fragment GetItem (int position)
		{
			switch (position)
			{
				case 0: return DailyRecyclerFragment.Create ();
				case 1: return HourlyRecyclerFragment.Create ();
				case 2: return DetailsRecyclerFragment.Create ();
				default: return null;
			}
		}
	}
}