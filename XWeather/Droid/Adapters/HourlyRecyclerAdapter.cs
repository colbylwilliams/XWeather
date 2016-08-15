using System;
using System.Collections.Generic;

using Android.Views;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Droid
{
	public class HourlyRecyclerAdapter : BaseHeadRecyclerAdapter<HourlyForecast, HourlyViewHolder, WuLocation, HourlyHeadHolder>
	{
		static int day = DateTime.Now.Day;

		public override IList<HourlyForecast> DataSet => WuClient.Shared.Selected?.HourlyForecast (day);

		public override WuLocation HeadData => WuClient.Shared.Selected;


		public HourlyRecyclerAdapter (int cellResource, int headResource) : base (cellResource, headResource) { }


		protected override HourlyViewHolder CreateCellViewHolder (View rootView) => HourlyViewHolder.Create (rootView);

		protected override HourlyHeadHolder CreateHeadViewHolder (View rootView) => HourlyHeadHolder.Create (rootView);
	}
}