#!/bin/bash

# c0lby:

BUILD_SOURCESDIRECTORY="/Users/colbywilliams/GitHub/XWeather"

mkdir tmp

# queryUrlBase="http://api.wunderground.com/api/$1/conditions/forecast10day/hourly/astronomy/q/"
queryUrlBase="http://api.wunderground.com/api/$1/conditions/forecast10day/hourly10day/astronomy/q/"

# queryArray=("$@")

queryArray=("FL/Miami" "GA/Atlanta" "LA/New_Orleans" "TX/Dallas" "CO/Denver" "NV/Las_Vegas" "CA/San_Francisco")
queryArray=$(printf ",%s" "${queryArray[@]}")
queryArray=${queryArray:1}


queryPath="$queryUrlBase{$queryArray}.json"


curl $queryPath -o "tmp/#1.json" --create-dirs


echo "Copying files to: $BUILD_SOURCESDIRECTORY/XWeather/iOS/Resources"

find ./tmp -type f -name "*.json" -exec cp {} "$BUILD_SOURCESDIRECTORY/XWeather/iOS/Resources" \;


echo "Copying files to: $BUILD_SOURCESDIRECTORY/XWeather/Droid/Assets"

find ./tmp -type f -name "*.json" -exec cp {} "$BUILD_SOURCESDIRECTORY/XWeather/Droid/Assets" \;


rm -rf ./tmp
