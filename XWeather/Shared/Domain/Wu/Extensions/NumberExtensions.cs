namespace XWeather
{
	public static class NumberExtensions
	{
		const double WuBsVal = -9999;

		public static bool IsMinZeroNaNorBs (this double d) => d.Equals (0) || d.Equals (double.MinValue) || double.IsNaN (d) || d.Equals (WuBsVal);

		public static double GetBestValue (double d1, double d2) => d1.IsMinZeroNaNorBs () ? (d2.IsMinZeroNaNorBs () ? 0 : d2) : d1;
	}
}