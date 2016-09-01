using System.Collections.Generic;

using CoreGraphics;
using CoreLocation;
using MapKit;

namespace XWeather.iOS
{
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
}