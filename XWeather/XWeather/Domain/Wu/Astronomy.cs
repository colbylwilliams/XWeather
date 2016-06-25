namespace XWeather
{
	public class Astronomy : WuObject
	{
		public override string WuApiKey => "astronomy";

		public MoonPhase moon_phase { get; set; }
		public AstronomyPhase sun_phase { get; set; }
	}


	public class MoonPhase : AstronomyPhase
	{
		public string percentIlluminated { get; set; }
		public string ageOfMoon { get; set; }
		public string phaseofMoon { get; set; }
		public string hemisphere { get; set; }
		public AstronomyTime current_time { get; set; }
		public AstronomyTime moonrise { get; set; }
		public AstronomyTime moonset { get; set; }
	}


	public class AstronomyPhase
	{
		public AstronomyTime sunrise { get; set; }
		public AstronomyTime sunset { get; set; }
	}


	public class AstronomyTime
	{
		public string hour { get; set; }
		public string minute { get; set; }
	}
}