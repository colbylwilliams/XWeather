using System.Collections.Generic;

namespace XWeather.Domain
{
	public class WuAcResults
	{
		public List<WuAcLocation> Results { get; set; }
	}

	public class WuAcLocation
	{
		public string name { get; set; }
		public string type { get; set; }
		public string c { get; set; }
		public string zmw { get; set; }
		public string tz { get; set; }
		public string tzs { get; set; }
		public string l { get; set; }
		public string ll { get; set; }
		public double lat { get; set; }
		public double lon { get; set; }

		public bool Current { get; set; }
		public bool Selected { get; set; }
	}
}