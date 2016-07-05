using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoreAnimation;
using Foundation;
using UIKit;

using ServiceStack;
using SettingsStudio;


using XWeather.Clients;
using XWeather.Domain;
using XWeather.Unified;

namespace XWeather.iOS
{
	public partial class WeatherPvc : UIPageViewController
	{

		LocationProvider LocationProvider;

		List<UITableViewController> Controllers = new List<UITableViewController> (3);


		public WeatherPvc (IntPtr handle) : base (handle)
		{
			LocationProvider = new LocationProvider ();
		}


		public override void ViewDidLoad ()
		{
			WuClient.Shared.UpdatedSelected += handleUpdatedCurrent;

			base.ViewDidLoad ();

			initEmptyView ();

			initToolbar ();

			initControllers ();
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			updateToolbarButtons (true);
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			//if (segue.Identifier == "locationsSegue") updateToolbarButtons (false);
			updateToolbarButtons (false);
		}


		public override UIStatusBarStyle PreferredStatusBarStyle () => UIStatusBarStyle.LightContent;


		partial void closeClicked (NSObject sender)
		{
			updateToolbarButtons (true);
			DismissViewController (true, null);
		}


		partial void settingsClicked (NSObject sender)
		{
			var settingsString = UIApplication.OpenSettingsUrlString?.ToString ();

			if (!string.IsNullOrEmpty (settingsString)) {
				var settingsUrl = NSUrl.FromString (settingsString);
				UIApplication.SharedApplication.OpenUrl (settingsUrl);
			}
		}


		void updateToolbarButtons (bool dismissing)
		{
			foreach (var button in toolbarButtons)
				button.Hidden = dismissing ? button.Tag > 1 : button.Tag < 2;

			pageIndicator.Hidden = !dismissing;
		}


		void handleUpdatedCurrent (object sender, EventArgs e)
		{
			BeginInvokeOnMainThread (() => {
				updateToolbarButtons (true);
				reloadData ();
				Settings.LocationsJson = WuClient.Shared.Locations.GetLocationsJson ();
			});
		}


		void reloadData ()
		{
			updateBackground ();

			if (WuClient.Shared.HasCurrent) removeEmptyView ();

			foreach (var controller in Controllers) controller?.TableView?.ReloadData ();
		}


		void updateBackground ()
		{
			/* c0lby: Set the weather's conditional 'overlays' as the layers content prop by
			 * supplying a delegate for the layer or subclassing.  This will improve performance */

			var location = WuClient.Shared.Selected;

			var random = location == null || Settings.RandomBackgrounds;

			var layer = View.Layer.Sublayers [0] as CAGradientLayer;

			if (layer == null) {
				layer = new CAGradientLayer ();
				layer.Frame = View.Bounds;
				View.Layer.InsertSublayer (layer, 0);
			}

			var gradients = location.GetTimeOfDayGradient (random);

			if (layer?.Colors?.Length > 0 && layer.Colors [0] == gradients.Item1 [0] && layer.Colors [1] == gradients.Item1 [1])
				return;

			CATransaction.Begin ();
			CATransaction.AnimationDuration = 1.5;
			layer.Colors = gradients.Item1;
			CATransaction.Commit ();
		}


		#region Views


		void initControllers ()
		{
			Controllers = new List<UITableViewController> { Storyboard.Instantiate<DailyTvc> (), Storyboard.Instantiate<HourlyTvc> (), Storyboard.Instantiate<DetailsTvc> () };

			GetNextViewController += (pvc, rvc) => rvc.Equals (Controllers [0]) ? Controllers [1] : rvc.Equals (Controllers [1]) ? Controllers [2] : null;

			GetPreviousViewController += (pvc, rvc) => rvc.Equals (Controllers [2]) ? Controllers [1] : rvc.Equals (Controllers [1]) ? Controllers [0] : null;

			WillTransition += (s, e) => { updateBackground (); };

			DidFinishAnimating += (s, e) => {
				var index = Controllers.IndexOf ((UITableViewController)ViewControllers [0]);
				pageIndicator.CurrentPage = index;
				Settings.WeatherPage = index;
			};

			SetViewControllers (new [] { Controllers [Settings.WeatherPage] }, UIPageViewControllerNavigationDirection.Forward, false, (finished) => { getData (); });

			pageIndicator.CurrentPage = Settings.WeatherPage;
		}


		void initToolbar ()
		{
			toolbarView.TranslatesAutoresizingMaskIntoConstraints = false;

			NavigationController.View.AddSubview (toolbarView);

			NavigationController.View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"H:|[toolbarView]|", 0, "toolbarView", toolbarView));
			NavigationController.View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"V:[toolbarView(44.0)]|", 0, "toolbarView", toolbarView));
		}


		void initEmptyView ()
		{
			emptyView.TranslatesAutoresizingMaskIntoConstraints = false;

			View.AddSubview (emptyView);

			loadingIndicatorView.Hidden = true;

			View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"H:|[emptyView]|", 0, "emptyView", emptyView));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"V:|[emptyView]|", 0, "emptyView", emptyView));

			var layer = emptyView.Layer.Sublayers [0] as CAGradientLayer;

			if (layer == null) {
				layer = new CAGradientLayer ();
				layer.Frame = View.Bounds;
				emptyView.Layer.InsertSublayer (layer, 0);
			}

			layer.Colors = Colors.Gradients [7];
		}


		void removeEmptyView ()
		{
			if (emptyView.IsDescendantOfView (View)) {

				loadingIndicatorView.StopAnimating ();

				UIView.Animate (0.5, () => emptyView.Alpha = 0, () => emptyView.RemoveFromSuperview ());
			}
		}


		#endregion


		void getData ()
		{
#if DEBUG
			Task.Run (async () => {

				await Task.Delay (1000);

				foreach (var location in TestData.Locations) {

					var name = location.name.Split (',') [0].Replace (' ', '_');

					var path = NSBundle.MainBundle.PathForResource (name, "json");

					using (var data = NSData.FromFile (path)) {

						var json = NSString.FromData (data, NSStringEncoding.ASCIIStringEncoding).ToString ();

						var weather = json?.FromJson<WuWeather> ();

						WuClient.Shared.Locations.Add (new WuLocation (location, weather));
					}
				}

				var i = new Random ().Next (5);

				WuClient.Shared.Selected = WuClient.Shared.Locations [i];

			});

#else
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

			Task.Run (async () => {

				var location = await LocationProvider.GetCurrentLocationAsync ();

				if (location != null) {

					await WuClient.Shared.GetLocations (Settings.LocationsJson, location.Coordinate.Latitude, location.Coordinate.Longitude);

					BeginInvokeOnMainThread (() => {

						UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					});
				}
			});

#endif
		}
	}
}