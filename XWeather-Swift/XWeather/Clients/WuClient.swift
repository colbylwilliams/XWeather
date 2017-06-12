//
//  WuClient.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class WuClient
{
	public static let Shared: WuClient = {
		let instance = WuClient()
		// setup code
		return instance
	}()
	
	public var locations: [WuLocation] = []
	
	public var selected: WuLocation? {
		willSet(newSelected) {
			for location in locations {
				location.selected = false
			}
		}
		didSet{
			selected?.selected = true
			// UpdatedSelected?.Invoke (this, EventArgs.Empty);
			// Settings.locationsJson = locations.getLocationsJson ();
		}
	}
	
	public var hasCurrent: Bool { return selected != nil }
	
	
}
