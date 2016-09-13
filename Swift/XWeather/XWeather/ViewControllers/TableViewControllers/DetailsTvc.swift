//
//  DetailsTvc.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

class DetailsTvc : BaseTvc {
	
	@IBOutlet var tableHeader: DetailsTvHeader!
	
	
	override var cellId: String? {
		get {
			return "DetailsTvCell"
		}
	}
}
