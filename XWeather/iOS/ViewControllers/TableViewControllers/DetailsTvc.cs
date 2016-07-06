using System;

using Foundation;
using UIKit;
using SettingsStudio;

namespace XWeather.iOS
{
	public partial class DetailsTvc : BaseTvc<DetailsTvCell>
	{

		public DetailsTvc (IntPtr handle) : base (handle) { }


		public override nint RowsInSection (UITableView tableView, nint section) => WeatherDetails.Count;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = DequeCell (tableView, indexPath);

			var label = WeatherDetails.GetLabel (indexPath.Row);
			var value = WeatherDetails.GetValue (indexPath.Row, Location, Settings.UomTemperature, Settings.UomSpeed, Settings.UomLength, Settings.UomDistance, Settings.UomPressure);

			cell.SetData (label, value);

			return cell;
		}


		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			tableHeader.SetData (Location);
			return tableHeader;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return WeatherDetails.IsSectionTop (indexPath.Row) ? 44 : 30;
		}
	}
}