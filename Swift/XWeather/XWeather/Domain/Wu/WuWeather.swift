//
//  WuWeather.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

public class WuWeather : WuObject {

	//public override var WuKey: String { get { return "conditions/forecast10day/hourly10day/astronomy/tide/rawtide" } }
	//public override var WuKey: String { get { return "conditions/forecast10day/hourly/astronomy" } }
	public override var WuKey: String { get { return "conditions/forecast10day/hourly10day/astronomy" } }

	// Conditions
	public var current_observation: CurrentObservation?


	// ForecastTenDay
	public var forecast: ForecastDetail?

	public var TextForecast: TxtForecast? { get { return forecast?.txt_forecast } }
	public var Simpleforecast: SimpleForecast? { get { return forecast?.simpleforecast } }


	// Hourly / HourlyTenDay
	public var hourly_forecast: [HourlyForecast]?


	// Astronomy
	public var moon_phase: MoonPhase?
	public var sun_phase: AstronomyPhase?


	//// Tide
	//public TideDetail tide { get; set; }

	//public List<TideInfo> TideInfo => tide?.tideInfo;
	//public List<TideSummary> TideSummary => tide?.tideSummary;
	//public List<TideStat> TideSummaryStats => tide?.tideSummaryStats;

	//// RawTide
	//public RawTideDetail rawtide { get; set; }

	//public List<TideInfo> RawTideInfo => rawtide?.tideInfo;
	//public List<RawTideOb> RawTideObs => rawtide?.rawTideObs;
	//public List<TideStat> RawTideStats => rawtide?.rawTideStats;
}
