using System;
using MonoTouch.CoreImage;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace NonConsumables {
	// WARNING: this is a trivial example of tracking non-consumable
	// in-app purchases. In reality this should be encrypted and
	// possibly even managed remotely on your server (with a strategy for offline use).
	// NSUserDefaults are EASY for iOS users to edit with a little bit of knowledge,
	// which means they could bypass paying for the features.
	// Basically, this is ONLY intended as a demo of the StoreKit code,
	// NOT how you should build a feature-management system for iOS apps.
	public static class PhotoFilterManager {
		static PhotoFilterManager ()
		{
		}
		
		public static void Purchase (string productId) {
			var key = new NSString(productId);
			NSUserDefaults.StandardUserDefaults.SetBool(true, key);
			NSUserDefaults.StandardUserDefaults.Synchronize ();
		}
		public static bool HasPurchased (string productId) {
			var key = new NSString(productId);
			return NSUserDefaults.StandardUserDefaults.BoolForKey (key);
		}

		
		public static void ApplySepia(string imagePath, UIImageView imgview)
		{	
			var uiimage = UIImage.FromFile (imagePath);
			var ciimage = new CIImage (uiimage);
			
			var sepia = new CISepiaTone();
			sepia.Image = ciimage;
			sepia.Intensity = 0.8f;
			var output = sepia.OutputImage;

			var context = CIContext.FromOptions(null);
			var cgimage = context.CreateCGImage (output, output.Extent);
			var ui = UIImage.FromImage (cgimage);

			imgview.Image = ui;
		}
		public static void ApplyGreyscale(string imagePath, UIImageView imgview)
		{	
			var uiimage = UIImage.FromFile (imagePath);
			var ciimage = new CIImage (uiimage);
			
			var greyscale = new CIColorControls();
			greyscale.Image = ciimage;
			greyscale.Saturation = 0f;
			var output = greyscale.OutputImage;

			var context = CIContext.FromOptions(null);
			var cgimage = context.CreateCGImage (output, output.Extent);
			var ui = UIImage.FromImage (cgimage);

			imgview.Image = ui;
		}
	}
}