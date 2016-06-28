using System.Collections.Generic;

namespace XWeather
{
	public static class CollectionExtensions
	{
		public static bool IsEmpty<T> (this List<T> list) => list == null || list.Count <= 0;
	}
}