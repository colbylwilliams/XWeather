using System;
using System.Threading.Tasks;

using UIKit;

using ServiceStack;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.iOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Button.TouchUpInside += delegate {

				Task.Run (async () => {

					var weather = await WuClient.GetAsync<WuWeather> ("zmw:94125.1.99999");

					System.Diagnostics.Debug.WriteLine (weather.SerializeToString ());
				});
			};
		}
	}
}