using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class DailyTvc : BaseTvc<DailyTvCell>
	{

		List<ForecastDay> Forecasts => Location?.Forecasts;


		public DailyTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			Analytics.TrackPageViewStart (this, Pages.WeatherDaily);
		}


		public override void ViewDidDisappear (bool animated)
		{
			Analytics.TrackPageViewEnd (this, Location);

			base.ViewDidDisappear (animated);
		}


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