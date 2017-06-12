//
//  ColorExtensions.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation
import CoreGraphics
import UIKit

extension WuLocation {
	
	func timeOfDayIndex (_ random: Bool = false) -> (index: Int, colors: [CGColor]) {
		
		if random {
			// the middle gradients are sexier
			let randomIndex = Int(arc4random_uniform(5)) + 2
			return (randomIndex, [UIColor(named: "gradientStart\(randomIndex)")!.cgColor, UIColor(named: "gradientStop\(randomIndex)")!.cgColor])
		}
		
		let maxIndex = 9
		
		if !hasSunTimes {
			// if there's no data, assume day
			return (maxIndex, [UIColor(named: "gradientStart\(maxIndex)")!.cgColor, UIColor(named: "gradientStop\(maxIndex)")!.cgColor])
		}
		
		let t:Double = 90 * 60
		let b:Double = 30 * 30
//		let f:Double = t + b
		
		var index: Int = 6;
		
		if let current = currentTime?.localDateTime, let rise = sunrise?.localDateTime, let set = sunset?.localDateTime {
		
			// start the sunrise t mins before
			let sunriseStart = rise.addingTimeInterval((0 - b))
			let sunriseEnd = rise.addingTimeInterval(t)
			
			
			// start the sunset t mins before
			let sunsetStart = set.addingTimeInterval((0-t))
			let sunsetEnd = set.addingTimeInterval(b)
			
			
			if (current.isOutside (sunriseStart, end: sunsetEnd)) {
				// night (before sunrise or after sunset)
				
				index = 0;
				
			} else if (current.isBetween (sunriseEnd, end: sunsetStart)) {
				// day (after sunrise and before sunset)
				
				index = maxIndex;
				
			} else if (current.isBetween (sunriseStart, end: sunriseEnd)) {
				// during sunrise
				
//				index = (int)Math.Floor ((current.Subtract (sunriseStart).TotalMinutes / f) * 10);
				
			} else if (current.isBetween (sunsetStart, end: sunsetEnd)) {
				// during sunset
				
//				index = (int)Math.Floor ((current.Subtract (sunsetStart).TotalMinutes / f) * 10);
				
				// the gradient array is ordered from dark to light, so reverse it for sunset
				index = maxIndex - index;
			}
		}
		
		return (index, [UIColor(named: "gradientStart\(index)")!.cgColor, UIColor(named: "gradientStop\(index)")!.cgColor])
	}
}
