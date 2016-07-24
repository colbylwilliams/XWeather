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

			//addGraph ();

			//wuIcon.Text = null;
		}

		//void addGraph()
		//{
		//	if (addedGraph) return;

		//	addedGraph = true;

		//	var graph = new DailyGraphView ();

		//	graph.TranslatesAutoresizingMaskIntoConstraints = false;

		//	AddSubview (graph);

		//	AddConstraints (NSLayoutConstraint.FromVisualFormat (@"H:|[graph]|", 0, "graph", graph));
		//	AddConstraints (NSLayoutConstraint.FromVisualFormat (@"V:|-(56)-[graph]|", 0, "graph", graph));
		//}
	}
}