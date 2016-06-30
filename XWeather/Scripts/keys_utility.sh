#!/bin/bash

# c0lby:

projectName=
moveDirectory=

while getopts "p:m:" object; do
	case "${object}" in
		p) projectName="${OPTARG}" ;;
		m) moveDirectory="${OPTARG}" ;;
	esac
done
shift $((OPTIND-1))

privateKeys="$AGENT_HOMEDIRECTORY/../private/$projectName"

cp -r "$privateKeys/." "$moveDirectory"
