using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using SettingsStudio;

using XWeather.Clients;
using XWeather.Domain;
using XWeather.Unified;

namespace XWeather.iOS
{
	public partial class LocationSearchTvc : BaseTvc<LocationSearchTvCell>, IUISearchResultsUpdating
	{
		bool selectedLocation;

		public List<WuAcLocation> LocationResults = new List<WuAcLocation> ();

		public List<NSAttributedString> ResultStrings = new List<NSAttributedString> ();


		public LocationSearchTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (!UIAccessibility.IsReduceTransparencyEnabled)
			{
				TableView.BackgroundColor = UIColor.Clear;

				var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Dark);

				TableView.BackgroundView = new UIVisualEffectView (blur);
			}
		}


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			Analytics.TrackPageViewStart (this, Pages.LocationSearch);
		}


		public override void ViewDidDisappear (bool animated)
		{
			Analytics.TrackPageViewEnd (this, new Dictionary<string, string> {
				{ "selected", selectedLocation.ToString() },
				{ "canceled", (!selectedLocation).ToString() }
			});

			selectedLocation = false;

			base.ViewDidDisappear (animated);
		}


		public override nfloat HeaderHeight => 0;


		public override nint RowsInSection (UITableView tableView, nint section) => LocationResults?.Count ?? 0;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = DequeCell (tableView, indexPath);

			cell.TextLabel.AttributedText = ResultStrings [indexPath.Row];

			return cell;
		}


		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			selectedLocation = true;

			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

			var location = LocationResults [indexPath.Row];

			Task.Run (async () =>
			{
				await WuClient.Shared.AddLocation (location);

				BeginInvokeOnMainThread (() =>
				{
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

					TableView.ReloadData ();
				});
			});
		}


		partial void emptyViewClicked (NSObject sender)
		{
			var searchController = ParentViewController as UISearchController;

			if (searchController != null) searchController.Active = false;
		}


		void initEmptyView ()
		{
			emptyView.TranslatesAutoresizingMaskIntoConstraints = false;

			ParentViewController?.View.AddSubview (emptyView);

			ParentViewController?.View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"H:|[emptyView]|", 0, "emptyView", emptyView));
			ParentViewController?.View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"V:|[emptyView]|", 0, "emptyView", emptyView));
		}


		#region IUISearchResultsUpdating


		[Export ("updateSearchResultsForSearchController:")]
		public async void UpdateSearchResultsForSearchController (UISearchController searchController)
		{
			var searchString = searchController.SearchBar?.Text;

			try
			{
				ResultStrings = new List<NSAttributedString> ();

				if (!string.IsNullOrWhiteSpace (searchString))
				{
					LocationResults = await WuAcClient.GetAsync (searchString);

					foreach (var result in LocationResults)
					{
						ResultStrings.Add (result.name.GetSearchResultAttributedString (searchString));
					}
				}
				else
				{
					LocationResults = new List<WuAcLocation> ();
				}

				if (LocationResults.Any ())
				{
					emptyView.RemoveFromSuperview ();
				}
				else if (!emptyView.IsDescendantOfView (ParentViewController.View))
				{
					initEmptyView ();
				}

				TableView?.ReloadData ();

				MaskCells (TableView);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine (ex.Message);
			}
		}


		#endregion


		public override bool PrefersStatusBarHidden () => true;
	}
}