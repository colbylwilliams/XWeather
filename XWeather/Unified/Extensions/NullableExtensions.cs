using System;

namespace XWeather.Unified
{
	public static class NullableExtensions
	{
		public static int Int32Value (this nint? n) => Convert.ToInt32 (n.Value);
	}
}