using System;

using Android.OS;
using Android.Views;

using Android.Support.V7.Widget;

using XWeather.Clients;


namespace XWeather.Droid
{
	public interface IRecyclerViewFragment
	{
		RecyclerView RecyclerView { get; set; }
		RecyclerView.Adapter Adapter { get; set; }
	}


	public abstract class RecyclerViewFragment<TCellData, TCellViewHolder, THeadData, THeadViewHolder> : Android.Support.V4.App.Fragment, IRecyclerViewFragment
		where TCellViewHolder : ViewHolder<TCellData>
		where THeadViewHolder : ViewHolder<THeadData>
	{
		public const string TAG = "RecycleViewFragment";

		public bool ShowDividers { get; set; } = true;

		public RecyclerView RecyclerView { get; set; }

		public RecyclerView.Adapter Adapter { get; set; }


		protected abstract BaseHeadRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder> GetAdapter ();


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var rootView = inflater.Inflate (Resource.Layout.RecyclerViewFragment, container, false);
			rootView.SetTag (rootView.Id, TAG);

			RecyclerView = rootView.FindViewById<RecyclerView> (Resource.Id.recyclerView);

			RecyclerView.SetLayoutManager (new LinearLayoutManager (Activity));

			if (ShowDividers)
			{
				RecyclerView.AddItemDecoration (new DividerItemDecoration (Activity, DividerItemDecoration.VerticalList));
			}

			RecyclerView.ScrollToPosition (0);

			Adapter = GetAdapter ();

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

			((BaseHeadRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemClick += handleItemClick;
			((BaseHeadRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemLongClick += handleItemLongClick;
		}


		void detachEvents ()
		{
			WuClient.Shared.UpdatedSelected -= handleUpdatedCurrent;

			if (Adapter != null)
			{
				((BaseHeadRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemClick -= handleItemClick;
				((BaseHeadRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder>)Adapter).ItemLongClick -= handleItemLongClick;
			}
		}


		void handleItemClick (object sender, int position) => OnItemClick ((View)sender, position);

		void handleItemLongClick (object sender, int position) => OnItemLongClick ((View)sender, position);


		void handleUpdatedCurrent (object sender, EventArgs e) => Activity.RunOnUiThread (Adapter.NotifyDataSetChanged);


		protected virtual void OnItemClick (View view, int position) { }

		protected virtual void OnItemLongClick (View view, int position) { }
	}
}