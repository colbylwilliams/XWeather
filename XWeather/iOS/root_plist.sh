#!/bin/bash

# c0lby:

## Create New Root.plist file ##

PreparePreferenceFile

		AddNewTitleValuePreference  -k "VersionNumber" 	-d "$versionNumber ($buildNumber)" 	-t "Version"

		# AddNewTitleValuePreference  -k "GitCommitHash" 	-d "$gitCommitHash" -t "Git Hash"


	AddNewPreferenceGroup	-t "Units of Measurement"
		AddNewMultiValuePreference  -k "UomTemperature"   	-d 0 		-t "Temperature"
			SetMultiValuePreferenceValues  0 1
			SetMultiValuePreferenceTitles  "Fahrenheit" "Celsius"

		AddNewMultiValuePreference  -k "UomDistance"   	-d 0 		-t "Distance"
			SetMultiValuePreferenceValues  0 1
			SetMultiValuePreferenceTitles  "Miles" "Kilometers"

		AddNewMultiValuePreference  -k "UomPressure"   	-d 0 		-t "Pressure"
			SetMultiValuePreferenceValues  0 1
			SetMultiValuePreferenceTitles  "Inches of Mercury" "Millibars"

		AddNewMultiValuePreference  -k "UomLength"   	-d 0 		-t "Length"
			SetMultiValuePreferenceValues  0 1
			SetMultiValuePreferenceTitles  "Inches" "Millimeters"

		AddNewMultiValuePreference  -k "UomSpeed"   	-d 0 		-t "Speed"
			SetMultiValuePreferenceValues  0 1
			SetMultiValuePreferenceTitles  "Miles per hour" "Kilometers per hour"


	AddNewPreferenceGroup 	-t "User Interface"
		AddNewStringNode 	-e "FooterText" 	-v "Background colors represent each location’s time of day. Enable this option to use random pretty colors instead."

		AddNewToggleSwitchPreference -k "RandomBackgrounds" 	-d true 	-t "Random Backgrounds"


	AddNewPreferenceGroup 	-t "Diagnostics Key"
		AddNewStringNode 	-e "FooterText" 	-v "$copyright"


	AddNewTitleValuePreference  -k "UserReferenceKey" 	-d "anonymous"  	-t ""
