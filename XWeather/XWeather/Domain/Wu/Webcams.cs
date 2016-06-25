using System.Collections.Generic;

namespace XWeather.Domain
{
	public class Webcams : WuObject
	{
		public override string WuKey => "webcams";

		public List<Webcam> webcams { get; set; }
	}

	public class Webcam
	{
		public string handle { get; set; }
		public string camid { get; set; }
		public string camindex { get; set; }
		public string assoc_station_id { get; set; }
		public string link { get; set; }
		public string linktext { get; set; }
		public string cameratype { get; set; }
		public string organization { get; set; }
		public string neighborhood { get; set; }
		public string zip { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public string tzname { get; set; }
		public string lat { get; set; }
		public string lon { get; set; }
		public string updated { get; set; }
		public string updated_epoch { get; set; }
		public string downloaded { get; set; }
		public string isrecent { get; set; }
		public string CURRENTIMAGEURL { get; set; }
		public string WIDGETCURRENTIMAGEURL { get; set; }
		public string CAMURL { get; set; }
	}
}