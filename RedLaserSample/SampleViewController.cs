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
	public class SampleViewController : UIViewController
	{
		BarcodePickerController picker;
		BarcodeOverlayController overlay;
		
		UILabel lblHeading, lblEANLabel, lblTypeLabel;
		UIToolbar toolbar;
		
		UILabel lblEAN8, lblUPCE, lblEAN13, lblSTICKY;
		UISwitch enableEAN8Switch, enableUPCESwitch, enableEAN13Switch, enableSTICKYSwitch;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// create view controls
			SetupView();
		}
		
		void SetupView()
		{
			// create toolbar
			toolbar = new UIToolbar()
			{
				Frame = new RectangleF(0, 416, 320, 44),
				Opaque = false,
				BarStyle = UIBarStyle.Black,
			};
			
			// setup toolbar buttons
			UIBarButtonItem[] buttons = new UIBarButtonItem[3];
			buttons[0] = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null, null);
			buttons[1] = new UIBarButtonItem(UIImage.FromFile("RedLaser/bolt.png"), UIBarButtonItemStyle.Plain, null);
			buttons[1].Clicked += ScanClicked;
			buttons[2] = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null, null);
			toolbar.Items = buttons;
			
			// create UI controls
			lblHeading = new UILabel()
			{
				Frame = new RectangleF(0, 54, 320, 21),
				Text = "Occipital RedLaser",
				Font = UIFont.BoldSystemFontOfSize(17),
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			lblEANLabel = new UILabel()
			{
				Frame = new RectangleF(0, 83, 320, 39),
				Text = "[...]",
				Font = UIFont.SystemFontOfSize(24),
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			lblTypeLabel = new UILabel()
			{
				Frame = new RectangleF(0, 120, 320, 29),
				Text = "",
				Font = UIFont.SystemFontOfSize(24),
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			lblSTICKY = new UILabel()
			{
				Frame = new RectangleF(147, 235, 130, 21),
				Text = "Enable STICKY",
				Font = UIFont.SystemFontOfSize(17),
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			lblEAN13 = new UILabel()
			{
				Frame = new RectangleF(147, 280, 130, 21),
				Text = "Enable EAN-13",
				Font = UIFont.SystemFontOfSize(17),
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			lblEAN8 = new UILabel()
			{
				Frame = new RectangleF(147, 325, 130, 21),
				Text = "Enable EAN-8",
				Font = UIFont.SystemFontOfSize(17),
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			lblUPCE = new UILabel()
			{
				Frame = new RectangleF(147, 370, 130, 21),
				Text = "Enable UPC-E",
				Font = UIFont.SystemFontOfSize(17),
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			enableSTICKYSwitch = new UISwitch()
			{
				Frame = new RectangleF(36, 235, 94, 27),
				Opaque = false,
				On = false,
			};
			
			enableEAN13Switch = new UISwitch()
			{
				Frame = new RectangleF(36, 280, 94, 27),
				Opaque = false,
				On = false,
			};
			
			enableEAN8Switch = new UISwitch()
			{
				Frame = new RectangleF(36, 325, 94, 27),
				Opaque = false,
				On = false,
			};
			
			enableUPCESwitch = new UISwitch()
			{
				Frame = new RectangleF(36, 370, 94, 27),
				Opaque = false,
				On = true,
			};
			
			// add controls to view
			this.View.AddSubview(toolbar);
			this.View.AddSubview(lblHeading);
			this.View.AddSubview(lblEANLabel);
			this.View.AddSubview(lblTypeLabel);
			this.View.AddSubview(lblSTICKY);
			this.View.AddSubview(enableSTICKYSwitch);
			this.View.AddSubview(lblEAN13);
			this.View.AddSubview(enableEAN13Switch);
			this.View.AddSubview(lblEAN8);
			this.View.AddSubview(enableEAN8Switch);
			this.View.AddSubview(lblUPCE);
			this.View.AddSubview(enableUPCESwitch);
		}

		void ScanClicked (object sender, EventArgs e)
		{
			// setup controllers
			picker = new BarcodePickerController();
			overlay = new BarcodeOverlayController() { parentPicker = picker, };
			
			// select the appropriate overlay
			if (enableUPCESwitch.On)
				overlay.overlayImage = UIImage.FromFile("RedLaser/RedLaserOverlay_UPCE.png");
			else
				overlay.overlayImage = UIImage.FromFile("RedLaser/RedLaserOverlay.png");
			
			// setup picker
			picker.OverlayDelegate = new BarcodeOverlayController.OverlayDelegate(overlay);
			picker.Delegate = new BarcodePickerDelegate(this);
			picker.View = overlay.View;
			
			// set requested barcode formats
			picker.ScanEAN8 = enableEAN8Switch.On;
			picker.ScanUPCE = enableUPCESwitch.On;
			picker.ScanEAN13 = enableEAN13Switch.On;
			picker.ScanSTICKY = enableSTICKYSwitch.On;
			
			// Optionally, you can change the active scanning region.
			// The region specified below is the default, and lines up
			// with the default overlay.  It is recommended to keep the
			// active region similar in size to the default region.
			// Additionally, the iPhone 3GS may not focus as well if
			// the region is too far away from center.
			//
			// Currently, only the top and bottom of this rectangle is used.
			// The x-position and width specified are ignored.
			//picker.ActiveRegion = new RectangleF(0, 146, 320, 157);
			
			/* UNCOMMENT THIS SECTION TO SCAN HORIZONTALLY
	   		// NOTE: Overlay must be modified to properly support horizontal scanning.
	
			// Orientation control:
			// Only UIImageOrientationUp and UIOrientationRight are supported currently.
			
			picker.Orientation = UIImageOrientation.Right;
			//picker.ActiveRegion = new RectangleF(80, 52, 157, 320);
			*/
			
			// old properties for earlier RedLaser SDK
			//picker.SourceType = UIImagePickerControllerSourceType.Camera;
			//picker.AllowsEditing = true;
			//picker.CameraOverlayView = overlay.View;
			//picker.ShowsCameraControls = false;
			
			// start up
			Console.WriteLine("Start Up");
			this.PresentModalViewController(picker, true);
		}
		
		internal void BarcodeScanned(RLBarcodeType type, string ean)
		{
			// update view with scanned barcode type
			if (type == RLBarcodeType.EAN13)
			{
				if (ean[0] == '0')
				{
					ean = ean.Substring(1);
					lblTypeLabel.Text = "UPC-A";
				}
				else
					lblTypeLabel.Text = "EAN-13";
			}
			else if (type == RLBarcodeType.UPCE)
			{
					lblTypeLabel.Text = "UPC-E";
			}
			else if (type == RLBarcodeType.EAN8)
			{
					lblTypeLabel.Text = "EAN-8";	
			}
			else if (type == RLBarcodeType.STICKY)
			{
					lblTypeLabel.Text = "STICKY";	
			}
			
			// update view with scanned barcode value
			lblEANLabel.Text = ean;
		}
		
		private class BarcodePickerDelegate : RedLaser.BarcodePickerControllerDelegate
		{
			SampleViewController controller;
			
			public BarcodePickerDelegate(SampleViewController controller)
			{
				this.controller = controller;
			}
			
			public override void BarcodeScanned (BarcodePickerController picker, string ean, NSDictionary info)
			{
				//Console.WriteLine("BarcodeScanned");
				
				// must call this to prevent multiple scan callbacks
				picker.StopScanning();
				
				// restore main screen (and restore title bar for 3.0)
				controller.DismissModalViewControllerAnimated(true);
				
				// retrieve the barcode type
				int btype = ((NSNumber)info.ObjectForKey(new NSString("BarcodeType"))).IntValue;
				RLBarcodeType type = (RLBarcodeType)btype;
				
				// send event through to parent controller
				if (controller != null) controller.BarcodeScanned(type, ean);
			}
			
			public override void Cancelled (BarcodePickerController picker)
			{
				//Console.WriteLine("Cancelled");
				
				// must call this to prevent multiple scan callbacks
				picker.StopScanning();
				
				// restore main screen (and restore title bar for 3.0)
				controller.DismissModalViewControllerAnimated(true);
			}
			
			/* old method for earlier RedLaser SDK
			public override void FinishPicking (UIImage image, NSDictionary editingInfo)
			{
				//Console.WriteLine("FinishPicking");
				
				// must call this to prevent multiple scan callbacks
				controller.picker.StopScanning();
				
				// restore main screen (and restore title bar for 3.0)
				controller.DismissModalViewControllerAnimated(true);
				
				// alert user to failure
				using(UIAlertView alert = new UIAlertView("Barcode not recognised",
					"Try again.", null, "OK", null))
							alert.Show();
			}
			*/
		}
	}
}