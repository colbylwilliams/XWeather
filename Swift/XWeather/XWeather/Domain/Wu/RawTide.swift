// //
// //  RawTide.swift
// //  XWeather
// //
// //  Created by Colby Williams on 9/13/16.
// //  Copyright © 2016 Colby Williams. All rights reserved.
// //

// import Foundation

// using System.Collections.Generic;

// namespace XWeather.Domain
// {
// 	public class RawTide : WuObject
// 	{
// 		public override string WuKey => "rawtide";

// 		public RawTideDetail rawtide { get; set; }

// 		public List<TideInfo> TideInfo => rawtide?.tideInfo;
// 		public List<RawTideOb> RawTideObs => rawtide?.rawTideObs;
// 		public List<TideStat> RawTideStats => rawtide?.rawTideStats;
// 	}


// 	public class RawTideDetail
// 	{
// 		public List<TideInfo> tideInfo { get; set; }
// 		public List<RawTideOb> rawTideObs { get; set; }
// 		public List<TideStat> rawTideStats { get; set; }
// 	}


// 	public class RawTideOb
// 	{
// 		public int epoch { get; set; }
// 		public double height { get; set; }
// 	}
// }