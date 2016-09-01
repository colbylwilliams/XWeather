using System;

using CoreLocation;
using MapKit;

namespace XWeather.iOS
{
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