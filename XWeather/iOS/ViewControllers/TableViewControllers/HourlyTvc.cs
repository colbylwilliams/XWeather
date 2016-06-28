using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class HourlyTvc : BaseTvc<HourlyTvCell>
	{

		List<HourlyForecast> Forecasts => Location?.Weather?.hourly_forecast ?? new List<HourlyForecast> ();


		public HourlyTvc (IntPtr handle) : base (handle) { }


		public override nint NumberOfSections (UITableView tableView) => 1;


		public override nint RowsInSection (UITableView tableView, nint section) => Forecasts.Count;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = DequeCell (tableView, indexPath);

			var hourly = Forecasts [indexPath.Row];

			cell.TextLabel.Text = hourly.FCTTIME.civil;
			cell.DetailTextLabel.Text = $"{hourly.temp.english}°";

			return cell;
		}


		public override string TitleForHeader (UITableView tableView, nint section) => Location?.Location?.name;


		public override nfloat GetHeightForHeader (UITableView tableView, nint section) => 200;


		public override nfloat GetHeightForFooter (UITableView tableView, nint section) => 44;
	}
}