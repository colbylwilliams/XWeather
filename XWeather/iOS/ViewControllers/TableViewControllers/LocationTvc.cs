using System;

using Foundation;
using UIKit;

using XWeather.Clients;
using XWeather.Domain;
using XWeather.Unified;

using System.Collections.Generic;
using CoreGraphics;
using System.Threading.Tasks;
using SettingsStudio;

namespace XWeather.iOS
{
	public partial class LocationTvc : BaseTvc<LocationTvCell>, IUISearchResultsUpdating
	{
		UISearchController searchController;

		LocationSearchTvc resultsController;

		List<WuLocation> Locations => WuClient.Shared.Locations;

		List<WuAcLocation> LocationResults = new List<WuAcLocation> ();

		List<NSAttributedString> resultStrings = new List<NSAttributedString> ();


		nfloat searchBarHeight => searchController?.SearchBar?.Frame.Height ?? 0;

		nfloat rowHeight = 44;


		public LocationTvc (IntPtr handle) : base (handle)
		{

		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TableView.ContentInset = new UIEdgeInsets (20, 0, 0, 0);

			//TableView.ContentOffset = new CGPoint (0, 24);

			setupSearchController ();

			if (!UIAccessibility.IsReduceTransparencyEnabled) {

				TableView.BackgroundColor = UIColor.Clear;

				var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Dark);

				TableView.BackgroundView = new UIVisualEffectView (blur);
			}
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//TableView.SetContentOffset (new CGPoint (0, searchBarHeight - 20), false);
		}


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			TableView.SetContentOffset (new CGPoint (0, searchBarHeight - 20), true);
		}


		void setupSearchController ()
		{
			resultsController = Storyboard.Instantiate<LocationSearchTvc> ();

			searchController = new UISearchController (resultsController) {
				DimsBackgroundDuringPresentation = false,
				SearchResultsUpdater = this,
				WeakDelegate = this
			};

			searchController.SearchBar.BarStyle = UIBarStyle.Black;
			searchController.SearchBar.Translucent = true;
			searchController.SearchBar.TintColor = Colors.TintGray;
			searchController.SearchBar.KeyboardAppearance = UIKeyboardAppearance.Dark;


			searchController.SearchBar.WeakDelegate = this;

			TableView.TableHeaderView = searchController.SearchBar;

			resultsController.TableView.WeakDelegate = this;
			resultsController.TableView.WeakDataSource = this;

			//searchController.SearchBar.TintColor = Colors.ElitePartnerColor;

			DefinesPresentationContext = true;
		}


		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			if (TableView.Equals (tableView)) {

				return tableView.Frame.Height - ((rowHeight * Locations.Count) + FooterHeight + searchBarHeight) + 20;
			}

			return searchBarHeight;
		}


		public override nint NumberOfSections (UITableView tableView) => 1;


		public override nint RowsInSection (UITableView tableView, nint section)
			=> tableView.Equals (TableView) ? Locations?.Count ?? 0 : LocationResults?.Count ?? 0;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (tableView.Equals (TableView)) {

				var cell = DequeCell (tableView, indexPath);

				var location = Locations [indexPath.Row];

				cell.TextLabel.Text = location.Location.name;

				return cell;

			} else {

				var cell = resultsController.DequeCell (tableView, indexPath);

				cell.TextLabel.AttributedText = resultStrings [indexPath.Row];

				return cell;
			}
		}


		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (tableView.Equals (TableView)) {

				// set location as the selected location
				WuClient.Shared.Selected = Locations [indexPath.Row];

				DismissViewController (true, null);

			} else {

				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

				var location = LocationResults [indexPath.Row];

				searchController.Active = false;

				Task.Run (async () => {

					await WuClient.Shared.AddLocation (location);

					Settings.LocationsJson = WuClient.Shared.Locations.GetLocationsJson ();

					BeginInvokeOnMainThread (() => {

						UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

						TableView.ReloadData ();

					});
				});
			}
		}


		#region IUISearchResultsUpdating


		[Export ("updateSearchResultsForSearchController:")]
		public async void UpdateSearchResultsForSearchController (UISearchController searchController)
		{
			var searchString = searchController.SearchBar?.Text;

			try {

				resultStrings = new List<NSAttributedString> ();

				if (!string.IsNullOrWhiteSpace (searchString)) {

					LocationResults = await WuAcClient.GetAsync (searchString, true);

					foreach (var result in LocationResults) {
						resultStrings.Add (result.name.GetSearchResultAttributedString (searchString));
					}

				} else {

					LocationResults = new List<WuAcLocation> ();
				}

				resultsController?.TableView?.ReloadData ();

			} catch (Exception ex) {

				System.Diagnostics.Debug.WriteLine (ex.Message);
			}
		}


		#endregion
	}
}