using Android.OS;
using Android.Preferences;
using Android.Views;

using SettingsStudio;

namespace XWeather.Droid
{
	public class SettingsFragment : PreferenceFragment
	{

		static readonly string [] UomPreferences = { SettingsKeys.UomTemperature, SettingsKeys.UomDistance, SettingsKeys.UomPressure, SettingsKeys.UomLength, SettingsKeys.UomSpeed };


		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetHasOptionsMenu (true);

			AddPreferencesFromResource (Resource.Xml.preferences);

			FindPreference ("VersionNumberString").Summary = $"{Settings.VersionNumber} ({Settings.BuildNumber})";
		}


		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);

			menu.RemoveItem (Resource.Id.action_search);
			menu.RemoveItem (Resource.Id.action_radar);
			menu.RemoveItem (Resource.Id.action_settings);
		}


		public override void OnResume ()
		{
			base.OnResume ();

			Analytics.TrackPageViewStart (this, Pages.Settings);

			foreach (var item in UomPreferences)
			{
				var preference = (ListPreference)FindPreference (item);

				preference.PreferenceChange += handlePreferenceChange;

				preference.Summary = preference.Entry ?? " ";
			}
		}


		public override void OnPause ()
		{
			Analytics.TrackPageViewEnd (this);

			base.OnPause ();

			foreach (var item in UomPreferences)
			{
				var preference = (ListPreference)FindPreference (item);

				preference.PreferenceChange -= handlePreferenceChange;
			}
		}


		void handlePreferenceChange (object sender, Preference.PreferenceChangeEventArgs e)
		{
			var listPreference = e.Preference as ListPreference;

			int val = 0;

			if (listPreference != null && int.TryParse (e.NewValue.ToString (), out val))
			{
				listPreference.Summary = listPreference.GetEntries () [val];
			}
			else
			{
				e.Preference.Summary = e.NewValue.ToString ();
			}
		}
	}
}