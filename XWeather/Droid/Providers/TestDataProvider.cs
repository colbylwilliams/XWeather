using System;
using System.IO;
using System.Threading.Tasks;

using Android.App;

using ServiceStack;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Droid
{
	public static class TestDataProvider
	{

		public static async Task InitTestDataAsync (Activity context)
		{
			await Task.Delay (10);

			foreach (var location in TestData.Locations) {

				var name = location.name.Split (',') [0].Replace (' ', '_');

				var path = $"{name}.json";

				using (var sr = new StreamReader (context.Assets.Open (path))) {

					var json = sr.ReadToEnd ();

					var weather = json?.FromJson<WuWeather> ();

					WuClient.Shared.Locations.Add (new WuLocation (location, weather));
				}
			}

			var i = new Random ().Next (5);

			WuClient.Shared.Selected = WuClient.Shared.Locations [i];
		}
	}
}