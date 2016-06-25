namespace XWeather.Domain
{
	public class Almanac : WuObject
	{
		public override string WuKey => "almanac";

		public AlmanacDetail almanac { get; set; }

		public string AirportCode => almanac?.airport_code;
		public TemperatureRecord RecordHigh => almanac?.temp_high;
		public TemperatureRecord RecordLow => almanac?.temp_low;
	}


	public class AlmanacDetail
	{
		public string airport_code { get; set; }
		public TemperatureRecord temp_high { get; set; }
		public TemperatureRecord temp_low { get; set; }
	}


	public class TemperatureRecord
	{
		public Temperature normal { get; set; }
		public Temperature record { get; set; }
		public string recordyear { get; set; }
	}
}