//
//  DateExtensions.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

extension Date {
	
	func isBetween (_ start: Date, end: Date) -> Bool {
		return start < self && self < end
	}
	
	func isOutside (_ start: Date, end: Date) -> Bool {
		return start > self || self > end
	}
}
