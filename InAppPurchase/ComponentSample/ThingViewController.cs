using System;
using System.Collections.Generic;
using MonoTouch.StoreKit;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using Xamarin.InAppPurchase;

namespace OneCoolThing
{
	public class ThingViewController : UIViewController {
	
		UIButton buyButton, restoreButton;
		UILabel infoLabel;
		UILabel resultLabel;

		// This is the product ID that has been configured in itunesconnect.apple.com
		string appStoreProductId = "net.conceptdevelopment.onecoolthing.product";

		public ThingViewController ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "OneCoolThing";
			View.BackgroundColor = UIColor.White;

			infoLabel = new UILabel(new RectangleF(10, 10, 300, 80));
			infoLabel.Lines = 3;
			infoLabel.Text = "Welcome to ONE COOL THING";

			resultLabel = new UILabel(new RectangleF(10, 160, 300, 300));
			resultLabel.Lines = 5;
			//resultLabel.Text = "Congratulations, if this is showing then you have purchased One Cool Thing via the App Store - maybe your first In App Purchase!";

			buyButton = UIButton.FromType (UIButtonType.RoundedRect);
			buyButton.Frame = new RectangleF(10, 100, 300, 40);
			buyButton.BackgroundColor = UIColor.Gray;
			buyButton.SetTitle ("loading...", UIControlState.Disabled); // updated once product info is downloaded from App Store
			buyButton.SetTitleColor(UIColor.Gray, UIControlState.Disabled); // enabled if product info is downloaded
			buyButton.Enabled = false;

			buyButton.TouchUpInside += (sender, e) => {
				Console.WriteLine("BuyProduct " + appStoreProductId);
				_purchaseManager.BuyProduct (appStoreProductId);
			};	


			restoreButton = UIButton.FromType (UIButtonType.RoundedRect);
			restoreButton.Frame = new RectangleF(10, 400, 150, 40);
			restoreButton.BackgroundColor = UIColor.LightGray;
			restoreButton.SetTitle ("Restore", UIControlState.Normal); // updated once product info is downloaded from App Store

			restoreButton.TouchUpInside += (sender, e) => {
				Console.WriteLine("Attempt RestorePreviousPurchases");
				_purchaseManager.RestorePreviousPurchases();
			};	

			View.Add (buyButton);
			View.Add (infoLabel);
			View.Add (resultLabel);
			View.Add (restoreButton);

			_purchaseManager.QueryInventory (appStoreProductId);

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//ReloadData ();
		}


		void ReloadData() {
			Console.WriteLine ("ReloadData");

			if (_purchaseManager.ProductPurchased (appStoreProductId)) {
				// already purchased - disable the button
				buyButton.SetTitle ("(already bought)", UIControlState.Disabled);
				buyButton.BackgroundColor = UIColor.Clear;
				buyButton.Enabled = false;
				resultLabel.Text = "Congratulations, if this is showing then you have purchased One Cool Thing via the App Store - maybe your first In App Purchase!";

			} else {
				// not purchased (that we know of)... 
				var product = _purchaseManager [0];
				buyButton.SetTitle ("Buy " + product.title + " " + product.formattedPrice, UIControlState.Normal);
				buyButton.BackgroundColor = UIColor.Yellow;
				buyButton.Enabled = true;
				resultLabel.Text = "";
			}
		}

		InAppPurchaseManager _purchaseManager;

		public void AttachToPurchaseManager(InAppPurchaseManager purchaseManager) {
			Console.WriteLine ("Attached to ThingViewController");
			_purchaseManager = purchaseManager;

			// Respond to events
			_purchaseManager.ReceivedValidProducts += (products) => {
				// Received valid products from the iTunes App Store,
				// Update the display
				Console.WriteLine("_purchaseManager.ReceivedValidProducts " + products);
				foreach (var p in products){
					Console.WriteLine("--- " + p.productIdentifier + " " + p.price);
				}

				ReloadData();
			};

			_purchaseManager.InAppProductPurchased += (MonoTouch.StoreKit.SKPaymentTransaction transaction, InAppProduct product) => {
				// Update list to remove any non-consumable products that were
				// purchased
				Console.WriteLine("_purchaseManager.InAppProductPurchased " + product.productIdentifier);
				ReloadData();
			};

			_purchaseManager.InAppPurchasesRestored += (count) => {
				// Update list to remove any non-consumable products that were
				// purchased and restored
				Console.WriteLine("_purchaseManager.InAppPurchasesRestored " + count);
				ReloadData();
			};

			_purchaseManager.transactionObserver.InAppPurchaseContentDownloadInProgress += (download) => {
				// Update the table to display the status of any downloads of hosted content
				// that we currently have in progress so we are forcing a table reload on the
				// download progress update. Since the final message will be the raising of the
				// InAppProductPurchased event, we'll just trap it to clear any completed
				// downloads instead of listening to the InAppPurchaseContentDownloadCompleted on the
				// purchase managers transaction observer.

				Console.WriteLine("transactionObserver.InAppPurchaseContentDownloadInProgress " + string.Format ("{0:###}%", _purchaseManager.activeDownloadPercent * 100.0f));
				ReloadData();

				// Display download percent in the badge
				//	StoreTab.BadgeValue = string.Format ("{0:###}%", _purchaseManager.activeDownloadPercent * 100.0f);
			};

			_purchaseManager.transactionObserver.InAppPurchaseContentDownloadCompleted += (download) => {
				// Clear badge
				Console.WriteLine("transactionObserver.InAppPurchaseContentDownloadCompleted");
			};

			_purchaseManager.transactionObserver.InAppPurchaseContentDownloadCanceled += (download) => {
				// Clear badge
				Console.WriteLine("transactionObserver.InAppPurchaseContentDownloadCanceled");
			};

			_purchaseManager.transactionObserver.InAppPurchaseContentDownloadFailed += (download) => {
				// Inform the user that the download has failed. Normally download would contain
				// information about the failure that you would want to display to the user, since
				// we are running in simulation mode download will be null, so just display a 
				// generic failure message.
				using(var alert = new UIAlertView("Download Failed", "Unable to complete the downloading of content for the product being purchased. Please try again later.", null, "OK", null))
				{
					alert.Show();	
				}
				Console.WriteLine("transactionObserver.InAppPurchaseContentDownloadFailed");
				// Force the table to reload to remove current download message
				ReloadData();
			};

			_purchaseManager.InAppProductPurchaseFailed += (transaction, product) => {
				// Inform caller that the purchase of the requested product failed.
				// NOTE: The transaction will normally encode the reason for the failure but since
				// we are running in the simulated iTune App Store mode, no transaction will be returned.
				//Display Alert Dialog Box
				using (var alert = new UIAlertView ("Xamarin.InAppPurchase", String.Format ("Attempt to purchase {0} has failed.", product.title), null, "OK", null)) {
					alert.Show ();	
				}
				Console.WriteLine("InAppProductPurchaseFailed " + transaction.Error.Code + " " + transaction.Error.LocalizedDescription);
				// Force a reload to clear any locked items
				ReloadData ();
			};
		}
	}
}

