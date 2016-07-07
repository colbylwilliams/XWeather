using System.Collections.Generic;

using Android.Views;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Droid
{
	public class HourlyRecyclerAdapter : BaseRecyclerAdapter<HourlyForecast, HourlyViewHolder, WuLocation, HourlyHeadHolder>
	{
		public override IList<HourlyForecast> DataSet => WuClient.Shared.Selected?.HourlyForecasts;

		public override WuLocation HeadData => WuClient.Shared.Selected;


		public HourlyRecyclerAdapter (int cellResource, int headResource) : base (cellResource, headResource) { }


		protected override HourlyViewHolder CreateCellViewHolder (View rootView) => HourlyViewHolder.Create (rootView);

		protected override HourlyHeadHolder CreateHeadViewHolder (View rootView) => HourlyHeadHolder.Create (rootView);
	}
}