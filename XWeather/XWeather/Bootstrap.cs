using XWeather.Constants;

namespace XWeather
{
	public static class Bootstrap
	{
		public static void Run ()
		{
			ServiceStack.Licensing.RegisterLicense (PrivateKeys.ServiceStackKey);
		}
	}
}