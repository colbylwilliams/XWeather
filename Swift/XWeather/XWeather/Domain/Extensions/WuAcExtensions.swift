//
//  WuAcExtensions.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

extension WuAcLocation {
	
}

extension GeoLookup {
	
	func toWuAcLocation (current: Bool = true) -> WuAcLocation? {
		
		if let location = self.location {
			
			let wuAcLocation = WuAcLocation()
			wuAcLocation.name = "\(location.city), \(location.state)"
			wuAcLocation.type = location.type
			wuAcLocation.c = location.country
			wuAcLocation.zmw = "\(location.zip).\(location.magic).\(location.wmo)"
			wuAcLocation.tz = location.tz_long
			wuAcLocation.tzs = location.tz_short
			wuAcLocation.l = location.l
			wuAcLocation.ll = "\(location.lat), \(location.lon)"
			wuAcLocation.lat = location.lat
			wuAcLocation.lon = location.lon
			wuAcLocation.Current = current
			
			return wuAcLocation
		}
		
		return nil
	}
}
