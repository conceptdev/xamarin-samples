using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PaintCode
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}

	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UINavigationController navCtrlr;
		UIViewController news;		

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			navCtrlr = new UINavigationController ();
			
			news = new NewsDialogViewController ();
			news = new BlueButtonViewController ();
			news = new GlossyButtonViewController ();
			
			//news.View.AutoresizingMask = UIViewAutoresizing.All;
			news.View.Frame = new System.Drawing.RectangleF (0
						, UIApplication.SharedApplication.StatusBarFrame.Height
						, UIScreen.MainScreen.ApplicationFrame.Width
						, UIScreen.MainScreen.ApplicationFrame.Height);

			navCtrlr.PushViewController (news, false);
			
			window.AddSubview (navCtrlr.View);
			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

