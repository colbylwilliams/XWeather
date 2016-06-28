using System;

using Foundation;
using UIKit;

using XWeather.Clients;
using XWeather.Domain;
using XWeather.Unified;

using System.Collections.Generic;

namespace XWeather.iOS
{
	public partial class LocationTvc : BaseTvc<LocationTvCell>, IUISearchResultsUpdating
	{
		UISearchController searchController;

		LocationSearchTvc resultsController;

		List<WuLocation> Locations = WuClient.Shared.Locations;
		//List<string> locations = new List<string> { "foo", "bar" };

		List<WuAcLocation> LocationResults = new List<WuAcLocation> ();

		List<NSAttributedString> resultStrings = new List<NSAttributedString> ();


		public LocationTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			setupSearchController ();

			if (!UIAccessibility.IsReduceTransparencyEnabled) {

				TableView.BackgroundColor = UIColor.Clear;
				var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Dark);
				TableView.BackgroundView = new UIVisualEffectView (blur);
			}

			//TableView.BackgroundView = new UIView { Frame = View.Frame, BackgroundColor = UIColor.Orange };
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
				WuClient.Shared.Current = Locations [indexPath.Row];

				DismissViewController (true, null);

			} else {

				//WuClient.Shared.Current = Locations [indexPath.Row];

				// add to the locations json 
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