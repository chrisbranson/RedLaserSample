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

using RedLaser;

namespace RedLaserSample
{
	public partial class RLSampleViewController : UIViewController
	{
		public RLSampleViewController (IntPtr handle) : base (handle) { }  
		
		partial void scanPressed (MonoTouch.UIKit.UIBarButtonItem sender)
		{
			if (overlayController.parentPicker == null)
			{
				BarcodePickerController picker = new BarcodePickerController ();
				
				// Allows the overlay controller to trigger cancel
				overlayController.parentPicker = picker;
				
				// setup picker
				picker.OverlayDelegate = new OverlayController.OverlayDelegate (overlayController);
				picker.Delegate = new BarcodePickerDelegate (this);
				picker.View = overlayController.View;
				
				// Initialize with portrait mode as default
				picker.Orientation = UIImageOrientation.Up;
				
				// The active scanning region size is set in OverlayController.m
			}
			
			// Update barcode on/off settings
			overlayController.parentPicker.ScanUPCE = enableUPCESwitch.On;
			overlayController.parentPicker.ScanEAN8 = enableEAN8Switch.On;
			overlayController.parentPicker.ScanEAN13 = enableEAN13Switch.On;
			overlayController.parentPicker.ScanSTICKY = enableSTICKYSwitch.On;
			overlayController.parentPicker.ScanQRCODE = enableQRCodeSwitch.On;
			overlayController.parentPicker.ScanCODE128 = enableCode128Switch.On;
			overlayController.parentPicker.ScanCODE39 = enableCode39Switch.On;
			//overlayController.parentPicker.ScanDataMatrix = enableDataMatrixSwitch.On;
			overlayController.parentPicker.ScanITF = enableITFSwitch.On;
			
			// Data matrix decoding does not work very well so it is disabled for now
			overlayController.parentPicker.ScanDATAMATRIX = false;
	
			// hide the status bar
			UIApplication.SharedApplication.StatusBarHidden = true;
			
			// Show the scanner overlay
			this.PresentModalViewController (overlayController.parentPicker, true);
		}
		
		internal void BarcodeScanned (RLBarcodeType type, string ean)
		{
			// update view with scanned barcode type
			if (type == RLBarcodeType.EAN13)
			{
				// Use first digit to differentiate between EAN13 and UPCA
				if (ean[0] == '0')
				{
					ean = ean.Substring (1);
					typeLabel.Text = "UPC-A";
				}
				else
					typeLabel.Text = "EAN-13";
			}
			else if (type == RLBarcodeType.UPCE)
			{
					typeLabel.Text = "UPC-E";
			}
			else if (type == RLBarcodeType.EAN8)
			{
					typeLabel.Text = "EAN-8";	
			}
			else if (type == RLBarcodeType.STICKY)
			{
					typeLabel.Text = "STICKYBITS";	
			}
			else if (type == RLBarcodeType.QRCODE)
			{
					typeLabel.Text = "QR Code";	
			}
			else if (type == RLBarcodeType.Code128)
			{
					typeLabel.Text = "Code 128";	
			}
			else if (type == RLBarcodeType.Code39)
			{
					typeLabel.Text = "Code 39";	
			}
			else if (type == RLBarcodeType.DataMatrix)
			{
					typeLabel.Text = "Data Matrix";	
			}
			else if (type == RLBarcodeType.ITF)
			{
					typeLabel.Text = "ITF";	
			}
			
			// update view with scanned barcode value
			eanLabel.Text = ean;
		}
		
		private class BarcodePickerDelegate : RedLaser.BarcodePickerControllerDelegate
		{
			RLSampleViewController controller;
			
			public BarcodePickerDelegate (RLSampleViewController controller)
			{
				this.controller = controller;
			}
			
			public override void BarcodeScanned (BarcodePickerController picker, string ean, NSDictionary info)
			{
				// must call this to prevent multiple scan callbacks
				picker.StopScanning ();
				
				UIApplication.SharedApplication.StatusBarHidden = false;
				
				// restore main screen (and restore title bar for 3.0)
				controller.DismissModalViewControllerAnimated(true);
				
				// If desired, you can access the latest frame that the barcode was decoded from
				/*
				UIImage decodedImage = ((UIImage) info.ObjectForKey (new NSString ("Image")));
				NSData bytes = decodedImage.AsPNG ();
				UIImage pngImage = UIImage.LoadFromData (bytes);
				pngImage.SaveToPhotosAlbum(
							(sender, args)=>{Console.WriteLine("image saved to Photos");}
				);
				*/
				
				// retrieve the barcode type
				int btype = ((NSNumber)info.ObjectForKey(new NSString("BarcodeType"))).IntValue;
				RLBarcodeType type = (RLBarcodeType)btype;
				
				// send event through to parent controller
				if (controller != null) controller.BarcodeScanned(type, ean);
			}
			
			public override void Cancelled (BarcodePickerController picker)
			{
				// must call this to prevent multiple scan callbacks
				picker.StopScanning();
				
				UIApplication.SharedApplication.StatusBarHidden = false;
				
				// restore main screen
				controller.DismissModalViewControllerAnimated(true);
			}
		}
	}
}