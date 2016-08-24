using System;
using System.Threading.Tasks;

using Foundation;

using ServiceStack;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Unified
{
	public static class TestDataProvider
	{
		public static void InitTestDataAsync ()
		{
			foreach (var location in TestData.Locations) {

				var name = location.name.Split (',') [0].Replace (' ', '_');

				var path = NSBundle.MainBundle.PathForResource (name, "json");

				using (var data = NSData.FromFile (path)) {

					var json = NSString.FromData (data, NSStringEncoding.ASCIIStringEncoding).ToString ();

					var weather = json?.FromJson<WuWeather> ();

					WuClient.Shared.Locations.Add (new WuLocation (location, weather));
				}
			}

			var i = new Random ().Next (5);

			WuClient.Shared.Selected = WuClient.Shared.Locations [i];
		}
	}
}