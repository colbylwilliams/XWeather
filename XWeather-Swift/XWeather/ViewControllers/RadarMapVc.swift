//
//  RadarMapVc.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import UIKit
import MapKit

class RadarMapVc: UIViewController {

	@IBOutlet weak var mapView: MKMapView!
	@IBOutlet weak var closeVisualEffectView: UIVisualEffectView!
	@IBOutlet weak var closeButton: UIButton!
	
	
	
	
	override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
	@IBAction func closeClicked(_ sender: Any) {
	}
	
}
