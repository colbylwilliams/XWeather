using System;
using System.Threading;
using System.Threading.Tasks;

using CoreLocation;
using Foundation;
using MapKit;
using UIKit;

using XWeather.Clients;

namespace XWeather.iOS
{
	public partial class RadarMapVc : UIViewController, IMKMapViewDelegate
	{

		bool animateRadar;

		RadarOverlay radarOverlay;

		CancellationTokenSource refreshOverlayCts, refreshTimerCts;


		public RadarMapVc (IntPtr handle) : base (handle) { }


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			var location = WuClient.Shared.Selected;

			mapView.UserTrackingMode = MKUserTrackingMode.None;
			mapView.CenterCoordinate = new CLLocationCoordinate2D (location.Location.lat, location.Location.lon);
			mapView.Camera.Altitude = 500000;
		}


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			Analytics.TrackPageViewStart (this, Pages.WeatherRadar);

			animateRadar = true;

			refreshRadarOverlay ();
		}


		public override void ViewDidDisappear (bool animated)
		{
			Analytics.TrackPageViewEnd (this);

			base.ViewDidDisappear (animated);
		}


		partial void closeClicked (NSObject sender)
		{
			animateRadar = false;

			KillTimer ();

			DismissViewController (true, null);
		}


		[Export ("mapView:rendererForOverlay:")]
		public MKOverlayRenderer OverlayRenderer (MKMapView mapView, IMKOverlay overlay)
		{
			var renderer = new RadarOverlayRenderer (overlay);

			if (overlay is RadarOverlay) renderer.RefreshRenderer ();

			return renderer;
		}


		[Export ("mapView:regionWillChangeAnimated:")]
		public void RegionWillChange (MKMapView mapView, bool animated) { }


		[Export ("mapView:regionDidChangeAnimated:")]
		public void RegionChanged (MKMapView mapView, bool animated)
		{
			if (animateRadar) refreshRadarOverlay ();
		}


		void updateRendererDisplayOnMainThread ()
		{
			BeginInvokeOnMainThread (() =>
			{
				if (radarOverlay != null)
				{
					var render = mapView?.RendererForOverlay (radarOverlay) as RadarOverlayRenderer;
					render?.UpdateDisplay ();
				}
			});
		}


		void initTimer ()
		{
			Task.Run (async () =>
			{
				try
				{
					KillTimer ();

					refreshTimerCts = new CancellationTokenSource ();

					updateRendererDisplayOnMainThread ();

					while (animateRadar)
					{
						await Task.Delay (500, refreshTimerCts.Token);

						updateRendererDisplayOnMainThread ();
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
			});
		}


		public void KillTimer () => refreshTimerCts?.Cancel ();


		void refreshRadarOverlay ()
		{
			try
			{
				refreshOverlayCts?.Cancel ();

				refreshOverlayCts = new CancellationTokenSource ();

				System.Diagnostics.Debug.WriteLine ("Begin Radar Request");

				refreshRadarOverlay (refreshOverlayCts.Token);

				refreshOverlayCts.Token.ThrowIfCancellationRequested ();

				initTimer ();
			}
			catch (OperationCanceledException)
			{
				System.Diagnostics.Debug.WriteLine ("Canceled Radar Request");
				return;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine ($"Error During Radar Request\n{ex.Message}");
				throw;
			}

			System.Diagnostics.Debug.WriteLine ("Finished Radar Request");
		}


		void refreshRadarOverlay (CancellationToken token)
		{
			token.ThrowIfCancellationRequested ();

			var bounds = mapView.GetRadarBoundsForScreen ();

			Task.Run (async () =>
			{
				var radar = await WuClient.Shared.GetRadarImageAsync (bounds);

				if (radar == null) refreshOverlayCts?.Cancel ();

				token.ThrowIfCancellationRequested ();

				System.Diagnostics.Debug.WriteLine ("Begin Splitting Images");
				var images = radar.SplitAnimatedGifImages ();
				System.Diagnostics.Debug.WriteLine ("Finished Splitting Images");

				token.ThrowIfCancellationRequested ();

				BeginInvokeOnMainThread (() =>
				{
					if (radarOverlay != null) mapView.RemoveOverlay (radarOverlay);

					radarOverlay = new RadarOverlay (mapView.VisibleMapRect, images);

					mapView.AddOverlay (radarOverlay);

					token.ThrowIfCancellationRequested ();

					var render = mapView.RendererForOverlay (radarOverlay) as RadarOverlayRenderer;

					render?.RefreshRenderer ();
				});
			});
		}
	}
}