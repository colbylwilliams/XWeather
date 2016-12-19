using System;

using Android.App;
using Android.OS;
using Android.Views;

using Android.Support.V4.View;
using Android.Support.V7.Widget;

using Toolbar = Android.Support.V7.Widget.Toolbar;


namespace XWeather.Droid
{
	[Activity (Icon = "@mipmap/icon", Theme = "@style/LocationsTheme", LaunchMode = Android.Content.PM.LaunchMode.SingleTop,
			   ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class LocationsActivity : BaseActivity, SearchView.IOnQueryTextListener, MenuItemCompat.IOnActionExpandListener
	{
		IMenuItem searchMenuItem;

		LocationsRecyclerFragment LocationsFragment;

		LocationsSearchRecyclerFragment SearchFragment;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.LocationsActivity);


			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);

			SetSupportActionBar (toolbar);


			LocationsFragment = new LocationsRecyclerFragment ();

			FragmentManager.BeginTransaction ()
						   .Add (Resource.Id.locations_activity_content_container, LocationsFragment)
						   .Commit ();
		}


		protected override void HandleUpdatedSelectedLocation (object sender, EventArgs e)
		{
			Finish ();
		}


		protected override void HandleNewLocationAdded (object sender, EventArgs e)
		{
			RunOnUiThread (() =>
			{
				if (searchMenuItem.IsActionViewExpanded)
				{
					searchMenuItem.CollapseActionView ();
				}
				else
				{
					LocationsFragment.Adapter.NotifyDataSetChanged ();
				}
			});
		}


		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.menu_locations, menu);

			searchMenuItem = menu.FindItem (Resource.Id.action_search);

			// Associate searchable configuration with the SearchView
			var searchView = (SearchView)MenuItemCompat.GetActionView (searchMenuItem);

			var searchManager = (SearchManager)GetSystemService (SearchService);

			MenuItemCompat.SetOnActionExpandListener (searchMenuItem, this);

			searchView.SetSearchableInfo (searchManager.GetSearchableInfo (ComponentName));

			searchView.SetOnQueryTextListener (this);

			base.OnCreateOptionsMenu (menu);

			return true;
		}


		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			return base.OnPrepareOptionsMenu (menu);
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_search:

					return true;

				case Resource.Id.action_radar:

					Android.Widget.Toast.MakeText (this, "Weather radar coming soon...", Android.Widget.ToastLength.Short).Show ();

					return true;

				case Resource.Id.action_settings:

					FragmentManager.BeginTransaction ()
								   .Replace (Resource.Id.locations_activity_content_container, new SettingsFragment ())
								   .AddToBackStack (null)
								   .Commit ();

					return true;

				default: return base.OnOptionsItemSelected (item);
			}
		}


		public bool OnQueryTextChange (string newText)
		{
			//begins an async filtering operation
			SearchFragment.FilterAdapter.Filter.InvokeFilter (newText);

			return true;
		}


		public bool OnQueryTextSubmit (string query) => false;


		public bool OnMenuItemActionExpand (IMenuItem item)
		{
			if (SearchFragment == null) SearchFragment = new LocationsSearchRecyclerFragment (this);

			FragmentManager.BeginTransaction ()
			   .Replace (Resource.Id.locations_activity_content_container, SearchFragment)
			   .AddToBackStack (null)
			   .Commit ();

			return true;
		}


		public bool OnMenuItemActionCollapse (IMenuItem item)
		{
			FragmentManager.PopBackStack ();

			return true;
		}
	}
}