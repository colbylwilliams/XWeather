//
//  WuLocation.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

public class WuLocation : Equatable //: IComparable<WuLocation>//, IEquatable<WuLocation>
{
	public static func ==(lhs: WuLocation, rhs: WuLocation) -> Bool {
		return lhs.Name == rhs.Name
	}
	
	
	init (location: WuAcLocation)
	{
		Location = location;
	}


	init (location: WuAcLocation, weather: WuWeather)
	{
		Location = location;
		Weather = weather;
		Updated = Date()
	}


	public var Updated: Date?

	public var Weather: WuWeather?

	public var Location: WuAcLocation!


	public var Current: Bool { get { return Location.Current; } set { Location.Current = newValue; } }

	public var Selected: Bool { get { return Location.Selected; } set { Location.Selected = newValue; } }

	public var Name: String? { get { return Weather?.current_observation?.display_location?.city ?? Location?.name } }

	public var HasSunTimes: Bool { get { return CurrentTime != nil && Sunset != nil && Sunrise != nil } }


	public var Conditions: CurrentObservation? { get { return Weather?.current_observation } }

	public var Forecasts: [ForecastDay] { get { return Weather?.Simpleforecast?.forecastday ?? [] } }

	public var TodayForecast: ForecastDay? { get { return Weather?.Simpleforecast?.forecastday?.first } }

	public var TxtForecasts: [TxtForecastDay] { get { return Weather?.TextForecast?.forecastday ?? [] } }

	public var HourlyForecasts: [HourlyForecast] { get { return Weather?.hourly_forecast ?? [] } }


	var hourlyForecastCache: [Int: [HourlyForecast]] = [:]
	
	public func HourlyForecast(day: Int, nextDay: Bool = true) -> [HourlyForecast] {
		
		if let cachedForecast = hourlyForecastCache[day] {
		
			return cachedForecast
		
		} else {
			
			hourlyForecastCache [day] = Weather?.hourly_forecast?.filter { $0.FCTTIME?.mday == day || (nextDay && $0.FCTTIME?.mday == day + 1) } ?? []
			
			return hourlyForecastCache [day]!
		}
	}
	
	public var CurrentTime: AstronomyTime? { get { return Weather?.moon_phase?.current_time } }

	public var Sunset: AstronomyTime? { get { return Weather?.moon_phase?.sunset ?? Weather?.sun_phase?.sunset } }

	public var Sunrise: AstronomyTime? { get { return Weather?.moon_phase?.sunrise ?? Weather?.sun_phase?.sunrise } }


//	public int CompareTo (WuLocation other)
//	{
//		if (Current) return -1;
//
//		if (other.Current) return 1;
//
//		return Name.CompareTo (other.Name);
//	}
}
