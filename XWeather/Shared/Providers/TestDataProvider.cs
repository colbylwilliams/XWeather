using System;
using System.Linq;
using System.Threading.Tasks;

using ServiceStack;

using NomadCode.Azure;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather
{
	public static class TestDataProvider
	{
#if __ANDROID__
		public static async Task InitTestDataAsync (Android.App.Activity context)
		{
			await Task.Delay (10);
#else
		public static async Task InitTestDataAsync ()
		{
#endif
			try
			{

				var locations = TestData.Locations;

				if (SettingsStudio.Settings.AzureStoreEnabled)
				{
					var existingLocations = await AzureClient.Shared.GetAsync<WuAcLocation> ();

					if (existingLocations?.Any () ?? false)
					{
						locations = existingLocations;
					}
				}
				else
				{
					var existingLocations = SettingsStudio.Settings.LocationsJson?.GetLocations ();

					if (existingLocations?.Any () ?? false)
					{
						locations = existingLocations;
					}
				}


				var selected = locations?.FirstOrDefault (l => l.Selected);

				foreach (var location in locations)
				{
					var name = location.name.Split (',') [0].Replace (' ', '_');

#if __IOS__
				var path = Foundation.NSBundle.MainBundle.PathForResource (name, "json");

				using (var data = Foundation.NSData.FromFile (path))
				{
					var json = Foundation.NSString.FromData (data, Foundation.NSStringEncoding.ASCIIStringEncoding).ToString ();
#elif __ANDROID__
					var path = $"{name}.json";

					using (var sr = new System.IO.StreamReader (context.Assets.Open (path)))
					{
						var json = sr.ReadToEnd ();
#endif
						var weather = json?.FromJson<WuWeather> ();

						if (weather != null)
						{
							await WuClient.Shared.AddLocation (new WuLocation (location, weather), true);
						}
					}
				}

				var lastSelected = WuClient.Shared.Locations.FirstOrDefault (l => l.Location.name.CompareIgnoreCase (selected?.name) == 0);

				if (lastSelected != null)
				{
					WuClient.Shared.Selected = lastSelected;
				}
				else
				{
					var i = new Random ().Next (5);

					WuClient.Shared.Selected = WuClient.Shared.Locations [i];
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine ($"{ex.Message}");
			}
		}
	}
}