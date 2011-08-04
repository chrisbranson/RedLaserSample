This MonoDevelop solution is a MonoTouch implementation of the RedLaser SDK sample (http://www.redlaser.com/SDK.aspx).

This solution requires iOS4 or higher and has been tested using RedLaser SDK v3.1.1, MonoTouch v4.0.4.1 and MonoDevelop 2.6 beta 3.

The solution is self contained and will compile only for the device as the RedLaser SDK requires access to camera hardware which is obviously not present on the simulator.

The project build options includes the neccessary additional compiler commands and frameworks.

Finally, to get this solution to build properly the g++ compiler/linker must be used instead of gcc. The new -cxx option is specified in the build options to enable this.

Chris Branson, 4th August 2011
https://github.com/chrisbranson/RedLaserSample