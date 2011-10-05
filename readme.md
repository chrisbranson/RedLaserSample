# RedLaser Sample

A MonoTouch implementation of the RedLaser SDK sample application (http://www.redlaser.com/SDK.aspx).

### Requirements

* iOS 4 or higher
* RedLaser SDK v3.2.0
* MonoTouch v4.0.4.1
* MonoDevelop 2.6

### Notes

This project uses the RedLaser MonoTouch bindings (https://github.com/chrisbranson/monotouch-bindings/tree/master/RedLaser), a pre-compiled dll is found in the zip archive under Downloads (https://github.com/chrisbranson/RedLaserSample/archives/master). Alternatively you can compile your own dll by fetching the source and running 'make'.

The solution includes the necessary additional compiler commands and frameworks. The app will run on the simulator, however as the RedLaser SDK requires access to camera hardware which is obviously not present it serves little purpose.

As the RedLaser SDK makes use of certain c++ libraries the **g++** compiler/linker must be used instead of **gcc**. The '-cxx' option is specified in the build options to enable this.

##

Chris Branson, 5th October 2011
https://github.com/chrisbranson