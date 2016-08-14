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


		public static async Task<List<WuAcLocation>> GetAsync (string searchString, bool cancel = false)
		{
			if (cancel) {

				try {

					client?.CancelAsync ();

				} catch (Exception ex) {

					// System.Net.WebConnection is throwing exceptions here. I think it's there bug, but we're trying to cancel anyway, so just ignore
					System.Diagnostics.Debug.WriteLine ($"Exception Cancelling Service Request with type AutoCompleteResults (not fatal)\n{ex.Message}");
				}
			}

			try {

				var results = await client?.GetAsync<WuAcResults> (ApiKeys.WuAcQueryFmt.Fmt (searchString));

				// filter out the bs
				return results.Results.Where (r => r.type.EqualsIgnoreCase ("city") && !r.c.EqualsIgnoreCase ("(null)")).ToList ();

			} catch (WebServiceException webEx) {

				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground Auto Complete request\n{webEx.Message}");
				// return new List<WuAcLocation> ();
				throw;

			} catch (Exception ex) {

				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground Auto Complete request\n{ex.Message}");
				// return new List<WuAcLocation> ();
				throw;
			}
		}
	}
}