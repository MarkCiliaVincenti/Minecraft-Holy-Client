#!/bin/bash
# This is a dummy bash script used for demonstration and test. It outputs a few variables
# and creates a dummy file in the application directory which will be detected by the program.

echo
echo "==========================="
echo "POST_PUBLISH BASH SCRIPT"
echo "==========================="
echo

# Some useful macros / environment variables
echo "BUILD_ARCH: ${BUILD_ARCH}"
echo "BUILD_TARGET: ${BUILD_TARGET}"
echo "BUILD_SHARE: ${BUILD_SHARE}"
echo "BUILD_APP_BIN: ${BUILD_APP_BIN}"
echo

# Directory and file will be detected by HelloWorld Program
echo "Do work..."
set -x #echo on
mkdir -p "${BUILD_APP_BIN}/subdir"
touch "${BUILD_APP_BIN}/subdir/file.test"
set +x #echo off

ls "/tmp/KuiperZone.PupNet/titlehhhh.holyclient-linux-x64-Release-Deb/AppDir/opt/titlehhhh.holyclient/"

echo
echo "==========================="
echo "POST_PUBLISH END"
echo "==========================="
echo