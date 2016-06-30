#!/bin/bash

# c0lby:

touch release-notes.md

# Get the last Git Commit Message and set to a variable
/usr/bin/git log -1 --pretty=%B >> release-notes.md
