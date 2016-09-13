//
//  ViewExtensions.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import CoreGraphics
import Foundation
import UIKit

extension UIView {
	
	func setTransparencyMask(top: CGFloat, bottom: CGFloat) {
		
		if top > self.frame.height || bottom > self.frame.height {
			
			self.layer.mask = self.visibilityMaskWithLocation(1)
		
		} else if top >= 0 && top <= self.frame.height {
			
			let location = top / self.frame.height
			
			self.layer.mask = self.visibilityMaskWithLocation(location)
			
		} else if bottom >= 0 && bottom <= self.frame.height {
			
			let location = abs(1 - (bottom / self.frame.height))
			
			self.layer.mask = self.visibilityMaskWithLocation(location, reverse: true)
			
		} else {
			
			self.layer.mask = nil
		}
		
		self.layer.masksToBounds = true
	}
	
	
	func visibilityMaskWithLocation(_ location: CGFloat, reverse: Bool = false) -> CAGradientLayer {
		
		let mask = CAGradientLayer()
		
		mask.frame = self.bounds
		
		if reverse {
			
			mask.colors = [ UIColor(white: 1, alpha: 1).cgColor, UIColor(white: 1, alpha: 0).cgColor ]
		
		} else {
		
			mask.colors = [ UIColor(white: 1, alpha: 0).cgColor, UIColor(white: 1, alpha: 1).cgColor ]
		}
		
		mask.locations = [ NSNumber(value: location.native), NSNumber(value: location.native) ]
		
		return mask
	}
}


extension UITableViewHeaderFooterView {
	
	func setBackgroundTransparent(labelColor: UIColor) {
		
		self.contentView.backgroundColor = UIColor.clear
		
		self.backgroundView?.backgroundColor = UIColor.clear
		
		self.textLabel?.textColor = labelColor
	}
}
