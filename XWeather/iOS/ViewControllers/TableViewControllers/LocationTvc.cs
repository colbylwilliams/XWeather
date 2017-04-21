using System;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using UIKit;

using XWeather.Clients;
using XWeather.Domain;
using XWeather.Unified;

using SettingsStudio;

namespace XWeather.iOS
{
	public partial class LocationTvc : BaseTvc<LocationTvCell>, IUISearchControllerDelegate
	{
		UISearchController searchController;

		LocationSearchTvc resultsController;

		List<WuLocation> Locations => WuClient.Shared.Locations;

		nfloat searchBarHeight => searchController?.SearchBar?.Frame.Height ?? 0;

		nfloat statusBarHeight = 20;

		nfloat rowHeight = 60;


		public LocationTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TableView.ContentInset = new UIEdgeInsets (statusBarHeight, 0, 0, 0);

			setupSearchController ();

			TableView.SetContentOffset (new CGPoint (0, searchBarHeight - statusBarHeight), false);

			if (!UIAccessibility.IsReduceTransparencyEnabled)
			{
				TableView.BackgroundColor = UIColor.Clear;

				var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Dark);

				TableView.BackgroundView = new UIVisualEffectView (blur);
			}
		}


		public override void MaskCells (UIScrollView scrollView)
		{
			base.MaskCells (scrollView);

			if (TableView?.TableHeaderView != null)
			{
				var topHiddenHeight = scrollView.ContentOffset.Y - TableView.TableHeaderView.Frame.Y + scrollView.ContentInset.Top;

				TableView.TableHeaderView.SetTransparencyMask (topHiddenHeight, 0);
			}


			if (HeaderHeight > 0 && searchController != null && !searchController.Active && scrollView.ContentOffset.Y == -(statusBarHeight + 1))
			{
				searchController.Active = true;
			}
		}


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			WuClient.Shared.LocationAdded += handleLocationAdded;

			Analytics.TrackPageViewStart (this, Pages.LocationList);
		}


		public override void ViewDidDisappear (bool animated)
		{
			WuClient.Shared.LocationAdded -= handleLocationAdded;

			Analytics.TrackPageViewEnd (this);

			base.ViewDidDisappear (animated);
		}


		public override nfloat HeaderHeight {
			get {
				var headerHeight = TableView.Frame.Height - ((rowHeight * Locations.Count) + FooterHeight + searchBarHeight) + statusBarHeight;

				return headerHeight < 0 ? 0 : headerHeight;
			}
		}


		public override nint RowsInSection (UITableView tableView, nint section) => Locations?.Count ?? 0;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = DequeCell (tableView, indexPath);

			var location = Locations [indexPath.Row];

			cell.SetData (location);

			return cell;
		}


		public override UIView GetViewForHeader (UITableView tableView, nint section) => tableHeader;


		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			// set location as the selected location
			WuClient.Shared.Selected = Locations [indexPath.Row];

			//AnalyticsManager.Shared.TrackEvent (TrackedEvents.LocationList.Selected);

			DismissViewController (true, null);
		}


		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
			=> indexPath.Row > 0 ? UITableViewCellEditingStyle.Delete : UITableViewCellEditingStyle.None;


		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if (editingStyle == UITableViewCellEditingStyle.Delete)
			{
				var location = Locations [indexPath.Row];

				WuClient.Shared.RemoveLocation (location);

				tableView.DeleteRows (new [] { indexPath }, UITableViewRowAnimation.Automatic);

				Analytics.TrackEvent (Events.LocaitonDeleted);
			}
		}


		partial void addButtonClicked (NSObject sender)
		{
			if (searchController != null && !searchController.Active)
			{
				TableView.SetContentOffset (new CGPoint (0, -(statusBarHeight + 1)), true);
			}
		}


		void handleLocationAdded (object sender, EventArgs e)
		{
			BeginInvokeOnMainThread (() =>
			{
				if (searchController.Active)
				{
					searchController.Active = false;
				}
				else
				{
					Analytics.TrackEvent (Events.LocationAdded);

					TableView?.ReloadData ();

					if (HeaderHeight == 0)
					{
						MaskCells (TableView);
					}
				}
			});
		}


		void setupSearchController ()
		{
			resultsController = Storyboard.Instantiate<LocationSearchTvc> ();

			searchController = new UISearchController (resultsController)
			{
				DimsBackgroundDuringPresentation = false,
				SearchResultsUpdater = resultsController,
				WeakDelegate = this
			};

			searchController.SearchBar.BarStyle = UIBarStyle.Black;
			searchController.SearchBar.Translucent = true;
			searchController.SearchBar.TintColor = Colors.TintGray;
			searchController.SearchBar.KeyboardAppearance = UIKeyboardAppearance.Dark;


			searchController.SearchBar.WeakDelegate = this;

			TableView.TableHeaderView = searchController.SearchBar;

			DefinesPresentationContext = true;
		}


		#region IUISearchControllerDelegate


		[Export ("didDismissSearchController:")]
		public void DidDismissSearchController (UISearchController searchController)
		{
			MaskCells (TableView);
			TableView.SetContentOffset (new CGPoint (0, searchBarHeight - 20), true);
		}


		[Export ("didPresentSearchController:")]
		public void DidPresentSearchController (UISearchController searchController)
		{
			searchController.SearchBar.BecomeFirstResponder ();
		}


		[Export ("presentSearchController:")]
		public void PresentSearchController (UISearchController searchController) { }


		[Export ("willDismissSearchController:")]
		public void WillDismissSearchController (UISearchController searchController) { }


		[Export ("willPresentSearchController:")]
		public void WillPresentSearchController (UISearchController searchController) { }


		#endregion
	}
}