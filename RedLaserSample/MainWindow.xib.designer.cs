// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace RedLaserSample {
	
	
	// Base type probably should be MonoTouch.Foundation.NSObject or subclass
	[MonoTouch.Foundation.Register("RLSampleAppDelegate")]
	public partial class RLSampleAppDelegate {
		
		private RLSampleViewController __mt_viewController;
		
		private MonoTouch.UIKit.UIWindow __mt_window;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("viewController")]
		private RLSampleViewController viewController {
			get {
				this.__mt_viewController = ((RLSampleViewController)(this.GetNativeField("viewController")));
				return this.__mt_viewController;
			}
			set {
				this.__mt_viewController = value;
				this.SetNativeField("viewController", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("window")]
		private MonoTouch.UIKit.UIWindow window {
			get {
				this.__mt_window = ((MonoTouch.UIKit.UIWindow)(this.GetNativeField("window")));
				return this.__mt_window;
			}
			set {
				this.__mt_window = value;
				this.SetNativeField("window", value);
			}
		}
	}
}
