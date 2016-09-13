//
//  LocationSearchTvc.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation
import UIKit

class LocationSearchTvc : BaseTvc {
	
	@IBOutlet weak var emptyView: UIVisualEffectView!
	
	
	override var cellId: String? {
		get {
			return "LocationSearchTvCell"
		}
	}

	@IBAction func emptyViewClicked(_ sender: AnyObject) {
		
	}
}
