//
//  WuObject.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class WuObject
{
	required public init() {}
	
	public var WuKey: String { return "" }
	
	public var response: WuObjectDetail?
}


public class WuObjectDetail
{
	public var version: String?
	public var termsofService: String?
	public var features: WuFeatures?
}


public class WuFeatures
{
	public var almanac: Int?
	public var alerts: Int?
	public var astronomy: Int?
	public var conditions: Int?
	public var currenthurricane: Int?
	public var forecast: Int?
	public var forecast10day: Int?
	public var geolookup: Int?
	public var history: Int?
	public var hourly: Int?
	public var hourly10day: Int?
	public var planner: Int?
	public var rawtide: Int?
	public var satellite: Int?
	public var tide: Int?
	public var webcams: Int?
	public var yesterday: Int?
}


public class WuTxtDate
{
	public var pretty: String?
	public var year: String?
	public var mon: String?
	public var mday: String?
	public var hour: String?
	public var min: String?
	public var tzname: String?
	public var epoch: String?
}


public class WuDate
{
	public var epoch: String?
	public var pretty: String?
	public var day: Int?
	public var month: Int?
	public var year: Int?
	public var yday: Int?
	public var hour: Int?
	public var min: String?
	public var sec: Int?
	public var isdst: String?
	public var monthname: String?
	public var monthname_short: String?
	public var weekday_short: String?
	public var weekday: String?
	public var ampm: String?
	public var tz_short: String?
	public var tz_long: String?
}


public class FCTTIME
{
	public var hour: Int?
	public var hour_padded: String?
	public var min: Int?
	public var min_unpadded: String?
	public var sec: Int?
	public var year: Int?
	public var mon: Int?
	public var mon_padded: String?
	public var mon_abbrev: String?
	public var mday: Int?
	public var mday_padded: String?
	public var yday: Int?
	public var isdst: String?
	public var epoch: Double?
	public var pretty: String?
	public var civil: String?
	public var month_name: String?
	public var month_name_abbrev: String?
	public var weekday_name: String?
	public var weekday_name_night: String?
	public var weekday_name_abbrev: String?
	public var weekday_name_unlang: String?
	public var weekday_name_night_unlang: String?
	public var ampm: String?
	public var tz: String?
	public var age: String?
	public var UTCDATE: String?
}


public class WuImage
{
	public var url: String?
	public var title: String?
	public var link: String?
}


public class GeoLocation
{
	public var city: String?
	public var state: String?
	public var country: String?
	public var country_iso3166: String?
	
	public var lat: Double?
	public var lon: Double?
	
	public var latitude: Double?
	public var longitude: Double?
	
	public var LatitudeValue: Double? { return lat.getBestValue(latitude) }
	public var LongitudeValue: Double? { return lon.getBestValue(longitude)	}
}


public class Temperature
{
	public var fahrenheit: Double?
	public var celsius: Double?
	
	public var F: Double?
	public var C: Double?
	
	public var FahrenheitValue: Double? { return fahrenheit.getBestValue(F)	}
	public var CelsiusValue: Double? { return celsius.getBestValue(C) }
}


public class Measurement
{
	public var english: Double?
	public var metric: Double?
}


public class Precipitation
{
	public var `in`: Double?
	public var cm: Double?
	public var mm: Int?
}


public class Wind
{
	public var mph: Int?
	public var kph: Int?
	public var dir: String?
	public var degrees: Int?
}
