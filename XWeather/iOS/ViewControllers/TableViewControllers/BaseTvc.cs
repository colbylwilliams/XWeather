using System;
using Foundation;
using UIKit;
using XWeather.Clients;

namespace XWeather.iOS
{
	public class BaseTvc<TCell> : UITableViewController
		where TCell : BaseTvCell
	{

		public WuLocation Location => WuClient.Shared.Current;


		public BaseTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.Clear;

			TableView.BackgroundColor = UIColor.Clear;
		}


		public override void WillDisplayHeaderView (UITableView tableView, UIView headerView, nint section)
		{
			var header = headerView as UITableViewHeaderFooterView;

			if (header?.ContentView != null)
				header.ContentView.BackgroundColor = UIColor.Clear;

			if (header?.BackgroundView != null)
				header.BackgroundView.BackgroundColor = UIColor.Clear;

			if (header?.TextLabel != null)
				header.TextLabel.TextColor = UIColor.White;
		}


		public override UIStatusBarStyle PreferredStatusBarStyle () => UIStatusBarStyle.LightContent;


		public TCell DequeCell (UITableView tableView, NSIndexPath indexPath)
			=> tableView.DequeueReusableCell (typeof (TCell).Name, indexPath) as TCell;
	}
}