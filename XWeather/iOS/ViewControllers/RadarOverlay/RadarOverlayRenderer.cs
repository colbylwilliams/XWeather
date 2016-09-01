using System;
using System.Collections.Generic;

using CoreGraphics;
using MapKit;

namespace XWeather.iOS
{
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
}