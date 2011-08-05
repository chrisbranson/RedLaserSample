# RedLaser Sample

This MonoDevelop solution is a MonoTouch implementation of the RedLaser SDK sample application (http://www.redlaser.com/SDK.aspx).

### Requirements

* iOS 4 or higher
* RedLasker SDK v3.1.1
* MonoTouch v4.0.4.1
* MonoDevelop 2.6 beta 3

### Notes

This project uses the RedLaser MonoTouch bindings (https://github.com/chrisbranson/monotouch-bindings/tree/master/RedLaser), a pre-compiled dll is found in the zip archive under Downloads (https://github.com/chrisbranson/RedLaserSample/archives/master). Alternatively you can compile your own dll by fetching the source and running 'make'.

The solution includes the necessary additional compiler commands and frameworks and will compile only for the device as the RedLaser SDK requires access to camera hardware which is obviously not present on the simulator.

As the RedLaser SDK makes use of certain c++ libraries the **g++** compiler/linker must be used instead of **gcc**. The new (from MonoTouch v.4.x) '-cxx' option is specified in the build options to enable this.

##

Chris Branson, 5th August 2011
https://github.com/chrisbranson