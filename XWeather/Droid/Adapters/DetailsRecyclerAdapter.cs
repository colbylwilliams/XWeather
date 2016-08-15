using System.Collections.Generic;

using Android.Views;

using SettingsStudio;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Droid
{
	public class DetailsRecyclerAdapter : BaseHeadRecyclerAdapter<WeatherDetail, DetailsViewHolder, WuLocation, DetailsHeadHolder>
	{
		public override IList<WeatherDetail> DataSet => WeatherDetails.GetDetails (WuClient.Shared.Selected, Settings.UomTemperature, Settings.UomSpeed, Settings.UomLength, Settings.UomDistance, Settings.UomPressure);

		public override WuLocation HeadData => WuClient.Shared.Selected;


		public DetailsRecyclerAdapter (int cellResource, int headResource) : base (cellResource, headResource) { }


		protected override DetailsViewHolder CreateCellViewHolder (View rootView) => DetailsViewHolder.Create (rootView);

		protected override DetailsHeadHolder CreateHeadViewHolder (View rootView) => DetailsHeadHolder.Create (rootView);
	}
}