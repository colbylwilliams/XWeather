using System;
using CoreGraphics;

namespace XWeather.Unified
{
	public static class ColorExtensions
	{
		public static Tuple<CGColor [], int> GetTimeOfDayGradient (this WuLocation location, bool random = false)
		{
			var index = location.GetTimeOfDayIndex (random);

			return new Tuple<CGColor [], int> (Colors.Gradients [index], index);
		}
	}
}