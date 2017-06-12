//
//  Conditions.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class Conditions : WuObject
{
	public override var WuKey: String { return "conditions" }
	
	public var current_observation: CurrentObservation?
}


public class DisplayLocation : ObservationLocation
{
	public var state_name: String?
	public var zip: String?
	public var magic: String?
	public var wmo: String?
}


public class ObservationLocation : GeoLocation
{
	public var full: String?
	public var elevation: String?
}


public class Estimated
{
}


public class CurrentObservation
{
	public var image: WuImage?
	public var display_location: DisplayLocation?
	public var observation_location: ObservationLocation?
	public var estimated: Estimated?
	public var station_id: String?
	public var observation_time: String?
	public var observation_time_rfc822: String?
	public var observation_epoch: String?
	public var local_time_rfc822: String?
	public var local_epoch: String?
	public var local_tz_short: String?
	public var local_tz_long: String?
	public var local_tz_offset: Double?
	public var weather: String?
	public var temperature_string: String?
	public var temp_f: Double?
	public var temp_c: Double?
	public var relative_humidity: String?
	public var wind_string: String?
	public var wind_dir: String?
	public var wind_degrees: Double?
	public var wind_mph: Double?
	public var wind_gust_mph: Double?
	public var wind_kph: Double?
	public var wind_gust_kph: Double?
	public var pressure_mb: Double?
	public var pressure_in: Double?
	public var pressure_trend: String?
	public var dewpoint_string: String?
	public var dewpoint_f: Double?
	public var dewpoint_c: Double?
	public var heat_index_string: String?
	public var heat_index_f: String?
	public var heat_index_c: String?
	public var windchill_string: String?
	public var windchill_f: String?
	public var windchill_c: String?
	public var feelslike_string: String?
	public var feelslike_f: Double?
	public var feelslike_c: Double?
	public var visibility_mi: Double?
	public var visibility_km: Double?
	public var solarradiation: String?
	public var UV: Double?
	public var precip_1hr_string: String?
	public var precip_1hr_in: Double?
	public var precip_1hr_metric: Double?
	public var precip_today_string: String?
	public var precip_today_in: Double?
	public var precip_today_metric: Double?
	public var icon: String?
	public var icon_url: String?
	public var forecast_url: String?
	public var history_url: String?
	public var ob_url: String?
	public var nowcast: String?
}
