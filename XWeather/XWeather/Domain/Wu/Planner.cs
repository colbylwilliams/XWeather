namespace XWeather.Domain
{
	public class Planner : WuObject
	{
		public override string WuKey => "planner";

		public Trip trip { get; set; }
	}


	public class PlannerDate
	{
		public WuDate date { get; set; }
	}


	public class PeriodOfRecord
	{
		public PlannerDate date_start { get; set; }
		public PlannerDate date_end { get; set; }
	}


	public class TemperatureStats
	{
		public Temperature min { get; set; }
		public Temperature avg { get; set; }
		public Temperature max { get; set; }
	}


	public class PrecipitationStats
	{
		public Precipitation min { get; set; }
		public Precipitation avg { get; set; }
		public Precipitation max { get; set; }
	}


	public class CloudCover
	{
		public string cond { get; set; }
	}


	public class PlannerPrediction
	{
		public string name { get; set; }
		public string description { get; set; }
		public string percentage { get; set; }
	}


	public class ChanceOf
	{
		public PlannerPrediction tempoversixty { get; set; }
		public PlannerPrediction chanceofwindyday { get; set; }
		public PlannerPrediction chanceofpartlycloudyday { get; set; }
		public PlannerPrediction chanceofsunnycloudyday { get; set; }
		public PlannerPrediction chanceofcloudyday { get; set; }
		public PlannerPrediction chanceofhumidday { get; set; }
		public PlannerPrediction chanceoffogday { get; set; }
		public PlannerPrediction chanceofprecip { get; set; }
		public PlannerPrediction chanceofrainday { get; set; }
		public PlannerPrediction chanceofthunderday { get; set; }
		public PlannerPrediction tempoverninety { get; set; }
		public PlannerPrediction chanceofsnowonground { get; set; }
		public PlannerPrediction chanceoftornadoday { get; set; }
		public PlannerPrediction chanceofsultryday { get; set; }
		public PlannerPrediction tempbelowfreezing { get; set; }
		public PlannerPrediction tempoverfreezing { get; set; }
		public PlannerPrediction chanceofhailday { get; set; }
		public PlannerPrediction chanceofsnowday { get; set; }
	}


	public class Trip
	{
		public string title { get; set; }
		public string airport_code { get; set; }
		public string error { get; set; }
		public PeriodOfRecord period_of_record { get; set; }
		public TemperatureStats temp_high { get; set; }
		public TemperatureStats temp_low { get; set; }
		public PrecipitationStats precip { get; set; }
		public TemperatureStats dewpoint_high { get; set; }
		public TemperatureStats dewpoint_low { get; set; }
		public CloudCover cloud_cover { get; set; }
		public ChanceOf chance_of { get; set; }
	}
}