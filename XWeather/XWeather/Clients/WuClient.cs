using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ServiceStack;

using XWeather.Domain;
using XWeather.Constants;

namespace XWeather.Clients
{
	public class WuClient
	{
		static WuClient _shared;

		public static WuClient Shared => _shared ?? (_shared = new WuClient ());


		public WuLocation Current { get; set; }

		public List<WuLocation> Locations { get; set; } = new List<WuLocation> ();


		JsonServiceClient _client;

		JsonServiceClient client => _client ?? (_client = new JsonServiceClient ());


		public async Task GetLocations (string json)
		{
			var locations = json.FromJson<List<WuAcLocation>> ();

			var tasks = locations.Select (l => GetWuLocation (l)).ToArray ();

			var wuLocations = await Task.WhenAll (tasks);

			Locations = new List<WuLocation> (wuLocations);
		}


		async Task<WuLocation> GetWuLocation (WuAcLocation acLocation)
		{
			var location = new WuLocation (acLocation);

			location.Weather = await GetAsync<WuWeather> (acLocation.l);

			location.Updated = DateTime.UtcNow;

			return location;
		}


		public Task<T> GetAsync<T> (string location)
			where T : WuObject, new()
		{
			try {

				return client.GetAsync<T> (ApiKeys.WuApiKeyedQueryFmt.Fmt (new T ().WuKey, location));

			} catch (WebServiceException webEx) {

				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground request for {typeof (T).Name}\n{webEx.Message}");
				throw;

			} catch (Exception ex) {

				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground request for {typeof (T).Name}\n{ex.Message}");
				throw;
			}
		}
	}
}