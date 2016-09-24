using System.Collections.Generic;
using System.Threading.Tasks;

using Java.Lang;

using Android.App;
using Android.Text;
using Android.Widget;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Droid
{
	public class LocationSearchFilter : Filter
	{

		public List<WuAcLocation> LocationResults = new List<WuAcLocation> ();

		public List<SpannableString> ResultStrings = new List<SpannableString> ();

		bool publish;

		FilterResults filterResults;

		readonly Activity Activity;
		readonly LocationSearchRecyclerAdapter Adapter;


		public LocationSearchFilter (Activity activity, LocationSearchRecyclerAdapter adapter)
		{
			Activity = activity;
			Adapter = adapter;
		}


		protected override FilterResults PerformFiltering (ICharSequence constraint)
		{
			if (filterResults == null) {
				filterResults = new FilterResults ();
			}

			Task.Run (async () => await SearchWithStringAsync (constraint));

			return filterResults;
		}


		async Task SearchWithStringAsync (ICharSequence constraint)
		{
			var searchString = constraint?.ToString ();

			if (searchString == null) {
				LocationResults = new List<WuAcLocation> ();
				ResultStrings = new List<SpannableString> ();
				return;
			}

			bool canceled = false;

			try {

				ResultStrings = new List<SpannableString> ();

				if (!string.IsNullOrWhiteSpace (searchString)) {

					LocationResults = await WuAcClient.GetAsync (searchString);

					Java.Lang.Object [] matchObjects = new Java.Lang.Object [LocationResults.Count];

					for (int i = 0; i < LocationResults.Count; i++) {

						var name = LocationResults [i].name;

						ResultStrings.Add (name.GetSearchResultSpannableString (searchString));

						matchObjects [i] = new Java.Lang.String (name);
					}

					filterResults.Values = matchObjects;
					filterResults.Count = LocationResults.Count;

				} else {

					LocationResults = new List<WuAcLocation> ();
				}

			} catch (System.Exception ex) {

				System.Diagnostics.Debug.WriteLine (ex.Message);

				canceled = true;

			} finally {

				if (!canceled) {
					publish = true;
					Activity.RunOnUiThread (() => PublishResults (constraint, filterResults));
				}
			}
		}


		protected override void PublishResults (ICharSequence constraint, FilterResults results)
		{
			if (publish) {
				if (results != null && results.Count > 0) {
					Adapter.NotifyDataSetChanged ();
				} else {
					Adapter.NotifyDataSetInvalidated ();
				}
			}

			publish = false;
		}
	}
}
