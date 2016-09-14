//
//  NumberExtensions.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation


extension Double {
	
	static var min = DBL_MIN
	static var max = DBL_MAX
	
	var isMinZeroNaNorBs: Bool {
		
		let wuBsVal: Double = -9999
		let zeroVal: Double = 0
		
		return self.isEqual(to: zeroVal) || self.isEqual(to: Double.min) || self.isNaN || self.isEqual(to: wuBsVal)
	}
	
	static func GetBestValue (_ d1: Double?, _ d2: Double?) -> Double {
		
		return (d1 == nil || d1!.isMinZeroNaNorBs) ? ((d2 == nil || d2!.isMinZeroNaNorBs) ? 0 : d2!) : d1!
	}
}
