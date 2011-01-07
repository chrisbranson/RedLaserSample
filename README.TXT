This MonoDevelop solution is a MonoTouch implementation of the RedLaser SDK sample (http://www.redlaser.com/SDK.aspx).

This solution requires iOS4 and has been tested using RedLaser SDK v2.8.2, MonoTouch v3.0.11 and MonoDevelop 2.4.

The solution is self contained and will compile for both the simulator and the device, however the RedLaser SDK requires access to camera hardware which is obviously not present on the simulator. Any attempts to start a scan will result in an exception being raised and the app closing.

The project build options includes the neccessary additional compiler commands and frameworks. Note: at present the 'Release' build requires 

Finally, to get this solution to build properly you will need to temporarily force use of the g++ compiler/linker instead of gcc. This is because the RedLaser SDK makes use of some standard c++ class libraries which. A dummy compiled c++ file (x.o and x_sim.o) is included in the solution to force g++ to include the relevant libraries.

To force the use of g++ perform the following steps;

1. Open a terminal window
2. cd /Developer/Platforms/iPhoneOS.platform/Developer/usr/bin
3. mv gcc-4.2 gcc-4.2.org
4. ln -s g++4.2 gcc-4.2
5. cd /Developer/Platforms/iPhoneSimulator.platform/Developer/usr/bin
6. mv gcc-4.2 gcc-4.2.org
7. ln -s g++4.2 gcc-4.2

You can then switch between the two compilers by removing/adding the symlink. I have seen no ill effects from using g++ all the time, ymmv.

Chris Branson, August 2010
forza04 AT gmail dot com