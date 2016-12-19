using System;
namespace XWeather.Domain
{
	public class Astronomy : WuObject
	{
		public override string WuKey => "astronomy";

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
		public double hour { get; set; }
		public double minute { get; set; }

		public DateTime LocalDateTime => DateTime.Today.AddHours (hour).AddMinutes (minute);
	}
}