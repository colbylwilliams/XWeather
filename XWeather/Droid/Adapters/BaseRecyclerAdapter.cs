using System;
using System.Collections.Generic;

using Android.Support.V7.Widget;
using Android.Views;
using Javax.Microedition.Khronos.Opengles;

namespace XWeather.Droid
{
	public abstract class BaseRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder> : RecyclerView.Adapter
		where TCellViewHolder : ViewHolder<TCellData>
		where THeadViewHolder : ViewHolder<THeadData>
	{
		const int HeaderView = 0;
		const int CellView = 1;

		public const string TAG = "RecyclerViewAdapter";


		public event EventHandler<int> HeadClick;
		public event EventHandler<int> ItemClick;


		public virtual IList<TCellData> DataSet => new List<TCellData> ();
		public virtual THeadData HeadData => default (THeadData);

		readonly int cellResource;
		readonly int headResource;


		protected BaseRecyclerAdapter (int cellResource, int headResource)
		{
			this.cellResource = cellResource;
			this.headResource = headResource;
		}


		// Create new views (invoked by the layout manager)
		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			if (viewType == CellView) {

				View rootView = LayoutInflater.From (parent.Context).Inflate (cellResource, parent, false);

				var holder = CreateCellViewHolder (rootView);

				holder.SetClickHandler (OnClick);

				return holder;
			}

			if (viewType == HeaderView) {

				View rootView = LayoutInflater.From (parent.Context).Inflate (headResource, parent, false);

				var holder = CreateHeadViewHolder (rootView);

				holder.SetClickHandler (OnClick);

				return holder;
			}

			throw new NotSupportedException ();
		}


		protected abstract TCellViewHolder CreateCellViewHolder (View rootView);

		protected abstract THeadViewHolder CreateHeadViewHolder (View rootView);


		// Replace the contents of a view (invoked by the layout manager)
		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			// Get element from your dataset at this position and replace the contents of the view with that element
			if (holder is TCellViewHolder) {
				position -= 1;
				holder.SetData (DataSet [position]);
			} else if (holder is THeadViewHolder) {
				holder.SetData (HeadData);
			}
		}


		// Return the size of your dataset (invoked by the layout manager)
		public override int ItemCount => (DataSet?.Count ?? 0) + 1;


		public override int GetItemViewType (int position) => position == 0 ? HeaderView : CellView;


		void OnClick (View view, int position)
		{
			if (position == 0) {
				HeadClick?.Invoke (view, position);
			} else {
				position -= 1;
				ItemClick?.Invoke (view, position);
			}
		}


		public TCellData RemoveItem (int position)
		{
			position -= 1;

			var item = DataSet [position];
			DataSet.RemoveAt (position);
			NotifyItemRemoved (position);
			return item;
		}


		public void AddItem (int position, TCellData item)
		{
			position -= 1;

			DataSet.Insert (position, item);
			NotifyItemInserted (position);
		}


		public void MoveItem (int fromPosition, int toPosition)
		{
			fromPosition -= 1;
			toPosition -= 1;

			var item = DataSet [fromPosition];
			DataSet.RemoveAt (fromPosition);
			DataSet.Insert (toPosition, item);
			NotifyItemMoved (fromPosition, toPosition);
		}


		void applyAndAnimateRemovals (IList<TCellData> newItems)
		{
			for (int i = DataSet.Count - 1; i >= 0; i--) {
				var item = DataSet [i];

				if (!newItems.Contains (item)) {
					RemoveItem (i);
				}
			}
		}


		void applyAndAnimateAdditions (IList<TCellData> newItems)
		{
			for (int i = 0; i < newItems.Count; i++) {
				var item = newItems [i];

				if (!DataSet.Contains (item)) {
					AddItem (i, item);
				}
			}
		}


		void applyAndAnimateMovedItems (IList<TCellData> newItems)
		{
			for (int toPosition = newItems.Count - 1; toPosition >= 0; toPosition--) {
				var item = newItems [toPosition];
				var fromPosition = DataSet.IndexOf (item);

				if (fromPosition >= 0 && fromPosition != toPosition) {
					MoveItem (fromPosition, toPosition);
				}
			}
		}
	}
}