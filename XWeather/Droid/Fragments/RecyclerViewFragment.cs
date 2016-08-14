using System;

using Android.OS;
using Android.Views;

//using Android.Support.V4.App;
using Android.Support.V7.Widget;

using XWeather.Clients;
using Android.App;

namespace XWeather.Droid
{
	public interface IRecyclerViewFragment
	{
		RecyclerView RecyclerView { get; set; }
		RecyclerView.Adapter Adapter { get; set; }
	}


	public abstract class RecyclerViewFragment<TCellData, TCellViewHolder, THeadData, THeadViewHolder> : Fragment, IRecyclerViewFragment
		where TCellViewHolder : ViewHolder<TCellData>
		where THeadViewHolder : ViewHolder<THeadData>
	{
		public const string TAG = "RecycleViewFragment";

		public bool ShowDividers { get; set; } = true;

		public RecyclerView RecyclerView { get; set; }

		public RecyclerView.Adapter Adapter { get; set; }

		public RecyclerView.LayoutManager LayoutManager;

		protected abstract BaseRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder> GetAdapter ();


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var rootView = inflater.Inflate (Resource.Layout.RecyclerViewFragment, container, false);
			rootView.SetTag (rootView.Id, TAG);
			RecyclerView = rootView.FindViewById<RecyclerView> (Resource.Id.recyclerView);

			// A LinearLayoutManager is used here, this will layout the elements in a similar fashion
			// to the way ListView would layout elements. The RecyclerView.LayoutManager defines how the
			// elements are laid out.
			LayoutManager = new LinearLayoutManager (Activity);
			RecyclerView.SetLayoutManager (LayoutManager);

			if (ShowDividers) {
				//RecyclerView.AddItemDecoration (new DividerItemDecoration (Activity, DividerItemDecoration.VerticalList));
			}

			Adapter = GetAdapter ();
			RecyclerView.ScrollToPosition (0);
			RecyclerView.SetAdapter (Adapter);

			return rootView;
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
			((BaseRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemClick += handleItemClick;
		}


		void detachEvents ()
		{
			WuClient.Shared.UpdatedSelected -= handleUpdatedCurrent;

			if (Adapter != null)
				((BaseRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemClick -= handleItemClick;
		}


		void handleItemClick (object sender, int position) => OnItemClick ((View)sender, position);


		void handleUpdatedCurrent (object sender, EventArgs e) => Activity.RunOnUiThread (Adapter.NotifyDataSetChanged);


		protected virtual void OnItemClick (View view, int position) { }
	}


	public abstract class RecyclerViewSupportFragment<TCellData, TCellViewHolder, THeadData, THeadViewHolder> : Android.Support.V4.App.Fragment, IRecyclerViewFragment
		where TCellViewHolder : ViewHolder<TCellData>
		where THeadViewHolder : ViewHolder<THeadData>
	{
		public const string TAG = "RecycleViewFragment";

		public bool ShowDividers { get; set; } = true;

		public RecyclerView RecyclerView { get; set; }

		public RecyclerView.Adapter Adapter { get; set; }

		public RecyclerView.LayoutManager LayoutManager;

		protected abstract BaseRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder> GetAdapter ();


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var rootView = inflater.Inflate (Resource.Layout.RecyclerViewFragment, container, false);
			rootView.SetTag (rootView.Id, TAG);
			RecyclerView = rootView.FindViewById<RecyclerView> (Resource.Id.recyclerView);

			// A LinearLayoutManager is used here, this will layout the elements in a similar fashion
			// to the way ListView would layout elements. The RecyclerView.LayoutManager defines how the
			// elements are laid out.
			LayoutManager = new LinearLayoutManager (Activity);
			RecyclerView.SetLayoutManager (LayoutManager);

			if (ShowDividers) {
				//RecyclerView.AddItemDecoration (new DividerItemDecoration (Activity, DividerItemDecoration.VerticalList));
			}

			Adapter = GetAdapter ();
			RecyclerView.ScrollToPosition (0);
			RecyclerView.SetAdapter (Adapter);

			return rootView;
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
			((BaseRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemClick += handleItemClick;
		}


		void detachEvents ()
		{
			WuClient.Shared.UpdatedSelected -= handleUpdatedCurrent;

			if (Adapter != null)
				((BaseRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemClick -= handleItemClick;
		}


		void handleItemClick (object sender, int position) => OnItemClick ((View)sender, position);


		void handleUpdatedCurrent (object sender, EventArgs e) => Activity.RunOnUiThread (Adapter.NotifyDataSetChanged);


		protected virtual void OnItemClick (View view, int position) { }
	}

}