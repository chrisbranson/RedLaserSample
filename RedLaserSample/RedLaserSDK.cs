using System;
using System.Runtime.InteropServices;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.RedLaser;

namespace MonoTouch.RedLaser
{
	public static class RedLaserSDK {
		/*******************************************************************************
			RL_GetRedLaserSDKVersion()
	
			This function returns the version of the RedLaser SDK, as a NSString.
			The primary purpose of this function is checking which SDK version you're 
			linking against, to compare that version against the most recent version 
			on redlaser.com. 
		*/
		
		//NSString *RL_GetRedLaserSDKVersion();
		[DllImport ("__Internal")]
		static extern IntPtr RL_GetRedLaserSDKVersion ();
		
		public static string GetRedLaserSDKVersion ()
		{
			NSString sdkVer = new NSString (RL_GetRedLaserSDKVersion ());
			
			return sdkVer.ToString ();
		}
		
		
		/*******************************************************************************
			RL_CheckReadyStatus()
	
			This function returns information about whether the SDK can be used. It 
			doesn't give dynamic state information about what the SDK is currently doing.
	
			Generally, positive values mean you can scan, negative values mean you 
			can't. The returned value *can* change from one call to the next. 
	
			If this function returns a negative value, it's usually best to design your
			app so that it won't attempt to scan at all. If this function returns
			MissingOSLibraries this is especially important, as the SDK will probably 
			crash if used. See the documentation. 
		*/
	
		// RedLaserStatus RL_CheckReadyStatus();
		[DllImport ("__Internal")]
		static extern int RL_CheckReadyStatus ();
		
		public static RedLaserStatus CheckReadyStatus ()
		{
			return (RedLaserStatus) RL_CheckReadyStatus ();
		}
		
		
		/*******************************************************************************
			FindBarcodesInUIImage
	
			Searches the given image for barcodes, and returns information on all barcodes
			that it finds. This performs an exhaustive search, which can take several 
			seconds to perform. This method searches for all barcode types. The intent
			of this method is to allow for barcode searching in photos from the photo library.
		*/
		
		//NSSet *FindBarcodesInUIImage(UIImage *inputImage);
		[DllImport ("__Internal")]
		public static extern NSSet FindBarcodesInUIImage (UIImage inputImage);
	}
}
