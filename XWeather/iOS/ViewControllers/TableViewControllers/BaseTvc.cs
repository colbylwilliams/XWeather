using System;
using System.Linq;

using Foundation;
using UIKit;

using XWeather.Clients;

namespace XWeather.iOS
{
	public class BaseTvc<TCell> : UITableViewController
		where TCell : BaseTvCell
	{

		internal nfloat FooterHeight = 44;


		public virtual nfloat HeaderHeight => 280;


		public WuLocation Location => WuClient.Shared.Selected;


		public BaseTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.Clear;

			TableView.BackgroundColor = UIColor.Clear;
		}


		[Export ("scrollViewDidScroll:")]
		public void Scrolled (UIScrollView scrollView)
		{
			MaskCells (scrollView);
		}


		public virtual void MaskCells (UIScrollView scrollView)
		{
			foreach (TCell cell in TableView.VisibleCells)
			{
				var topHiddenHeight = scrollView.ContentOffset.Y + HeaderHeight - cell.Frame.Y + scrollView.ContentInset.Top;
				var bottomHiddenHeight = cell.Frame.Bottom - (scrollView.ContentOffset.Y + scrollView.Frame.Height - FooterHeight);

				cell.SetTransparencyMask (topHiddenHeight, bottomHiddenHeight);
			}
		}


		public override nfloat GetHeightForHeader (UITableView tableView, nint section) => HeaderHeight;


		public override nfloat GetHeightForFooter (UITableView tableView, nint section) => FooterHeight;


		public override void WillDisplayHeaderView (UITableView tableView, UIView headerView, nint section)
		{
			if (TableView.VisibleCells.Any ()) MaskCells (TableView);

			var header = headerView as UITableViewHeaderFooterView;

			if (header?.ContentView != null)
				header.ContentView.BackgroundColor = UIColor.Clear;

			if (header?.BackgroundView != null)
				header.BackgroundView.BackgroundColor = UIColor.Clear;

			if (header?.TextLabel != null)
				header.TextLabel.TextColor = UIColor.White;
		}


		public override void WillDisplayFooterView (UITableView tableView, UIView footerView, nint section)
		{
			var footer = footerView as UITableViewHeaderFooterView;

			if (footer?.ContentView != null)
				footer.ContentView.BackgroundColor = UIColor.Clear;

			if (footer?.BackgroundView != null)
				footer.BackgroundView.BackgroundColor = UIColor.Clear;

			if (footer?.TextLabel != null)
				footer.TextLabel.TextColor = UIColor.White;
		}


		public TCell DequeCell (UITableView tableView, NSIndexPath indexPath)
			=> tableView.DequeueReusableCell (typeof (TCell).Name, indexPath) as TCell;


		public override UIStatusBarStyle PreferredStatusBarStyle () => UIStatusBarStyle.LightContent;
	}
}