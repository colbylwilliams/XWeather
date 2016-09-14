//
//  WuAcResults.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

public class WuAcResults {

	public var Results: [WuAcLocation]?
}

public class WuAcLocation {

	public var name: String?
	public var type: String?
	public var c: String?
	public var zmw: String?
	public var tz: String?
	public var tzs: String?
	public var l: String?
	public var ll: String?
	public var lat: Double?
	public var lon: Double?

	public var Current: Bool = false
	public var Selected: Bool = false
}
