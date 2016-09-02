using System;
using Android.Graphics;

namespace XWeather.Droid
{
	public static class PathExtensions
	{
		public static void LineTo (this Path path, PointF point) => path.LineTo (point.X, point.Y);

		public static void MoveTo (this Path path, PointF point) => path.MoveTo (point.X, point.Y);

		public static bool IsEmpty (this PointF point) => (point.X == 0 && point.Y == 0);
	}
}