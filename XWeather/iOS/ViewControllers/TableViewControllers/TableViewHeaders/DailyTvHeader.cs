using System;

using UIKit;

namespace XWeather.iOS
{
	public partial class DailyTvHeader : UIView
	{
		//bool addedGraph;

		public DailyTvHeader (IntPtr handle) : base (handle) { }

		public void SetData (WuLocation location)
		{
			locationLabel.Text = location?.Name;

			// redraw graph
			graphView.SetNeedsDisplay ();
		}
	}
}