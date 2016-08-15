using System;
using System.Collections.Generic;

using Android.Views;

using Android.Support.V7.Widget;


namespace XWeather.Droid
{
	public abstract class BaseRecyclerAdapter<TCellData, TCellViewHolder> : RecyclerView.Adapter
		where TCellViewHolder : ViewHolder<TCellData>
	{
		readonly int cellResource;

		public const string TAG = "RecyclerViewAdapter";

		public event EventHandler<int> ItemClick;
		public event EventHandler<int> ItemLongClick;


		public virtual IList<TCellData> DataSet => new List<TCellData> ();


		protected BaseRecyclerAdapter (int cellResource)
		{
			this.cellResource = cellResource;
		}


		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View rootView = LayoutInflater.From (parent.Context).Inflate (cellResource, parent, false);

			var holder = CreateCellViewHolder (rootView);

			holder.SetClickHandler (OnClick);

			holder.SetLongClickHandler (OnLongClick);

			return holder;
		}


		protected abstract TCellViewHolder CreateCellViewHolder (View rootView);


		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			if (holder is TCellViewHolder) {
				holder.SetData (DataSet [position]);
			}
		}


		public override int ItemCount => DataSet?.Count ?? 0;


		protected virtual void OnClick (View view, int position) => ItemClick?.Invoke (view, position);


		protected virtual void OnLongClick (View view, int position) => ItemLongClick?.Invoke (view, position);


		public virtual TCellData RemoveItem (int position)
		{
			var item = DataSet [position];
			DataSet.RemoveAt (position);
			NotifyItemRemoved (position);
			return item;
		}


		public virtual void AddItem (int position, TCellData item)
		{
			DataSet.Insert (position, item);
			NotifyItemInserted (position);
		}


		public virtual void MoveItem (int fromPosition, int toPosition)
		{
			var item = DataSet [fromPosition];
			DataSet.RemoveAt (fromPosition);
			DataSet.Insert (toPosition, item);
			NotifyItemMoved (fromPosition, toPosition);
		}


		protected void ApplyAndAnimateRemovals (IList<TCellData> badItems)
		{
			for (int i = DataSet.Count - 1; i >= 0; i--) {

				var item = DataSet [i];

				if (badItems.Contains (item)) {
					RemoveItem (i);
				}
			}
		}


		protected void ApplyAndAnimateAdditions (IList<TCellData> newItems)
		{
			for (int i = 0; i < newItems.Count; i++) {

				var item = newItems [i];

				if (!DataSet.Contains (item)) {
					AddItem (i, item);
				}
			}
		}


		protected void ApplyAndAnimateMovedItems (IList<TCellData> newItems)
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