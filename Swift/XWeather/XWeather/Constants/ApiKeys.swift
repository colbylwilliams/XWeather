//
//  ApiKeys.swift
//  XWeather
//
//  Created by Colby Williams on 9/14/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation

struct ApiKeys {
	
	static let empty = ""
	
	static let jsonExt = ".json"
	
	static let AzureWebsite = "https://xweather.azurewebsites.net"
	
	static let WuApiBasePath = "http://api.wunderground.com/api"
	
	static let WuApiKeyedPath = "\(WuApiBasePath)/\(PrivateKeys.wuApiKey)"
	
	static func WuApiKeyedQueryFmt (_ s0: String, _ s1: String, jsonExtention: Bool = false) -> String {
		
		return "\(WuApiKeyedPath)/\(s0)/\(s1)\(jsonExtention ? jsonExt : empty)"
	}
	
	static func WuAcQueryFmt (_ s0: String) -> String {
		
		return "http://autocomplete.wunderground.com/aq?query=\(s0)"
	}
}
