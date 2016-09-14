//
//  Astronomy.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

public class Astronomy : WuObject {

	public override var WuKey: String { get { return "astronomy" } }

	public var moon_phase: MoonPhase?
	public var sun_phase: AstronomyPhase?
}


public class MoonPhase : AstronomyPhase {

	public var percentIlluminated: String?
	public var ageOfMoon: String?
	public var phaseofMoon: String?
	public var hemisphere: String?
	public var current_time: AstronomyTime?
	public var moonrise: AstronomyTime?
	public var moonset: AstronomyTime?
}


public class AstronomyPhase {

	public var sunrise: AstronomyTime?
	public var sunset: AstronomyTime?
}


public class AstronomyTime {

	public var hour: Double?
	public var minute: Double?

	//public DateTime LocalDateTime => DateTime.Today.AddHours (hour).AddMinutes (minute);
}
