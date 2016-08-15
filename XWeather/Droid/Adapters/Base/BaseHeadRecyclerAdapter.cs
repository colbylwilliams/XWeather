using System;

using Android.Views;

using Android.Support.V7.Widget;


namespace XWeather.Droid
{
	public abstract class BaseHeadRecyclerAdapter<TCellData, TCellViewHolder, THeadData, THeadViewHolder> : BaseRecyclerAdapter<TCellData, TCellViewHolder>
		where TCellViewHolder : ViewHolder<TCellData>
		where THeadViewHolder : ViewHolder<THeadData>
	{
		const int HeaderView = 0;
		const int CellView = 1;

		public event EventHandler<int> HeadClick;


		public virtual THeadData HeadData => default (THeadData);

		readonly int headResource;


		protected BaseHeadRecyclerAdapter (int cellResource, int headResource) : base (cellResource)
		{
			this.headResource = headResource;
		}


		// Create new views (invoked by the layout manager)
		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			if (viewType == CellView) {

				return base.OnCreateViewHolder (parent, viewType);

			}

			if (viewType == HeaderView) {

				View rootView = LayoutInflater.From (parent.Context).Inflate (headResource, parent, false);

				var holder = CreateHeadViewHolder (rootView);

				holder.SetClickHandler (OnClick);
				holder.SetLongClickHandler (OnLongClick);

				return holder;
			}

			throw new NotSupportedException ();
		}


		protected abstract THeadViewHolder CreateHeadViewHolder (View rootView);


		// Replace the contents of a view (invoked by the layout manager)
		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			// Get element from your dataset at this position and replace the contents of the view with that element
			if (holder is TCellViewHolder) {
				position -= 1;
				base.OnBindViewHolder (holder, position);
			} else if (holder is THeadViewHolder) {
				holder.SetData (HeadData);
			}
		}


		// Return the size of your dataset (invoked by the layout manager)
		public override int ItemCount => (DataSet?.Count ?? 0) + 1;


		public override int GetItemViewType (int position) => position == 0 ? HeaderView : CellView;


		protected override void OnClick (View view, int position)
		{
			if (position == 0) {
				HeadClick?.Invoke (view, position);
			} else {
				position -= 1;
				base.OnClick (view, position);
			}
		}


		protected override void OnLongClick (View view, int position)
		{
			if (position == 0) {
				//HeadClick?.Invoke (view, position);
			} else {
				position -= 1;
				base.OnLongClick (view, position);
			}
		}


		public override TCellData RemoveItem (int position)
		{
			position -= 1;

			return base.RemoveItem (position);
		}


		public override void AddItem (int position, TCellData item)
		{
			position -= 1;

			base.AddItem (position, item);
		}


		public override void MoveItem (int fromPosition, int toPosition)
		{
			fromPosition -= 1;
			toPosition -= 1;

			base.MoveItem (fromPosition, toPosition);
		}
	}
}