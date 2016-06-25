using System.Text.RegularExpressions;

namespace XWeather.Domain
{
	public static class WuAcExtensions
	{
		public static string [] GetCityStateArray (this WuAcLocation location)
			=> string.IsNullOrEmpty (location.name) ? null : Regex.Split (location.name, @"\s*,\s*");


		public static string GetZip (this WuAcLocation location)
			=> string.IsNullOrEmpty (location.zmw) ? string.Empty : location.zmw.Substring (0, location.zmw.IndexOf ('.'));
	}
}