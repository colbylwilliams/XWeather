using System.Collections.Generic;

namespace XWeather.Domain
{
	public class Alerts : WuObject
	{
		public override string WuKey => "alerts";

		public string query_zone { get; set; }
		public List<Alert> alerts { get; set; }
	}

	public class Alert
	{

	}
}