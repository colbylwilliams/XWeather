#!/bin/bash

projectDir=
# release=

while getopts "p:" o; do
	case "${o}" in
		p) projectDir="${OPTARG}" ;;
		# r) release="${OPTARG}" ;;
	esac
done
shift $((OPTIND-1))

buildNumber=$(/usr/bin/git rev-list HEAD --count 2>/dev/null || printf "0")

manifestXml="$projectDir/Properties/AndroidManifest.xml"

currentCode=`grep versionCode $manifestXml | sed 's/.*versionCode="//; s/".*//'`
currentName=`grep versionName $manifestXml | sed 's/.*versionName="//; s/".*//'`

versionCode=$buildNumber
versionName=$currentName

echo "Updating Android build information. New version code: $versionCode - New version name: $versionName";

sed -i '' 's/versionCode *= *"'$currentCode'"/versionCode="'$versionCode'"/; s/versionName *= *"[^"]*"/versionName="'$versionName'"/' $manifestXml

# IncrementVersionNumber ()
# {
# 	a=( ${versionNumber//./ } )                   # replace points, split into array
# 	((a[2]++))                                    # increment revision (or other part)
# 	versionNumber="${a[0]}.${a[1]}.${a[2]}"       # compose new version
# }
