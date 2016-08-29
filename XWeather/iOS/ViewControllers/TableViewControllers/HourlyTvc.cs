using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using XWeather.Domain;
using XWeather.Unified;

namespace XWeather.iOS
{
	public partial class HourlyTvc : BaseTvc<HourlyTvCell>
	{
		static int day = DateTime.Now.Day;

		List<HourlyForecast> Forecasts => Location?.HourlyForecast (day);


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			AnalyticsManager.Shared.TrackEvent (TrackedEvents.WeatherHourly.Opened);
		}


		public HourlyTvc (IntPtr handle) : base (handle) { }


		public override nint RowsInSection (UITableView tableView, nint section) => Forecasts?.Count ?? 0;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = DequeCell (tableView, indexPath);

			var forecast = Forecasts [indexPath.Row];

			cell.SetData (forecast);

			return cell;
		}


		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			tableHeader.SetData (Location);

			return tableHeader;
		}
	}
}