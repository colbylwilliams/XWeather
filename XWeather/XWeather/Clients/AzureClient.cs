using Microsoft.WindowsAzure.MobileServices;

using XWeather.Constants;

namespace XWeather.Clients
{
	public class AzureClient
	{
		public static MobileServiceClient MobileService = new MobileServiceClient (ApiKeys.AzureWebsite);


		public void Init ()
		{
			//await MobileService.GetTable<TodoItem> ().InsertAsync (item);
		}
	}
}