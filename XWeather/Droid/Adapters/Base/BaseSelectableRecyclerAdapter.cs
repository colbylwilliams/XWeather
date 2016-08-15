using System.Linq;

using System.Collections.Generic;
using Android.Support.V7.Widget;

namespace XWeather.Droid
{
	public abstract class BaseSelectableRecyclerAdapter<TCellData, TCellViewHolder> : BaseRecyclerAdapter<TCellData, TCellViewHolder>
		where TCellViewHolder : SelectableViewHolder<TCellData>
	{
		public List<TCellData> SelectedDataSet = new List<TCellData> ();


		protected BaseSelectableRecyclerAdapter (int cellResource) : base (cellResource) { }


		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			var data = DataSet [position];

			if (holder is TCellViewHolder) {
				holder.SetData (data, SelectedDataSet.Contains (data));
			}
		}


		public virtual void DeselectAllItems ()
		{
			SelectedDataSet = new List<TCellData> ();

			NotifyDataSetChanged ();
		}


		public virtual void SelectAllItems ()
		{
			SelectedDataSet = DataSet.ToList ();

			NotifyDataSetChanged ();
		}


		public virtual void RemoveSelectedItems ()
		{
			foreach (var item in SelectedDataSet) {

				if (DataSet.Contains (item)) {

					DataSet.Remove (item);
				}
			}

			NotifyDataSetChanged ();
		}


		public virtual void ToggleItemSelection (int position)
		{
			var item = DataSet [position];

			if (SelectedDataSet.Contains (item)) {

				SelectedDataSet.Remove (item);

			} else {

				SelectedDataSet.Add (item);
			}

			NotifyItemChanged (position);
		}
	}
}