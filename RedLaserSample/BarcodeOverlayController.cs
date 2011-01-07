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

using RedLaser;

namespace RedLaserSample
{
	public class BarcodeOverlayController : UIViewController
	{
		public BarcodePickerController parentPicker { get; set; }
		public UIImage overlayImage { get; set; }
		
		UIImageView _greenTopArrow;
		UIImageView _greenBottomArrow;
		UIImageView _whiteTopArrow;
		UIImageView _whiteBottomArrow;
		UIImageView _bottomBar;
		UIImageView _overlay;
		UIView _cameraView;
		
		UIButton _cancelButton;
		
		UILabel _textCue, _latestResultLabel;
		
		bool _isGreen;
		bool _isSilent;
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// setup view controls
			SetupView();
			
			// initalise flags
			_isGreen = false;
			SetArrows(false, false);
			
			// set camera view
			parentPicker.CameraView = _cameraView;
			
			// hide this view (will appear when picker requests, see overlay delegate)
			this.View.Hidden = true;
			
			// get is device silent flag
			_isSilent = NSUserDefaults.StandardUserDefaults.BoolForKey("silent_pref");
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			if (overlayImage != null) overlayImage.Dispose();
		}
		
		void SetupView()
		{
			_overlay = new UIImageView(overlayImage);
			_overlay.Frame = new RectangleF(0,0,320,427);
			
			_cameraView = new UIView(new RectangleF(0,0,320,480));
			
			_latestResultLabel = new UILabel()
			{
				Frame = new RectangleF(49,336,182,33),
				Text = "0000000000000",
				Font = UIFont.SystemFontOfSize(24),
				TextAlignment = UITextAlignment.Left,
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			_textCue = new UILabel()
			{
				Frame = new RectangleF(0,102,320,21),
				Text = "Align barcode edges with screen",
				Font = UIFont.SystemFontOfSize(13),
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White,
				Opaque = false,
				BackgroundColor = UIColor.FromRGBA(0,0,0,0),
			};
			
			_greenBottomArrow = new UIImageView(UIImage.FromFile("RedLaser/green_down_arrow.png"));
			_greenBottomArrow.Frame = new RectangleF(0,129,320,19);
			_greenTopArrow = new UIImageView(UIImage.FromFile("RedLaser/green_up_arrow.png"));
			_greenTopArrow.Frame = new RectangleF(0,302,320,19);
			
			_whiteBottomArrow = new UIImageView(UIImage.FromFile("RedLaser/white_down_arrow.png"));
			_whiteBottomArrow.Frame = new RectangleF(0,129,320,19);
			_whiteTopArrow = new UIImageView(UIImage.FromFile("RedLaser/white_up_arrow.png"));
			_whiteTopArrow.Frame = new RectangleF(0,302,320,19);
			
			_bottomBar = new UIImageView(UIImage.FromFile("RedLaser/botbar.png"));
			_bottomBar.Frame = new RectangleF(0,422,320,57);
			
			_cancelButton = new UIButton()
			{
				Frame = new RectangleF(5,434,72,37),
			};
			_cancelButton.SetBackgroundImage(UIImage.FromFile("RedLaser/cancel.png"), UIControlState.Normal);
			_cancelButton.SetBackgroundImage(UIImage.FromFile("RedLaser/cancel_down.png"), UIControlState.Selected);
			_cancelButton.TouchUpInside += Handle_cancelButtonTouchUpInside;
			
			this.View.AddSubview(_cameraView);
			this.View.AddSubview(_overlay);
			this.View.AddSubview(_latestResultLabel);
			this.View.AddSubview(_textCue);
			this.View.AddSubview(_greenBottomArrow);
			this.View.AddSubview(_greenTopArrow);
			this.View.AddSubview(_whiteBottomArrow);
			this.View.AddSubview(_whiteTopArrow);
			this.View.AddSubview(_bottomBar);
			this.View.AddSubview(_cancelButton);
		}

		void Handle_cancelButtonTouchUpInside (object sender, EventArgs e)
		{
			if (parentPicker != null)
			{
				this.View.Hidden = true;
				parentPicker.Cancel();
			}
		}
		
		internal void SetArrows(bool inRange, bool animated)
		{
			UIImageView otherView1, otherView2, focusView1, focusView2;
			
			// already showing green bars
			if (inRange && _isGreen) return;
			
			// update flag
			_isGreen = inRange;
			
			if (_isGreen)
			{
				_textCue.Text = "Hold still for scan";
				otherView1 = _whiteTopArrow; otherView2 = _whiteBottomArrow;
				focusView1 = _greenTopArrow; focusView2 = _greenBottomArrow;
			}
			else
			{
				_textCue.Text = "Align barcode edges with arrows";
				focusView1 = _whiteTopArrow; focusView2 = _whiteBottomArrow;
				otherView1 = _greenTopArrow; otherView2 = _greenBottomArrow;
			}
			
			if (animated)
			{
				UIView.BeginAnimations("");
				UIView.SetAnimationDuration(0.15f);
				UIView.SetAnimationBeginsFromCurrentState(true);
			}
			
			focusView1.Alpha = 1;
			focusView2.Alpha = 1;
			
			otherView1.Alpha = 0;
			otherView2.Alpha = 0;
			
			if (animated) UIView.CommitAnimations();
		}
		
		internal void BeepOrVibrate()
		{
			if (!_isSilent)
			{
				SystemSound.FromFile("Sounds/beep.wav").PlayAlertSound();
			}
		}
	
		internal class OverlayDelegate : RedLaser.RealtimeOverlayDelegate
		{
			BarcodeOverlayController controller;
			
			public OverlayDelegate(BarcodeOverlayController controller)
			{
				this.controller = controller;
			}
			
			public override void PickerAppeared (BarcodePickerController picker)
			{
				//Console.WriteLine("PickerAppeared");
				this.controller.View.Hidden = false;
			}
			
			public override void StatusUpdated (BarcodePickerController picker, NSDictionary status)
			{
				//Console.WriteLine("StatusUpdated");
				
				NSNumber isValid = (NSNumber) status.ObjectForKey(new NSString("Valid"));
				NSNumber inRange = (NSNumber) status.ObjectForKey(new NSString("InRange"));
				
				controller.SetArrows(inRange.BoolValue, true);
				
				if (isValid.BoolValue)
				{
					controller.BeepOrVibrate();
					controller.View.Hidden = true;
					
					NSString barcode = (NSString)status.ObjectForKey(new NSString("EAN"));
					picker.ReturnBarcode(barcode.ToString(), status);
				}
			}
		}
	}
}
