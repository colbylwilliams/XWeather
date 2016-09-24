using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ServiceStack;

using XWeather.Domain;
using XWeather.Constants;

namespace XWeather.Clients
{
	public static class WuAcClient
	{
		static JsonServiceClient _client;

		static JsonServiceClient client => _client ?? (_client = new JsonServiceClient ());


		public static async Task<List<WuAcLocation>> GetAsync (string searchString)
		{
			try {

				var results = await client?.GetAsync<WuAcResults> (ApiKeys.WuAcQueryFmt.Fmt (searchString));

				// filter out the bs
				return results.Results.Where (r => r.type.EqualsIgnoreCase ("city") && !r.c.EqualsIgnoreCase ("(null)")).ToList ();

			} catch (WebServiceException webEx) {

				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground Auto Complete request\n{webEx.Message}");

				throw;

			} catch (Exception ex) {

				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground Auto Complete request\n{ex.Message}");

				throw;
			}
		}
	}
}