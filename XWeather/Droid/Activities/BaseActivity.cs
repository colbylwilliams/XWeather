using System;

using Android.Support.V7.App;

using XWeather.Clients;

namespace XWeather.Droid
{
	public class BaseActivity : AppCompatActivity
	{

		protected override void OnStart ()
		{
			base.OnStart ();

			WuClient.Shared.UpdatedSelected += HandleUpdatedSelectedLocation;
			WuClient.Shared.LocationAdded += HandleNewLocationAdded;
		}


		protected override void OnStop ()
		{
			base.OnStop ();

			WuClient.Shared.UpdatedSelected -= HandleUpdatedSelectedLocation;
			WuClient.Shared.LocationAdded -= HandleNewLocationAdded;
		}


		protected virtual void HandleUpdatedSelectedLocation (object sender, EventArgs e) { }
		protected virtual void HandleNewLocationAdded (object sender, EventArgs e) { }
	}
}