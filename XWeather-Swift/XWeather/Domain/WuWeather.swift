//
//  WuWeather.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class WuWeather : WuObject
{
	public override var WuKey: String { return "conditions/forecast10day/hourly10day/astronomy" }
	
	public var current_observation: CurrentObservation?
	
	public var forecast: ForecastDetail?
	
	public var textForecast: TxtForecast? { return forecast?.txt_forecast }
	
	public var simpleForecast: SimpleForecast? { return forecast?.simpleforecast }
	
	public var hourly_forecast: [HourlyForecast] = []
	
	public var moon_phase: MoonPhase?
	
	public var sun_phase: AstronomyPhase?
}
