//
//  NumberExtensions.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

extension Optional where Wrapped == Double {
	
	var isMinZeroNaNorBs: Bool {
		if let this = self {
			return this.isEqual(to: 0) || this.isNaN || this.isEqual(to: -9999) // -9999 is a BS value Weather Undergroud sometimes returns
		}
		return true;
	}
	
	func getBestValue (_ other: Double?) -> Double {
		return self.isMinZeroNaNorBs ? other.isMinZeroNaNorBs ? 0 : other! : self!
	}
}
