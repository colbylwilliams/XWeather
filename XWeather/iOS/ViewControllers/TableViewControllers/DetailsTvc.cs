using System;

using Foundation;
using UIKit;

using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class DetailsTvc : BaseTvc<DetailsTvCell>
	{

		CurrentObservation Conditions => Location?.Weather?.current_observation;


		public DetailsTvc (IntPtr handle) : base (handle) { }


		public override nint RowsInSection (UITableView tableView, nint section) => 0;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = DequeCell (tableView, indexPath);

			cell.SetData ("Label", "Value");

			return cell;
		}


		public override string TitleForHeader (UITableView tableView, nint section) => Location?.Location?.name;
	}
}