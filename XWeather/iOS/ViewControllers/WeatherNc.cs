using System;

using Foundation;
using UIKit;

namespace XWeather.iOS
{
	public partial class WeatherNc : UINavigationController
	{
		public WeatherNc (IntPtr handle) : base (handle)
		{
		}

		public override void DismissViewController (bool animated, Action completionHandler)
		{
			System.Diagnostics.Debug.WriteLine (TopViewController);

			base.DismissViewController (animated, completionHandler);
		}




	}
}