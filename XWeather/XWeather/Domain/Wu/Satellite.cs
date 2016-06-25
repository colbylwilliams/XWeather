namespace XWeather.Domain
{
	public class Satellite : WuObject
	{
		public override string WuKey => "satellite";

		public SatelliteDetail satellite { get; set; }

		public string image_url => satellite?.image_url;
		public string image_url_ir4 => satellite?.image_url_ir4;
		public string image_url_vis => satellite?.image_url_vis;
	}

	public class SatelliteDetail
	{
		public string image_url { get; set; }
		public string image_url_ir4 { get; set; }
		public string image_url_vis { get; set; }
	}
}