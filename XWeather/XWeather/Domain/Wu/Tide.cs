using System.Collections.Generic;

namespace XWeather.Domain
{
	public class Tide : WuObject
	{
		public override string WuKey => "tide";

		public TideDetail tide { get; set; }

		public List<TideInfo> TideInfo => tide?.tideInfo;
		public List<TideSummary> TideSummary => tide?.tideSummary;
		public List<TideStat> TideSummaryStats => tide?.tideSummaryStats;
	}


	public class TideDetail
	{
		public List<TideInfo> tideInfo { get; set; }
		public List<TideSummary> tideSummary { get; set; }
		public List<TideStat> tideSummaryStats { get; set; }
	}


	public class TideInfo
	{
		public string tideSite { get; set; }
		public string lat { get; set; }
		public string lon { get; set; }
		public string units { get; set; }
		public string type { get; set; }
		public string tzname { get; set; }
	}


	public class TideData
	{
		public string height { get; set; }
		public string type { get; set; }
	}


	public class TideSummary
	{
		public WuTxtDate date { get; set; }
		public WuTxtDate utcdate { get; set; }
		public TideData data { get; set; }
	}


	public class TideStat
	{
		public double maxheight { get; set; }
		public double minheight { get; set; }
	}
}