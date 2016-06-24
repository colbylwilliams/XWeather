using Microsoft.WindowsAzure.MobileServices;

namespace XWeather.Droid
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			CurrentPlatform.Init ();
		}
	}
}