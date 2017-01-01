using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using Android.Support.V7.Widget;

using XWeather.Clients;


namespace XWeather.Droid
{
	public class LocationsRecyclerFragment : Fragment, ActionMode.ICallback
	{
		public const string TAG = "LocationsRecyclerFragment";


		public bool Editing { get; set; }


		public RecyclerView RecyclerView { get; set; }

		public LocationsRecyclerAdapter Adapter { get; set; }


		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var rootView = inflater.Inflate (Resource.Layout.RecyclerViewFragment, container, false);

			rootView.SetTag (rootView.Id, TAG);


			RecyclerView = rootView.FindViewById<RecyclerView> (Resource.Id.recyclerView);

			RecyclerView.SetLayoutManager (new LinearLayoutManager (Activity));

			RecyclerView.AddItemDecoration (new DividerItemDecoration (Activity, DividerItemDecoration.VerticalList));

			RecyclerView.ScrollToPosition (0);


			Adapter = new LocationsRecyclerAdapter (Resource.Layout.LocationListItem);

			RecyclerView.SetAdapter (Adapter);

			return rootView;
		}


		public override void OnResume ()
		{
			base.OnResume ();

			Analytics.TrackPageViewStart (this, Pages.LocationList);
		}


		public override void OnPause ()
		{
			Analytics.TrackPageViewEnd (this);

			base.OnPause ();
		}


		public override void OnStop ()
		{
			detachEvents ();

			base.OnStop ();
		}


		public override void OnDestroy ()
		{
			detachEvents ();

			base.OnDestroy ();
		}


		public override void OnStart ()
		{
			base.OnStart ();

			WuClient.Shared.UpdatedSelected += handleUpdatedCurrent;

			Adapter.ItemClick += handleItemClick;
			Adapter.ItemLongClick += handleItemLongClick;
		}


		void detachEvents ()
		{
			WuClient.Shared.UpdatedSelected -= handleUpdatedCurrent;

			if (Adapter != null)
			{
				Adapter.ItemClick -= handleItemClick;
				Adapter.ItemLongClick -= handleItemLongClick;
			}
		}


		void handleItemClick (object sender, int position)
		{
			if (Editing)
			{

				Adapter.ToggleItemSelection (position);

			}
			else
			{

				// set location as the selected location
				WuClient.Shared.Selected = WuClient.Shared.Locations [position];
			}
		}


		void handleItemLongClick (object sender, int position)
		{
			if (!Editing)
			{

				Editing = true;

				Activity.StartActionMode (this);

				Adapter.ToggleItemSelection (position);
			}
		}


		void handleUpdatedCurrent (object sender, EventArgs e) => Activity.RunOnUiThread (Adapter.NotifyDataSetChanged);



		#region ActionMode.ICallback

		// Called when the user selects a contextual menu item
		public bool OnActionItemClicked (ActionMode mode, IMenuItem item)
		{
			switch (item.ItemId)
			{

				case Resource.Id.context_action_share:
					Toast.MakeText (Activity, "Sharing coming soon...", ToastLength.Short).Show ();
					return false;
				case Resource.Id.context_action_delete:
					Adapter.RemoveSelectedItems ();
					mode.Finish ();
					return true;
				case Resource.Id.context_action_select_all:
					Adapter.SelectAllItems ();
					return true;
				default:
					return false;
			}
		}


		// Called when the action mode is created; StartActionMode() was called
		public bool OnCreateActionMode (ActionMode mode, IMenu menu)
		{
			Activity.MenuInflater.Inflate (Resource.Menu.menu_locations_context, menu);
			return true;
		}


		// Called when the user exits the action mode
		public void OnDestroyActionMode (ActionMode mode)
		{
			Adapter.DeselectAllItems ();
			Editing = false;
		}


		// Called each time the action mode is shown. Always called after onCreateActionMode, but
		// may be called multiple times if the mode is invalidated.
		public bool OnPrepareActionMode (ActionMode mode, IMenu menu)
		{
			return false; // false if nothing is done
		}

		#endregion
	}
}