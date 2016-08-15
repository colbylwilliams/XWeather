using Android.Views;

using Android.Support.V4.App;

namespace XWeather.Droid
{
	public abstract class BasePagerAdapter : FragmentPagerAdapter
	{
		readonly FragmentManager fragmentManager;

		string [] _tags;
		string [] Tags => _tags ?? (_tags = new string [Count]);


		protected BasePagerAdapter (FragmentManager manager) : base (manager)
		{
			fragmentManager = manager;
		}


		public override Java.Lang.Object InstantiateItem (ViewGroup container, int position)
		{
			var obj = base.InstantiateItem (container, position);

			if (obj is Fragment) {
				// record the fragment tag here
				var f = (Fragment)obj;
				var tag = f.Tag;
				Tags [position] = tag;
			}

			return obj;
		}


		public Fragment GetFragmentAtPosition (int position)
		{
			string tag = Tags [position];

			if (tag == null) return null;

			return fragmentManager.FindFragmentByTag (tag);
		}
	}
}