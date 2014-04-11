using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.InAppPurchase;
using Xamarin.InAppPurchase.Utilities;

namespace OneCoolThing
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{

		public InAppPurchaseManager PurchaseManager = new InAppPurchaseManager ();


		// class-level declarations
		public override UIWindow Window {
			get;
			set;
		}
		ThingViewController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{


			// create a new window instance based on the screen size
			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new ThingViewController();

			// If you have defined a view, add it here:
			//window.AddSubview (navigationController.View);
			Window.RootViewController = viewController;


			// Initialize the In App Purchase Manager
			PurchaseManager.simulateiTunesAppStore = false; // use the real thing
			PurchaseManager.PublicKey = "ASDFASDFASDF";
			PurchaseManager.automaticPersistenceType = InAppPurchasePersistenceType.UserDefaults;



			// Warn user that the store is not available
			if (PurchaseManager.canMakePayments) {
				Console.WriteLine ("Xamarin.InAppBilling: User can make payments to iTunes App Store.");
			} else {
				//Display Alert Dialog Box
				using(var alert = new UIAlertView("Xamarin.InAppBilling", "Sorry but you cannot make purchases from the In App Billing store. Please try again later.", null, "OK", null))
				{
					alert.Show();	
				}
			}

			// Show any invalid product queries
			PurchaseManager.ReceivedInvalidProducts += (productIDs) => {
				// Display any invalid product IDs to the console
				Console.WriteLine("PurchaseManager.ReceivedInvalidProducts - The following IDs were rejected by the iTunes App Store:");
				foreach(string ID in productIDs){
					Console.WriteLine(ID);
				}
			};

			// Report the results of the user restoring previous purchases
			PurchaseManager.InAppPurchasesRestored += (count) => {
				Console.WriteLine("PurchaseManager.InAppPurchasesRestored");
			};

			// Report miscellanous processing errors
			PurchaseManager.InAppPurchaseProcessingError += (message) => {
				Console.WriteLine("PurchaseManager.InAppPurchaseProcessingError " + message);
			};

			// Report any issues with persistence
			PurchaseManager.InAppProductPersistenceError += (message) => {
				Console.WriteLine("PurchaseManager.InAppProductPersistenceError");
			};

			PurchaseManager.TransactionsRemovedFromQueue += (transactions) => {
				Console.WriteLine("PurchaseManager.TransactionsRemovedFromQueue " + transactions.Count());
				foreach (var t in transactions){
					Console.WriteLine("--- " + t.Payment.ProductIdentifier);
				}

			};

			viewController.AttachToPurchaseManager (PurchaseManager);

			// make the window visible
			Window.MakeKeyAndVisible ();

			return true;
		}

		// This method is invoked when the application is about to move from active to inactive state.
		// OpenGL applications should use this method to pause.
		public override void OnResignActivation (UIApplication application)
		{
		}
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		// This method is called when the application is about to terminate. Save data, if needed.
		public override void WillTerminate (UIApplication application)
		{
		}
	}

	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

