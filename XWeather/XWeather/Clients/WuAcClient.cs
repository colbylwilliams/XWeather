using System;
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


		public static Task<WuAcResults> GetAsync (string searchString, bool cancel = false)
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

				return client?.GetAsync<WuAcResults> (ApiKeys.WuAcQueryFmt.Fmt (searchString));

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