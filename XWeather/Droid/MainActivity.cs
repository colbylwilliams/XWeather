using Android.App;
using Android.OS;

namespace XWeather.Droid
{
	[Activity (Label = "XWeather", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			Bootstrap.Run (this, Application);

			SetContentView (Resource.Layout.Main);
		}
	}
}