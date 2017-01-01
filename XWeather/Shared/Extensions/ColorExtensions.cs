using System;

namespace XWeather
{
	public static class ColorExtensions
	{
		public static int GetTimeOfDayIndex (this WuLocation location, bool random = false)
		{
			var maxIndex = 9;

			// the middle gradients are sexier
			if (random) return new Random ().Next (5) + 2;


			// if there's no data, assume day
			if (!location.HasSunTimes) return maxIndex;


			const double t = 90;
			const double b = 30;
			const double f = t + b;

			var current = location.CurrentTime.LocalDateTime;

			var sunrise = location.Sunrise.LocalDateTime;

			var sunset = location.Sunset.LocalDateTime;


			// start the sunrise t mins before
			var sunriseStart = sunrise.Subtract (TimeSpan.FromMinutes (b));
			var sunriseEnd = sunrise.AddMinutes (t);


			// start the sunset t mins before
			var sunsetStart = sunset.Subtract (TimeSpan.FromMinutes (t));
			var sunsetEnd = sunset.AddMinutes (b);


			int index = 0;

			if (current.IsOutside (sunriseStart, sunsetEnd)) {
				// night (before sunrise or after sunset)

				index = 0;

			} else if (current.IsBetween (sunriseEnd, sunsetStart)) {
				// day (after sunrise and before sunset)

				index = maxIndex;

			} else if (current.IsBetween (sunriseStart, sunriseEnd)) {
				// during sunrise

				index = (int)Math.Floor ((current.Subtract (sunriseStart).TotalMinutes / f) * 10);

			} else if (current.IsBetween (sunsetStart, sunsetEnd)) {
				// during sunset

				index = (int)Math.Floor ((current.Subtract (sunsetStart).TotalMinutes / f) * 10);

				// the gradient array is ordered from dark to light, so reverse it for sunset
				index = maxIndex - index;
			}

			return index;
		}
	}
}