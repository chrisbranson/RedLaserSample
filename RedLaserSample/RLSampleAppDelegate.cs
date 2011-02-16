/*
 * A MonoTouch implementation of RedLaser's SDK sample
 * 
 * Chris Branson, November 2009
 * Chris Branson, August 2010 - updated to support RedLaser SDK 2.8.2
 * 
 * This is the sample view controller and demonstrates initialisation of
 * the barcode picker controller, setting of properties and handling
 * of events.
 * 
 * Please refer to README.TXT for important information regarding this solution.
 * 
 */

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.ObjCRuntime;
using System;

namespace RedLaserSample
{
	public partial class RLSampleAppDelegate : UIApplicationDelegate
	{
		//UIWindow window;
		//RLSampleViewController viewController;

		public override void FinishedLaunching (UIApplication application)
		{
			// setup status bar style
			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.BlackOpaque;
		
			//viewController = new RLSampleViewController ();
			window.AddSubview (viewController.View);
            window.MakeKeyAndVisible ();
		}

	}
}
