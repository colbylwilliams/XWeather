namespace XWeather.Domain
{
	public class Conditions : WuObject
	{
		public override string WuKey => "conditions";

		public CurrentObservation current_observation { get; set; }
	}


	public class DisplayLocation : ObservationLocation
	{
		public string state_name { get; set; }
		public string zip { get; set; }
		public string magic { get; set; }
		public string wmo { get; set; }
	}


	public class ObservationLocation : GeoLocation
	{
		public string full { get; set; }
		public string elevation { get; set; }
	}


	public class Estimated
	{
	}


	public class CurrentObservation
	{
		public WuImage image { get; set; }
		public DisplayLocation display_location { get; set; }
		public ObservationLocation observation_location { get; set; }
		public Estimated estimated { get; set; }
		public string station_id { get; set; }
		public string observation_time { get; set; }
		public string observation_time_rfc822 { get; set; }
		public string observation_epoch { get; set; }
		public string local_time_rfc822 { get; set; }
		public string local_epoch { get; set; }
		public string local_tz_short { get; set; }
		public string local_tz_long { get; set; }
		public double local_tz_offset { get; set; }
		public string weather { get; set; }
		public string temperature_string { get; set; }
		public double temp_f { get; set; }
		public double temp_c { get; set; }
		public string relative_humidity { get; set; }
		public string wind_string { get; set; }
		public string wind_dir { get; set; }
		public double wind_degrees { get; set; }
		public double wind_mph { get; set; }
		public double wind_gust_mph { get; set; }
		public double wind_kph { get; set; }
		public double wind_gust_kph { get; set; }
		public double pressure_mb { get; set; }
		public double pressure_in { get; set; }
		public string pressure_trend { get; set; }
		public string dewpoint_string { get; set; }
		public double dewpoint_f { get; set; }
		public double dewpoint_c { get; set; }
		public string heat_index_string { get; set; }
		public string heat_index_f { get; set; }
		public string heat_index_c { get; set; }
		public string windchill_string { get; set; }
		public string windchill_f { get; set; }
		public string windchill_c { get; set; }
		public string feelslike_string { get; set; }
		public double feelslike_f { get; set; }
		public double feelslike_c { get; set; }
		public double visibility_mi { get; set; }
		public double visibility_km { get; set; }
		public string solarradiation { get; set; }
		public double UV { get; set; }
		public string precip_1hr_string { get; set; }
		public double precip_1hr_in { get; set; }
		public double precip_1hr_metric { get; set; }
		public string precip_today_string { get; set; }
		public double precip_today_in { get; set; }
		public double precip_today_metric { get; set; }
		public string icon { get; set; }
		public string icon_url { get; set; }
		public string forecast_url { get; set; }
		public string history_url { get; set; }
		public string ob_url { get; set; }
		public string nowcast { get; set; }
	}
}