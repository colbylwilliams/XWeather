﻿using System;
using System.Collections.Generic;
using System.Linq;

using CoreAnimation;
using Foundation;
using UIKit;

using SettingsStudio;

using XWeather.Clients;
using XWeather.Unified;

namespace XWeather.iOS
{
	public partial class WeatherPvc : UIPageViewController
	{

		List<UITableViewController> Controllers = new List<UITableViewController> (3);


		public WeatherPvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			WuClient.Shared.UpdatedSelected += handleFirstUpdatedSelected;

			base.ViewDidLoad ();

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
			updateToolbarButtons (false);

			if (segue.Identifier.Equals ("locationsSegue", StringComparison.OrdinalIgnoreCase))
			{
				var current = ViewControllers.FirstOrDefault ();

				if (current != null)
					Analytics.TrackPageViewEnd (ViewControllers.FirstOrDefault (), WuClient.Shared.Selected);
			}
		}


		public override UIStatusBarStyle PreferredStatusBarStyle () => UIStatusBarStyle.LightContent;


		partial void closeClicked (NSObject sender)
		{
			updateToolbarButtons (true);

            DismissViewController(true, () =>
            {
                var current = ViewControllers.FirstOrDefault();

                if (current != null)
                    Analytics.TrackPageViewStart(current, childPageName(current), WuClient.Shared.Selected);
            });
		}


		partial void settingsClicked (NSObject sender)
		{
			var settingsString = UIApplication.OpenSettingsUrlString?.ToString ();

			if (!string.IsNullOrEmpty (settingsString))
			{
				var settingsUrl = NSUrl.FromString (settingsString);

				if (UIApplication.SharedApplication.OpenUrl (settingsUrl))
				{
					Analytics.TrackPageView (Pages.Settings.Name ());
				}
			}
		}


		void updateToolbarButtons (bool dismissing)
		{
			foreach (var button in toolbarButtons)
			{
				button.Hidden = dismissing ? button.Tag > 1 : button.Tag < 2;
			}

			pageIndicator.Hidden = !dismissing;
		}


		void handleFirstUpdatedSelected (object sender, EventArgs e)
		{
			WuClient.Shared.UpdatedSelected -= handleFirstUpdatedSelected;
			WuClient.Shared.UpdatedSelected += handleUpdatedSelected;

			refreshForUpdatedSelected ();
		}


		void handleUpdatedSelected (object sender, EventArgs e)
		{
			refreshForUpdatedSelected ();

			var current = ViewControllers.FirstOrDefault ();

			if (current != null)
				Analytics.TrackPageViewStart (current, childPageName (current), WuClient.Shared.Selected);
		}


		void refreshForUpdatedSelected ()
		{
			BeginInvokeOnMainThread (() =>
			{
				updateToolbarButtons (true);
				reloadData ();
			});
		}

		void reloadData ()
		{
			updateBackground ();

			foreach (var controller in Controllers) controller?.TableView?.ReloadData ();
		}


		void updateBackground ()
		{
			var location = WuClient.Shared.Selected;

			var random = location == null || Settings.RandomBackgrounds;

			var layer = View.Layer.Sublayers [0] as CAGradientLayer;

			if (layer == null)
			{
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


		void initControllers ()
		{
			Controllers = new List<UITableViewController> { Storyboard.Instantiate<DailyTvc> (), Storyboard.Instantiate<HourlyTvc> (), Storyboard.Instantiate<DetailsTvc> () };

			GetNextViewController += (pvc, rvc) => rvc.Equals (Controllers [0]) ? Controllers [1] : rvc.Equals (Controllers [1]) ? Controllers [2] : null;

			GetPreviousViewController += (pvc, rvc) => rvc.Equals (Controllers [2]) ? Controllers [1] : rvc.Equals (Controllers [1]) ? Controllers [0] : null;

			WillTransition += (s, e) => { updateBackground (); };

			DidFinishAnimating += (s, e) =>
			{
				var index = Controllers.IndexOf ((UITableViewController)ViewControllers [0]);
				pageIndicator.CurrentPage = index;
				Settings.WeatherPage = index;
			};

			SetViewControllers (new [] { Controllers [Settings.WeatherPage] }, UIPageViewControllerNavigationDirection.Forward, false, (finished) => { getData (); });

			pageIndicator.CurrentPage = Settings.WeatherPage;

			updateBackground ();
		}


		void initToolbar ()
		{
			toolbarView.TranslatesAutoresizingMaskIntoConstraints = false;

			NavigationController.View.AddSubview (toolbarView);

			NavigationController.View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"H:|[toolbarView]|", 0, "toolbarView", toolbarView));
			NavigationController.View.AddConstraints (NSLayoutConstraint.FromVisualFormat (@"V:[toolbarView(44.0)]|", 0, "toolbarView", toolbarView));
		}


		Pages childPageName (UIViewController page)
		{
			if (page != null)
			{
				if (page.Equals (Controllers [0])) return Pages.WeatherDaily;
				if (page.Equals (Controllers [1])) return Pages.WeatherHourly;
				if (page.Equals (Controllers [2])) return Pages.WeatherDetails;
			}
			return Pages.Unknown;
		}

#if DEBUG

		void getData () => TestDataProvider.InitTestDataAsync ();
#else

		LocationProvider LocationProvider;

		void getData ()
		{
			if (LocationProvider == null) LocationProvider = new LocationProvider ();

			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

			System.Threading.Tasks.Task.Run (async () =>
			{
				var location = await LocationProvider.GetCurrentLocationCoordnatesAsync ();

				await WuClient.Shared.GetLocations (location);

				BeginInvokeOnMainThread (() =>
				{
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				});
			});
		}
#endif
	}
}