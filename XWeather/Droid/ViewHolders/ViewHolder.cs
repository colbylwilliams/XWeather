using System;

using Android.Support.V7.Widget;
using Android.Views;

namespace XWeather.Droid
{
	public abstract class ViewHolder<TData> : RecyclerView.ViewHolder//, IViewHolder<TData>
	{
		protected ViewHolder (View itemView) : base (itemView)
		{
#pragma warning disable RECS0021
			FindViews (itemView); // Warns about calls to virtual member functions occuring in the constructor
#pragma warning restore RECS0021
		}


		public void SetClickHandler (Action<View, int> handler)
		{
			ItemView.Click += (sender, e) => handler ((View)sender, AdapterPosition);
		}


		public void SetLongClickHandler (Action<View, int> handler)
		{
			ItemView.LongClick += (sender, e) => handler ((View)sender, AdapterPosition);
		}


		public abstract void FindViews (View rootView);


		public abstract void SetData (TData data);
	}


	public abstract class SelectableViewHolder<TData> : ViewHolder<TData>
	{
		protected SelectableViewHolder (View itemView) : base (itemView) { }

		public virtual void SetData (TData data, bool selected)
		{
			SetData (data);
			SetSelected (selected);
		}

		public abstract void SetSelected (bool selected);
	}


	public static class ViewHolder
	{
		public static void SetData<TData> (this RecyclerView.ViewHolder holder, TData data)
		{
			var viewHolder = holder as ViewHolder<TData>;

			viewHolder?.SetData (data);
		}

		public static void SetData<TData> (this RecyclerView.ViewHolder holder, TData data, bool selected)
		{
			var viewHolder = holder as SelectableViewHolder<TData>;

			viewHolder?.SetData (data, selected);
		}
	}
}