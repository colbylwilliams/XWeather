using System.Threading.Tasks;

using Android.App;
using Android.Widget;
using Android.OS;

using ServiceStack;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Droid
{
	[Activity (Label = "XWeather", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			Bootstrap.Run ();

			SetContentView (Resource.Layout.Main);

			Button button = FindViewById<Button> (Resource.Id.myButton);

			button.Click += delegate {

				Task.Run (async () => {

					var weather = await WuClient.GetAsync<WuWeather> ("zmw:94125.1.99999");

					System.Diagnostics.Debug.WriteLine (weather.SerializeToString ());
				});
			};
		}
	}
}