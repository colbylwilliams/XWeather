//
//  Hourly.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class Hourly : WuObject
{
	public override var WuKey: String { return "hourly" }
	
	public var hourly_forecast: [HourlyForecast] = []
}

public class HourlyForecast
{
	public var FCTTIME: FCTTIME?
	public var temp: Measurement?
	public var dewpoint: Measurement?
	public var condition: String?
	public var icon: String?
	public var icon_url: String?
	public var fctcode: String?
	public var sky: String?
	public var wspd: Measurement?
	public var wdir: Measurement?
	public var wx: String?
	public var uvi: String?
	public var humidity: String?
	public var windchill: Measurement?
	public var heatindex: Measurement?
	public var feelslike: Measurement?
	public var qpf: Measurement?
	public var snow: Measurement?
	public var pop: Double?
	public var mslp: Measurement?
}
