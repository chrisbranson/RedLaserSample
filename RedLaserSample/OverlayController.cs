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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.AudioToolbox;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;

using RedLaser;

namespace RedLaserSample
{
	public partial class OverlayController : UIViewController
	{
		public BarcodePickerController parentPicker { get; set; }
		
		CAShapeLayer _rectLayer;
		bool _isSilent;
		
		public OverlayController (IntPtr handle) : base (handle) { }  
		
		internal void BeepOrVibrate ()
		{
			if (!_isSilent)
			{
				SystemSound.FromFile ("Sounds/beep.wav").PlayAlertSound ();
			}
		}
		
		partial void cancelPressed (MonoTouch.UIKit.UIBarButtonItem sender)
		{
			if (parentPicker != null)
			{
				// Pass-through the cancel operation
				parentPicker.Cancel ();
			}
		}
		
		partial void flashPressed (MonoTouch.UIKit.UIBarButtonItem sender)
		{
			if (flashButton.Style == UIBarButtonItemStyle.Bordered)
			{
				flashButton.Style = UIBarButtonItemStyle.Done;
				parentPicker.TurnFlash (true);
			}
			else
			{
				flashButton.Style = UIBarButtonItemStyle.Bordered;
				parentPicker.TurnFlash (false);
			}
		}
		
		// Optionally, you can change the active scanning region.
		// The region specified below is the default, and lines up
		// with the default overlay.  It is recommended to keep the
		// active region similar in size to the default region.
		// Additionally, the iPhone 3GS may not focus as well if
		// the region is too far away from center.
		//
		// In portrait mode only the top and bottom of this rectangle
		// is used. The x-position and width specified are ignored.

		void setPortraitLayout ()
		{
			// Set portrait
			parentPicker.Orientation = UIImageOrientation.Up;
			
			// Set the active scanning region for portrait mode
			parentPicker.ActiveRegion = new RectangleF (0, 100, 320, 250);
			
			// Activate the new settings
			parentPicker.ResumeScanning ();
			
			// Animate the UI changes
			CGAffineTransform transform = CGAffineTransform.MakeRotation (0);
			this.View.Transform = transform;
			UIView.BeginAnimations ("rotateToPortrait");
			//UIView.SetAnimationDelegate = this;
			UIView.SetAnimationCurve (UIViewAnimationCurve.Linear);
			UIView.SetAnimationDuration (0.5f);
			
			redlaserLogo.Transform = transform;
			
			setActiveRegionRect ();
			
			UIView.CommitAnimations (); // Animate!
		}
		
		void setLandscapeLayout ()
		{
			// Set landscape
			parentPicker.Orientation = UIImageOrientation.Right;
			
			// Set the active scanning region for portrait mode
			parentPicker.ActiveRegion = new RectangleF (100, 0, 120, 436);
			
			// Activate the new settings
			parentPicker.ResumeScanning ();
			
			// Animate the UI changes
			CGAffineTransform transform = CGAffineTransform.MakeRotation (3.14159f/2.0f);
			this.View.Transform = transform;
			UIView.BeginAnimations ("rotateToLandscape");
			//UIView.SetAnimationDelegate = this;
			UIView.SetAnimationCurve (UIViewAnimationCurve.Linear);
			UIView.SetAnimationDuration (0.5f);
			
			redlaserLogo.Transform = transform;
			
			setActiveRegionRect ();
			
			UIView.CommitAnimations (); // Animate!
		}
		
		partial void rotatePressed (MonoTouch.UIKit.UIBarButtonItem sender)
		{
			// Swap the orientation
			if (parentPicker.Orientation == UIImageOrientation.Up)
			{
				setLandscapeLayout ();
			}
			else 
			{
				setPortraitLayout ();
			}
		}
		
		CGPath newPathInRect (RectangleF rect)
		{
			CGPath path = new CGPath ();
			path.AddRect (rect);
			return path;
		}
		
		void setActiveRegionRect ()
		{
			_rectLayer.Frame = new RectangleF (parentPicker.ActiveRegion.X,
			                                   parentPicker.ActiveRegion.Y,
			                                   parentPicker.ActiveRegion.Width,
			                                   parentPicker.ActiveRegion.Height);
			
			CGPath path = newPathInRect (_rectLayer.Bounds);
			_rectLayer.Path = path;
			_rectLayer.SetNeedsLayout ();
		}
		
		internal void SetArrows (bool inRange, bool animated)
		{
			if (inRange)
				_rectLayer.StrokeColor = UIColor.Green.CGColor;
			else
				_rectLayer.StrokeColor = UIColor.White.CGColor;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Create active region rectangle
			_rectLayer = new CAShapeLayer ();
			_rectLayer.FillColor = UIColor.FromRGBA (1.0f, 0.0f, 0.0f, 0.2f).CGColor;
			_rectLayer.StrokeColor = UIColor.White.CGColor;
			_rectLayer.LineWidth = 3;
			_rectLayer.AnchorPoint = PointF.Empty;
			this.View.Layer.AddSublayer (_rectLayer);
			
			// get is device silent flag
			_isSilent = NSUserDefaults.StandardUserDefaults.BoolForKey("silent_pref");
			
			// set camera view
			parentPicker.CameraView = cameraView;
		}
	
		internal class OverlayDelegate : RedLaser.RealtimeOverlayDelegate
		{
			OverlayController controller;
			
			public OverlayDelegate (OverlayController controller)
			{
				this.controller = controller;
			}
			
			public override void PickerAppeared (BarcodePickerController picker)
			{
			}
			
			public override void PickerAppearing (BarcodePickerController controller)
			{
				if (this.controller.parentPicker.Orientation == UIImageOrientation.Up)
					this.controller.setPortraitLayout ();
				else
					this.controller.setLandscapeLayout ();
				
				if (!this.controller.parentPicker.HasFlash)
				{
					this.controller.flashButton.Enabled = false;
					// TODO: remove flash button from toolBar
					/*
					NSMutableArray * items = [[toolBar items] mutableCopy];
					[items removeObject:flashButton];
					[toolBar setItems:items animated:NO];
					[items release];
					*/
				}
				else
				{
					this.controller.flashButton.Enabled = true;
					this.controller.flashButton.Style = UIBarButtonItemStyle.Bordered;
				}
			}
			
			public override void StatusUpdated (BarcodePickerController picker, NSDictionary status)
			{
				NSNumber isValid = (NSNumber) status.ObjectForKey (new NSString ("Valid"));
				NSNumber inRange = (NSNumber) status.ObjectForKey (new NSString ("InRange"));
				
				controller.SetArrows (inRange.BoolValue, true);
				
				if (isValid.BoolValue)
				{
					controller.BeepOrVibrate ();
					
					NSString barcode = (NSString)status.ObjectForKey (new NSString ("EAN"));
					picker.ReturnBarcode (barcode.ToString (), status);
				}
			}
		}
	}
}
