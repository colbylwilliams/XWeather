//
//  GeoLookup.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

public class GeoLookup : WuObject {

	public override var WuKey: String { get { return "geolookup" } }

	public var location: GeoLookupLocation?
}


public class WeatherStation : GeoLocation {

	public var icao: String?
}


public class PersonalWeatherStation : WeatherStation {

	public var neighborhood: String?
	public var id: String?
	public var distance_km: Int?
	public var distance_mi: Int?
}


public class NearbyWeatherStations {

	public var airport: Airport?
	public var pws: Pws?
}


public class Airport {

	public var station:[WeatherStation]?
}


public class Pws {

	public var station:[PersonalWeatherStation]?
}


public class GeoLookupLocation : GeoLocation {

	public var type: String?
	public var country_name: String?
	public var tz_short: String?
	public var tz_long: String?
	public var zip: String?
	public var magic: String?
	public var wmo: String?
	public var l: String?
	public var requesturl: String?
	public var wuiurl: String?
	public var nearby_weather_stations: NearbyWeatherStations?
}
