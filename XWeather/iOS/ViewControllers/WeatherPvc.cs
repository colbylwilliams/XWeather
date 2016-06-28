using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UIKit;
using XWeather.Clients;
using XWeather.Domain;
using XWeather.Unified;
using System.Linq;
using CoreAnimation;

namespace XWeather.iOS
{
	public partial class WeatherPvc : UIPageViewController
	{

		List<UITableViewController> Controllers = new List<UITableViewController> (3);
		//List<UIViewController> Controllers = new List<UIViewController> (3);

		public WeatherPvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			initToolbar ();

			initControllers ();
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			reloadData ();
		}


		void updateBackground ()
		{
			/* c0lby: Set the weather's conditional 'overlays' as the layers content prop by
			 * supplying a delegate for the layer or subclassing.  This will improve performance */

			var location = WuClient.Shared.Current;

			if (location == null) return;

			var layer = View.Layer.Sublayers [0] as CAGradientLayer;

			if (layer == null) {
				layer = new CAGradientLayer ();
				layer.Frame = View.Bounds;
				View.Layer.InsertSublayer (layer, 0);
			}

			var gradients = location.GetTimeOfDayGradient ();

			if (layer?.Colors?.Length > 0 && layer.Colors [0] == gradients.Item1 [0] && layer.Colors [1] == gradients.Item1 [1])
				return;

			CATransaction.Begin ();
			CATransaction.AnimationDuration = 1.5;
			layer.Colors = gradients.Item1;
			CATransaction.Commit ();
		}


		void initControllers ()
		{
			Controllers = new List<UITableViewController> { Storyboard.Instantiate<DailyTvc> (), Storyboard.Instantiate<HourlyTvc> (), Storyboard.Instantiate<DetailsTvc> () };
			//Controllers = new List<UIViewController> { Storyboard.Instantiate<DailyTvc> (), Storyboard.Instantiate<HourlyTvc> (), Storyboard.Instantiate<DetailsTvc> () };

			GetNextViewController += (pvc, rvc) => rvc.Equals (Controllers [0]) ? Controllers [1] : rvc.Equals (Controllers [1]) ? Controllers [2] : null;

			GetPreviousViewController += (pvc, rvc) => rvc.Equals (Controllers [2]) ? Controllers [1] : rvc.Equals (Controllers [1]) ? Controllers [0] : null;

			WillTransition += (s, e) => { updateBackground (); };

			DidFinishAnimating += (s, e) => pageIndicator.CurrentPage = Controllers.IndexOf ((UITableViewController)ViewControllers [0]);

			SetViewControllers (new [] { Controllers [0] }, UIPageViewControllerNavigationDirection.Forward, false, (finished) => { getData (); });
		}


		void initToolbar ()
		{
			toolbarView.TranslatesAutoresizingMaskIntoConstraints = false;

			View.AddSubview (toolbarView);

			View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"H:|[toolbarView]|", 0, "toolbarView", toolbarView));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"V:[toolbarView(44.0)]|", 0, "toolbarView", toolbarView));
		}


		void getData ()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

			Task.Run (async () => {

				await WuClient.Shared.GetLocations (TestData.LocationsJson);

				WuClient.Shared.Current = WuClient.Shared.Locations [1];

				BeginInvokeOnMainThread (() => {

					reloadData ();

					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				});
			});
		}


		void reloadData ()
		{
			updateBackground ();

			foreach (var controller in Controllers)
				controller?.TableView?.ReloadData ();

		}

		public override UIStatusBarStyle PreferredStatusBarStyle () => UIStatusBarStyle.LightContent;
	}
}