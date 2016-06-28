using System;
using System.Collections.Generic;

namespace XWeather.Unified
{
	public static class CollectionExtensions
	{
		public static T NIndexer<T> (this List<T> list, nint index) => list [Convert.ToInt32 (index)];

		public static nint NCount<T> (this List<T> list) => (nint)list.Count;
	}
}