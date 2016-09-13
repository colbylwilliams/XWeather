//
//  LocationTvc.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation
import UIKit

class LocationTvc : BaseTvc {
	
	@IBOutlet weak var tableHeader: UIView!
	
	override var cellId: String? {
		get {
			return "LocationTvCell"
		}
	}

	
	@IBAction func addButtonClicked(_ sender: AnyObject) {
		
	}

}
