//
//  Webcams.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

public class Webcams : WuObject {

	public override var WuKey: String { get { return "webcams" } }

	public var webcams: [Webcam]?
}

public class Webcam {

	public var handle: String?
	public var camid: String?
	public var camindex: String?
	public var assoc_station_id: String?
	public var link: String?
	public var linktext: String?
	public var cameratype: String?
	public var organization: String?
	public var neighborhood: String?
	public var zip: String?
	public var city: String?
	public var state: String?
	public var country: String?
	public var tzname: String?
	public var lat: String?
	public var lon: String?
	public var updated: String?
	public var updated_epoch: String?
	public var downloaded: String?
	public var isrecent: String?
	public var CURRENTIMAGEURL: String?
	public var WIDGETCURRENTIMAGEURL: String?
	public var CAMURL: String?
}
