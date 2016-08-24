using System;

namespace XWeather
{
	public class LocationProviderFactory
	{
		public static Func<ILocationProvider> Create { get; set; }
	}
}