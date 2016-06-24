using Microsoft.WindowsAzure.MobileServices;

namespace XWeather.iOS
{
	public static class Bootstrap
	{
		public static void Run()
		{
			CurrentPlatform.Init ();
		}
	}
}