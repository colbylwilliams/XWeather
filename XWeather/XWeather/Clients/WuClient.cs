using System;
using System.Threading.Tasks;

using ServiceStack;

using XWeather.Domain;
using XWeather.Constants;

namespace XWeather.Clients
{
	public static class WuClient
	{
		static JsonServiceClient _client;

		static JsonServiceClient client => _client ?? (_client = new JsonServiceClient ());


		public static Task<T> GetAsync<T> (string location)
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