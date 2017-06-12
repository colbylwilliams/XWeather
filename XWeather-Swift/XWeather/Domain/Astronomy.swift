//
//  Astronomy.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class Astronomy : WuObject
{
	public override var WuKey: String { return "astronomy" }
	
	public var moon_phase: MoonPhase?
	public var sun_phase: AstronomyPhase?
}


public class MoonPhase : AstronomyPhase
{
	public var percentIlluminated: String?
	public var ageOfMoon: String?
	public var phaseofMoon: String?
	public var hemisphere: String?
	public var current_time: AstronomyTime?
	public var moonrise: AstronomyTime?
	public var moonset: AstronomyTime?
}


public class AstronomyPhase
{
	public var sunrise: AstronomyTime?
	public var sunset: AstronomyTime?
}


public class AstronomyTime
{
	public var hour: Double?
	public var minute: Double?
	
	public var localDateTime: Date {
		
		let todayStart = Calendar.current.startOfDay(for: Date())
		
		if let hours = self.hour, let minutes = self.minute {
			
			let seconds = (hours * 3600) + (minutes * 60)
			
			return todayStart.addingTimeInterval(seconds)
		}
		
		return todayStart
	}
}
