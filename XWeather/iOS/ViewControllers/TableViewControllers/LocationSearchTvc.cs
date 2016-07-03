using System;
using System.Collections.Generic;
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

		public List<WuAcLocation> LocationResults = new List<WuAcLocation> ();

		public List<NSAttributedString> ResultStrings = new List<NSAttributedString> ();


		public LocationSearchTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//if (TableView.ContentInset.Top > 0) return;
			//TableView.ContentInset = new UIEdgeInsets (44.0f, 0.0f, 0.0f, 0.0f);
			//TableView.ContentOffset = new CoreGraphics.CGPoint (0.0f, 44.0f);

			if (!UIAccessibility.IsReduceTransparencyEnabled) {

				TableView.BackgroundColor = UIColor.Clear;

				var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Dark);

				TableView.BackgroundView = new UIVisualEffectView (blur);
			}
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
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

			var location = LocationResults [indexPath.Row];

			Task.Run (async () => {

				await WuClient.Shared.AddLocation (location);

				Settings.LocationsJson = WuClient.Shared.Locations.GetLocationsJson ();

				BeginInvokeOnMainThread (() => {

					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

					TableView.ReloadData ();
				});
			});
		}


		#region IUISearchResultsUpdating


		[Export ("updateSearchResultsForSearchController:")]
		public async void UpdateSearchResultsForSearchController (UISearchController searchController)
		{
			var searchString = searchController.SearchBar?.Text;

			try {

				ResultStrings = new List<NSAttributedString> ();

				if (!string.IsNullOrWhiteSpace (searchString)) {

					LocationResults = await WuAcClient.GetAsync (searchString, true);

					foreach (var result in LocationResults) {
						ResultStrings.Add (result.name.GetSearchResultAttributedString (searchString));
					}

				} else {

					LocationResults = new List<WuAcLocation> ();
				}

				TableView?.ReloadData ();

			} catch (Exception ex) {

				System.Diagnostics.Debug.WriteLine (ex.Message);
			}
		}


		#endregion


		public override bool PrefersStatusBarHidden () => true;
	}
}