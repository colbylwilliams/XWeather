//
//  SettingsKeys.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import Foundation

struct SettingsKeys {
	// Visible Settings
	static let versionNumber = "VersionNumber";
	static let buildNumber = "BuildNumber";
	static let gitCommitHash = "GitCommitHash";
	static let userReferenceKey = "UserReferenceKey";
	static let randomBackgrounds = "RandomBackgrounds";
	static let uomTemperature = "UomTemperature";
	static let uomDistance = "UomDistance";
	static let uomPressure = "UomPressure";
	static let uomLength = "UomLength";
	static let uomSpeed = "UomSpeed";
	
	// Hidden Settings
	static let didSetUomDefaults = "DidSetUomDefaults";
	static let locationsJson = "LocationsJson";
	static let weatherPage = "WeatherPage";
	static let highLowGraph = "HighLowGraph";
}
