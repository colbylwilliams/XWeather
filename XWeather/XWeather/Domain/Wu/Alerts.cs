using System.Collections.Generic;

namespace XWeather
{
	public class Alerts : WuObject
	{
		public override string WuApiKey => "alerts";

		public string query_zone { get; set; }
		public List<Alert> alerts { get; set; }
	}

	public class Alert
	{

	}
}