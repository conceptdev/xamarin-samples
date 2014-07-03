
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Xamarin.HeartMonitor
{
	/// <summary>Kick everything off</summary>
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}

	/// <summary>
	/// Sizes the window according to the screen, for iPad as well as iPhone support
	/// </summary>
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			var ms = new MainScreen();
			
			window = new UIWindow (UIScreen.MainScreen.Bounds);	
			window.BackgroundColor = UIColor.White;
			window.Bounds = UIScreen.MainScreen.Bounds;

			window.RootViewController = ms;
            window.MakeKeyAndVisible ();
			return true;
		}
	}
}