//
//  WuLocation.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class WuLocation
{
	required public init() {}
	
	public init (_ location: WuAcLocation) {
		self.location = location
	}
	
	public init (_ location: WuAcLocation, weather: WuWeather) {
		self.location = location
		self.weather = weather
		self.updated = Date()
	}
	
	
	public var updated: Date?
	
	public var weather: WuWeather?
	
	public var location: WuAcLocation?
	
	
	public var current: Bool { return location?.current ?? false }
	
	public var selected: Bool {
		get { return location?.current ?? false }
		set(newSelected) { location?.selected = newSelected }
	}
	
	
	public var name: String? { return weather?.current_observation?.display_location?.city ?? location?.name }
	
	public var hasSunTimes: Bool { return currentTime != nil && sunset != nil && sunrise != nil }
	
	
	public var conditions: CurrentObservation? { return weather?.current_observation }
	
	public var forecasts: [ForecastDay] { return weather?.simpleForecast?.forecastday ?? [] }
	
	public var todayForecast: ForecastDay? { return weather?.simpleForecast?.forecastday.first }
	
	public var textForecasts: [TxtForecastDay] { return weather?.textForecast?.forecastday ?? [] }
	
	public var hourlyForecasts: [HourlyForecast] { return weather?.hourly_forecast ?? [] }
	
	
	public var currentTime: AstronomyTime? { return weather?.moon_phase?.current_time }
	
	public var sunset: AstronomyTime? { return weather?.moon_phase?.sunset ?? weather?.sun_phase?.sunset }
	
	public var sunrise: AstronomyTime? { return weather?.moon_phase?.sunrise ?? weather?.sun_phase?.sunrise }
}
