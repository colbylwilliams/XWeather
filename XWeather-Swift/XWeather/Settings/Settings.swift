//
//  Settings.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

public class Settings {
	
	public func registerDefaultSettings() {
		
		if let path = Bundle.main.url(forResource: "Root", withExtension: "plist", subdirectory: "Settings.bundle"),
		   let settings = NSDictionary(contentsOf: path) as? [String: [[String: AnyObject]]],
		   let preferences = settings["PreferenceSpecifiers"] {
			
			var registrationDict = [String: AnyObject]()
			
			for preference in preferences {
				if let key = preference["Key"] as? String, let def = preference["DefaultValue"] {
					registrationDict[key] = def
				}
			}
			
			UserDefaults.standard.register(defaults: registrationDict)
		}
	}
	
	
	public func synchronize() {
		UserDefaults.standard.synchronize()
	}
	
	
	func setSetting (_ key: String, val: Any?) {
		UserDefaults.standard.set(val, forKey: key)
	}
	
	
	func stringForKey(_ key: String) -> String {
		return UserDefaults.standard.string(forKey: key) ?? ""
	}
	
	
	func boolForKey(_ key: String) -> Bool {
		return UserDefaults.standard.bool(forKey: key)
	}
	
	
	func intForKey(_ key: String) -> Int {
		return UserDefaults.standard.integer(forKey: key)
	}
	
	
	// Visible Settings
	
	var versionNumber: String {
		get { return stringForKey (SettingsKeys.versionNumber) }
	}
	
	var buildNumber: String {
		get { return stringForKey(SettingsKeys.buildNumber) }
	}
	
	var gitHash: String {
		get { return stringForKey(SettingsKeys.gitCommitHash) }
	}
	
	var userReferenceKey: String {
		get { return stringForKey (SettingsKeys.userReferenceKey) }
		set(newValue) { setSetting (SettingsKeys.userReferenceKey, val: newValue) }
	}
	
	
	var randomBackgrounds: Bool {
		get { return boolForKey (SettingsKeys.randomBackgrounds) }
		set(newValue) { setSetting (SettingsKeys.randomBackgrounds, val: newValue) }
	}
	
	
	var uomTemperature: TemperatureUnit {
		get { return TemperatureUnit(rawValue: intForKey (SettingsKeys.uomTemperature))! }
		set(newValue) { setSetting (SettingsKeys.uomTemperature, val: newValue.rawValue) }
	}
	
	
	var uomDistance: DistanceUnit {
		get { return DistanceUnit(rawValue: intForKey (SettingsKeys.uomDistance))! }
		set(newValue) { setSetting (SettingsKeys.uomDistance, val: newValue.rawValue) }
	}
	
	
	var uomPressure: PressureUnit {
		get { return PressureUnit(rawValue: intForKey (SettingsKeys.uomPressure))! }
		set(newValue) { setSetting (SettingsKeys.uomPressure, val: newValue.rawValue) }
	}
	
	
	var uomLength: LengthUnit {
		get { return LengthUnit(rawValue: intForKey (SettingsKeys.uomLength))! }
		set(newValue) { setSetting (SettingsKeys.uomLength, val: newValue.rawValue) }
	}
	
	
	var uomSpeed: SpeedUnit {
		get { return SpeedUnit(rawValue: intForKey (SettingsKeys.uomSpeed))! }
		set(newValue) { setSetting (SettingsKeys.uomSpeed, val: newValue.rawValue) }
	}
	
	
	// Hidden Settings
	
	var didSetUomDefaults: Bool {
		get { return boolForKey (SettingsKeys.didSetUomDefaults) }
		set(newValue) { setSetting (SettingsKeys.didSetUomDefaults, val: newValue) }
	}
	
	
	var locationsJson: String {
		get { return stringForKey (SettingsKeys.locationsJson) }
		set(newValue) { setSetting (SettingsKeys.locationsJson, val: newValue); }
	}
	
	
	var weatherPage: Int {
		get { return intForKey (SettingsKeys.weatherPage) }
		set(newValue) { setSetting (SettingsKeys.weatherPage, val: newValue) }
	}
	
	
	var highLowGraph: Bool {
		get { return boolForKey (SettingsKeys.highLowGraph) }
		set(newValue) { setSetting (SettingsKeys.highLowGraph, val: newValue) }
	}
	
}
