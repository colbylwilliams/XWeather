using Android.Support.V7.App;
using HockeyApp.Android;

namespace XWeather.Droid
{
	public class BaseActivity : AppCompatActivity
	{

		protected override void OnStart ()
		{
			base.OnStart ();

			Tracking.StartUsage (this);
		}


		protected override void OnStop ()
		{
			base.OnStop ();

			Tracking.StopUsage (this);
		}
	}
}