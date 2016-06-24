using Microsoft.WindowsAzure.MobileServices;

namespace XWeather
{
	public class AzureClient
	{
		public static MobileServiceClient MobileService = new MobileServiceClient (ApiKeys.AzureWebsite);


		public void Init()
		{
			//await MobileService.GetTable<TodoItem> ().InsertAsync (item);
		}
	}
}