//
//  Forecast.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

public class Forecast : WuObject {

	public override var WuKey: String { get { return "forecast" } }

	public var forecast: ForecastDetail?

	public var TextForecast: TxtForecast?
	public var Simpleforecast: SimpleForecast?
}


public class ForecastDetail {

	public var txt_forecast: TxtForecast?
	public var simpleforecast: SimpleForecast?
}


public class TxtForecastDay {

	public var period: Int?
	public var icon: String?
	public var icon_url: String?
	public var title: String?
	public var fcttext: String?
	public var fcttext_metric: String?
	public var pop: String?
}


public class ForecastDay {

	public var date: WuDate?
	public var period: Int?
	public var high: Temperature?
	public var low: Temperature?
	public var conditions: String?
	public var icon: String?
	public var icon_url: String?
	public var skyicon: String?
	public var pop: Int?
	public var qpf_allday: Precipitation?
	public var qpf_day: Precipitation?
	public var qpf_night: Precipitation?
	public var snow_allday: Precipitation?
	public var snow_day: Precipitation?
	public var snow_night: Precipitation?
	public var maxwind: Wind?
	public var avewind: Wind?
	public var avehumidity: Int?
	public var maxhumidity: Int?
	public var minhumidity: Int?
}


public class TxtForecast {

	public var date: String?
	public var forecastday: [TxtForecastDay]?
}


public class SimpleForecast {

	public var forecastday: [ForecastDay]?
}
