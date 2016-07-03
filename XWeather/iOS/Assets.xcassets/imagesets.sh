#!/bin/bash

# c0lby:

for i in "$@"
do
	echo "$i.imageset"
	mkdir "$i.imageset"
	cp imagesets/Contents.json "$i.imageset/"
done