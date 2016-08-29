using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using UIKit;

using XWeather.Clients;
using XWeather.Unified;

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

			AnalyticsManager.Shared.TrackEvent (TrackedEvents.WeatherRadar.Opened);

			animateRadar = true;

			refreshRadarOverlay ();
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
			BeginInvokeOnMainThread (() => {
				if (radarOverlay != null) {
					var render = mapView?.RendererForOverlay (radarOverlay) as RadarOverlayRenderer;
					render?.UpdateDisplay ();
				}
			});
		}


		void initTimer ()
		{
			Task.Run (async () => {

				try {

					KillTimer ();

					refreshTimerCts = new CancellationTokenSource ();

					updateRendererDisplayOnMainThread ();

					while (animateRadar) {

						await Task.Delay (500, refreshTimerCts.Token);

						updateRendererDisplayOnMainThread ();
					}

				} catch (OperationCanceledException) {
					return;
				}
			});
		}


		public void KillTimer () => refreshTimerCts?.Cancel ();


		void refreshRadarOverlay ()
		{
			try {

				refreshOverlayCts?.Cancel ();

				refreshOverlayCts = new CancellationTokenSource ();

				System.Diagnostics.Debug.WriteLine ("Begin Radar Request");

				refreshRadarOverlay (refreshOverlayCts.Token);

				refreshOverlayCts.Token.ThrowIfCancellationRequested ();

				initTimer ();

			} catch (OperationCanceledException) {

				System.Diagnostics.Debug.WriteLine ("Canceled Radar Request");
				return;

			} catch (Exception ex) {

				System.Diagnostics.Debug.WriteLine ($"Error During Radar Request\n{ex.Message}");
				throw;
			}

			System.Diagnostics.Debug.WriteLine ("Finished Radar Request");
		}


		void refreshRadarOverlay (CancellationToken token)
		{
			token.ThrowIfCancellationRequested ();

			var bounds = mapView.GetRadarBoundsForScreen ();

			Task.Run (async () => {

				var radar = await WuClient.Shared.GetRadarImageAsync (bounds);

				if (radar == null) refreshOverlayCts?.Cancel ();


				token.ThrowIfCancellationRequested ();

				System.Diagnostics.Debug.WriteLine ("Begin Splitting Images");
				var images = radar.SplitAnimatedGifImages ();
				System.Diagnostics.Debug.WriteLine ("Finished Splitting Images");

				token.ThrowIfCancellationRequested ();

				BeginInvokeOnMainThread (() => {

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


	public class RadarOverlay : MKOverlay
	{
		readonly MKMapRect MapRect;

		public List<CGImage> Images { get; set; }

		public RadarOverlay (MKMapRect mapRect, List<CGImage> images)
		{
			MapRect = mapRect;
			Images = images;
		}

		public override CLLocationCoordinate2D Coordinate => new CLLocationCoordinate2D ();

		public override MKMapRect BoundingMapRect => MapRect;
	}


	public class RadarOverlayRenderer : MKOverlayRenderer
	{
		nint index;

		CGRect cachedRect;

		List<CGImage> cachedImages;


		public RadarOverlayRenderer (IMKOverlay overlay) : base (overlay) { }


		public void RefreshRenderer ()
		{
			var radar = Overlay as RadarOverlay;

			if (radar != null) {
				cachedRect = RectForMapRect (radar.BoundingMapRect);
				cachedImages = radar.Images;
				index = 0;
			}
		}


		public void UpdateDisplay ()
		{
			index++;

			if (index >= cachedImages.Count) index = 0;

			SetNeedsDisplay ();
		}


		public override bool CanDrawMapRect (MKMapRect mapRect, nfloat zoomScale) => cachedImages?.Count > index;


		public override void DrawMapRect (MKMapRect mapRect, nfloat zoomScale, CGContext context)
		{
			if (cachedImages?.Count > index)
				context.DrawImage (cachedRect, cachedImages [Convert.ToInt32 (index)]);
		}
	}


	public static class RadarOverlayExtensions
	{
		public static RadarBounds GetRadarBounds (this MKMapView map)
		{
			CLLocationCoordinate2D center = map.CenterCoordinate;

			var topLeft = MKMapPoint.FromCoordinate (new CLLocationCoordinate2D (center.Latitude + (map.Region.Span.LatitudeDelta / 2.0), center.Longitude - (map.Region.Span.LongitudeDelta / 2.0)));
			var bottomRight = MKMapPoint.FromCoordinate (new CLLocationCoordinate2D (center.Latitude - (map.Region.Span.LatitudeDelta / 2.0), center.Longitude + (map.Region.Span.LongitudeDelta / 2.0)));

			var bounds = new RadarBounds {
				MinLat = center.Latitude + (map.Region.Span.LatitudeDelta / 2.0),
				MaxLat = center.Latitude - (map.Region.Span.LatitudeDelta / 2.0),
				MinLon = center.Longitude - (map.Region.Span.LongitudeDelta / 2.0),
				MaxLon = center.Longitude + (map.Region.Span.LongitudeDelta / 2.0),

				Height = Math.Abs (topLeft.Y - bottomRight.Y),
				Width = Math.Abs (topLeft.X - bottomRight.X)
			};

			return bounds;
		}

		public static RadarBounds GetRadarBoundsForScreen (this MKMapView map)
		{
			RadarBounds bounds = map.GetRadarBounds ();
			bounds.Width = map.Frame.Size.Width;
			bounds.Height = map.Frame.Size.Height;
			return bounds;
		}
	}
}