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
		}
	}
}