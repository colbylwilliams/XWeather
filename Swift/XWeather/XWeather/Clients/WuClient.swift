//
//  WuClient.swift
//  XWeather
//
//  Created by Colby Williams on 9/14/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

class WuClient {
	
	public class var shared : WuClient {
		
		struct Static {
			static let instance: WuClient = WuClient()
		}
		
		return Static.instance
	}
	
	
	var selected: WuLocation? {
		willSet(newSelected) {
			
			locations.forEach { $0.Selected = false }
			
			newSelected?.Selected = true
		}
		didSet {
			
			
		}
	}

	
	var locations: [WuLocation] = [] {
		didSet {
			locations.sort(by: locationSort)
		}
	}
	
	
	var hasCurrent: Bool { get { return selected != nil } }
	
	
	func addLocation(_ location: WuAcLocation, callback:() -> ()) {
		
		getWuLocation(location) { result in
		
			locations.append(result)
			
			callback()
		}
	}
	
	
	func removeLocation(_ location: WuLocation, callback:() -> ()) {
		
		if let index = locations.index(where: { $0 == location }) {
		
			locations.remove(at: index)
			
			callback()
		}
	}

	
	func getCurrentLocation(_ coordnates: LocationCoordinates, callback:(WuAcLocation?) -> ()) {
		
		getData("/q/\(coordnates.Latitude),\(coordnates.Longitude)") { result in
			
			if let geoLookup = result as? GeoLookup {
				
				callback(geoLookup.toWuAcLocation())
			}
			
			callback(nil)
		}
	}

	
	func getLocations(json: String, coordnates: LocationCoordinates, callback:() -> ()) {
		
	}

	
	func getLocations(locations: [WuAcLocation], callback:() -> ()) {
		
	}

	
	func getWuLocation(_ acLocation: WuAcLocation, callback:(WuLocation) -> ()) {
		
	}

	
	func getData<T: WuObject>(_ location: String, callback:(T) -> ()) {
		
	}


	func getRadarImage(_ bounds: RadarBounds, callback:(Data) -> ()) {
		
	}

	
	func locationSort(_ l1: WuLocation, _ l2: WuLocation) -> Bool {
		
		if l1.Current {
			return true
		}
		
		if l2.Current {
			return false
		}
		
		if l1.Name == nil {
			return false
		}
		
		if l2.Name == nil {
			return true
		}
		
		return  l1.Name! < l2.Name!
	}
}
