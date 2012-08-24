using System;
using System.Collections.Generic;
using System.Net;
using System.Json;
using MonoTouch.StoreKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;

namespace ReceiptValidation
{
	/// <summary>
	/// Code has been partially ported from Objective-C provided by Apple to address
	/// the in-app purchase vulnerability discovered in iOS 5.1 in July 2012
	/// https://developer.apple.com/library/ios/#releasenotes/StoreKit/IAP_ReceiptValidation/_index.html#//apple_ref/doc/uid/TP40012484
	/// WARNING: This class does NOT YET implement the signature or trust checks
	/// </summary>
	/// <remarks>
	/// The handling of sandbox vs production URLs replicates Apple's sample. It is not 
	/// very sophisticated - you need to update the URL yourself in VerifyPurchase()
	/// 
	/// Also, please excuse all the System.Json hacks, there's probably neater ways...
	/// </remarks>		
	public class VerificationController
	{
		const string ITC_CONTENT_PROVIDER_SHARED_SECRET = ""; // TODO: specify your own
		const string ITMS_PROD_VERIFY_RECEIPT_URL = "https://buy.itunes.apple.com/verifyReceipt";
		const string ITMS_SANDBOX_VERIFY_RECEIPT_URL = "https://sandbox.itunes.apple.com/verifyReceipt";
		const string KNOWN_TRANSACTIONS_KEY = @"knownIAPTransactions";

		static VerificationController singleton;
		public static VerificationController SharedInstance {
			get {
				if (singleton == null)
					singleton = new VerificationController();
				return singleton;
			}
		}

		private VerificationController ()
		{
			transactionsReceiptStorageDictionary = new NSMutableDictionary();
		}
		NSMutableDictionary transactionsReceiptStorageDictionary;

		/// <summary>
		/// Called once transaction gets to SKPaymentTransactionState.Purchased
		/// or SKPaymentTransactionState.Restored (StoreKit has called UpdatedTransactions)
		/// </summary>			
		/// <returns>
		/// <c>true</c>, if verification is okay, <c>false</c> if there was a problem.
		/// </returns>
		public bool VerifyPurchase (SKPaymentTransaction transaction) 
		{
			Console.WriteLine ("VerifyPurchase " + transaction.TransactionIdentifier);
			if (ITC_CONTENT_PROVIDER_SHARED_SECRET == "")
				throw new NotImplementedException("Shared secret has not been specified at ITC_CONTENT_PROVIDER_SHARED_SECRET");

			bool isOK = IsTransactionAndItsReceiptValid (transaction);
			if (!isOK) // something wrong with transaction
				return isOK;

			var jsonObjectString = EncodeBase64 (transaction.TransactionReceipt.ToString ());

			var payload = @"{""receipt-data"" : """+jsonObjectString+@""", ""password"" : """+ITC_CONTENT_PROVIDER_SHARED_SECRET+@"""}";

			// Use ITMS_SANDBOX_VERIFY_RECEIPT_URL while testing against the sandbox.
			var serverURL = ITMS_SANDBOX_VERIFY_RECEIPT_URL; //ITMS_PROD_VERIFY_RECEIPT_URL;

			Console.WriteLine ("VerifyPurchase payload " + payload);

			// using .NET WebClient rather than NSURLConnection, so no trust validation...
			WebClient client = new WebClient();
			// Earlier port used async, was hard to keep the SKPaymentTransaction around to call Finish on later...
			//client.UploadDataCompleted += DidReceiveData;
			//client.UploadDataAsync (new Uri(serverURL), null,System.Text.Encoding.UTF8.GetBytes (payload), null);
			try {
				// make it synchronous
				var response = client.UploadData (serverURL, System.Text.Encoding.UTF8.GetBytes (payload));
				// ...and wait...
				var responseString = System.Text.Encoding.UTF8.GetString(response);
				Console.WriteLine ("VerificationController response string:" +responseString);

				isOK = DoesTransactionInfoMatchReceipt (responseString);
			} catch (System.Net.WebException e) {
				Console.WriteLine ("VerifyPurchase failed" + e.Message);
				isOK = false;
			}

			return isOK;
		}
#region not used in this implementation
//		private void DidReceiveData (object sender, UploadDataCompletedEventArgs e)
//		{
//			Console.WriteLine ("DidReceiveData");
//			if (e.Result == null | e.Cancelled) {	
//				Console.WriteLine ("DidReceiveData failed " + e.Error.Message);
//			} else {
//				var responseString = System.Text.Encoding.UTF8.GetString(e.Result);
//				Console.WriteLine ("VerificationController-response string:" +responseString);
//				bool isOK = DoesTransactionInfoMatchReceipt (responseString);
//			}
//			// required: some sort of callback to the purchase manager
//		}
#endregion

		bool IsTransactionAndItsReceiptValid (SKPaymentTransaction transaction) 
		{
			if (!(transaction != null 
			      && transaction.TransactionReceipt != null 
			      && transaction.TransactionReceipt.Length > 0))
				return false; // transaction is not valid

			// Pull the purchase info out, and save it in the dictionary for 
			// later in the verification stage

			var receiptDict = JsonValue.Parse (transaction.TransactionReceipt.ToString ().Replace (" = ", " : ")); //HACK:
			var transactionPurchaseInfo = receiptDict["purchase-info"].ToString ();
			var decodedPurchaseInfo = DecodeBase64(transactionPurchaseInfo);
			var purchaseInfoDict = JsonValue.Parse (decodedPurchaseInfo.ToString ().Replace (" = ", " : ")); //HACK:

			var transactionId = purchaseInfoDict["transaction-id"].ToString ().Trim ('"'); //HACK:
			var purchaseDateString = purchaseInfoDict["purchase-date"].ToString ().Trim ('"'); //HACK:
			var signature = receiptDict["signature"].ToString ();

			Console.WriteLine ("IsTransactionAndItsReceiptValid? {0}, {1}",transactionId, purchaseDateString);

			var dateFormat = "yyyy-MM-dd HH:mm:ss GMT";
			purchaseDateString = purchaseDateString.Replace ("Etc/", "");
			var purchaseDate = DateTime.ParseExact (purchaseDateString, 
			                                        dateFormat, 
			                                        System.Globalization.CultureInfo.InvariantCulture);

			if (!IsTransactionIdUnique(transactionId))
				return false; // we've seen this transaction before

			// HACK: this hasn't been implemented in MonoTouch yet
			var result = CheckReceiptSecurity(transactionPurchaseInfo, signature, purchaseDate);
			if (!result) return false;

			if (!DoTransactionDetailsMatchPurchaseInfo(transaction, purchaseInfoDict))
				return false;

			// remember that we've seen it
			SaveTransactionId (transactionId);

			// save for future reference
			transactionsReceiptStorageDictionary.SetValueForKey(new NSString(purchaseInfoDict.ToString ()), 
			                                                    new NSString(transactionId));

			return true;
		}

		bool DoTransactionDetailsMatchPurchaseInfo (SKPaymentTransaction transaction, JsonValue purchaseInfoDict)
		{
			Console.WriteLine ("DoTransactionDetailsMatchPurchaseInfo " + transaction.TransactionIdentifier);
			if (transaction == null || purchaseInfoDict == null)
				return false;

			int failCount = 0;

			if (transaction.Payment.ProductIdentifier != purchaseInfoDict ["product-id"].ToString().Trim ('"')) //HACK:
				failCount++;

			if (transaction.TransactionIdentifier != purchaseInfoDict ["transaction-id"].ToString().Trim ('"')) //HACK:
				failCount++;

			// Optionally add more checks here...

			if (failCount > 0) {
				Console.WriteLine("IsTransactionIdUnique failed {0} tests", failCount);
				return false;
			}
			return true;
		}

		bool IsTransactionIdUnique (string transactionId)
		{
			Console.WriteLine ("IsTransactionIdUnique " + transactionId + " in NSUserDefaults");

			var defaults = NSUserDefaults.StandardUserDefaults;
			var transactionDictionary = new NSString (KNOWN_TRANSACTIONS_KEY);
			defaults.Synchronize ();

			if (defaults [transactionDictionary] == null) {
				var d = new NSMutableDictionary ();
				defaults.SetValueForKey (d, transactionDictionary);
				defaults.Synchronize ();
			}
			var t = defaults [KNOWN_TRANSACTIONS_KEY] as NSDictionary;
			if (t [transactionId] == null) {
				Console.WriteLine("IsTransactionIdUnique failed");
				return true;
			}
			return false;
		}

		void SaveTransactionId (string transactionId)
		{
			Console.WriteLine ("SaveTransactionId " + transactionId + " to NSUserDefaults");

			var defaults = NSUserDefaults.StandardUserDefaults;
			var transactionDictionary = KNOWN_TRANSACTIONS_KEY;
			var dictionary = NSMutableDictionary.FromDictionary (defaults [transactionDictionary] as NSDictionary);

			if (dictionary == null) {
				dictionary = NSMutableDictionary.FromObjectAndKey (new NSNumber (1), new NSString (transactionId));
			} else {
				dictionary.SetValueForKey (new NSNumber (1), new NSString (transactionId));
			}
			defaults[transactionDictionary] = dictionary;
			defaults.Synchronize ();
		}

		bool DoesTransactionInfoMatchReceipt (string receiptString)
		{
			Console.WriteLine ("DoesTransactionInfoMatchReceipt " + receiptString);

			var verifiedReceiptDictionary = JsonValue.Parse (receiptString);
			var status = verifiedReceiptDictionary ["status"].ToString ();

			if (status == null)
				return false;

			int verifyReceiptStatus = Convert.ToInt32 (status);
			if (verifyReceiptStatus != 0 && verifyReceiptStatus != 21006)
				return false; // 21006 = This receipt is valid but the subscription has expired.


			var verifiedReceiptReceiptDictionary = verifiedReceiptDictionary ["receipt"];
			var verifiedReceiptUniqueIdentifier = verifiedReceiptReceiptDictionary ["unique_identifier"];
			var transactionIdFromVerifiedReceipt = verifiedReceiptReceiptDictionary ["transaction_id"];

			var t = transactionsReceiptStorageDictionary [new NSString (transactionIdFromVerifiedReceipt)];
			var purchaseInfoFromTransaction = JsonValue.Parse (t.ToString ());
			if (purchaseInfoFromTransaction == null)
				return false; // didn't find a receipt to compare to

			// NOTE: Instead of counting errors you could just return early.
			int failCount = 0;

			if (verifiedReceiptReceiptDictionary ["bid"].ToString () != purchaseInfoFromTransaction ["bid"].ToString ())
				failCount++;

			if (verifiedReceiptReceiptDictionary ["product_id"].ToString () != purchaseInfoFromTransaction ["product-id"].ToString ())
				failCount++;

			if (verifiedReceiptReceiptDictionary ["quantity"].ToString () != purchaseInfoFromTransaction ["quantity"].ToString ())
				failCount++;

			if (verifiedReceiptReceiptDictionary ["item_id"].ToString () != purchaseInfoFromTransaction ["item-id"].ToString ())
				failCount++;

			if (MonoTouch.UIKit.UIDevice.CurrentDevice.RespondsToSelector (new MonoTouch.ObjCRuntime.Selector ("identifierForVendor"))) {
				// iOS 6?
				Console.WriteLine ("iOS6 NOT SUPPORTED AT THIS TIME");
//				var localIdentifier = MonoTouch.UIKit.UIDevice.CurrentDevice.VendorIdentifier.UUID; // ???
//				var purchaseInfoUniqueVendorId = purchaseInfoFromTransaction["unique-vendor-identifier"].ToString();
//				var verifiedReceiptVendorIdentifier = verifiedReceiptReceiptDictionary["unique_vendor_identifier"];
//
//				if (verifiedReceiptVendorIdentifier != null) {
//					if (purchaseInfoUniqueVendorId != verifiedReceiptVendorIdentifier.ToString ()
//					    || purchaseInfoUniqueVendorId != localIdentifier)
//						failCount++; // comment this line out to test in the Simulator
//				}
			} else {
				// Pre iOS 6
				var localIdentifier = MonoTouch.UIKit.UIDevice.CurrentDevice.UniqueIdentifier;
				var purchaseInfoUniqueId = purchaseInfoFromTransaction ["unique-identifier"].ToString ().Trim ('"'); //HACK:

				if (purchaseInfoUniqueId != verifiedReceiptUniqueIdentifier
					|| purchaseInfoUniqueId != localIdentifier)
					failCount++; // comment this line out to test in the Simulator
			}

			if (failCount > 0) {
				Console.WriteLine("DoesTransactionInfoMatchReceipt failed {0} tests", failCount);
				return false;
			}
			return true;
		}

		string EncodeBase64 (string toEncode)
		{
			byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
			string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
			return returnValue;
		}
		string DecodeBase64 (string encodedData) 
		{
			encodedData = encodedData.Trim ('"');
			byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
			string returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
			return returnValue;
		}

		bool CheckReceiptSecurity (string purchaseInfoString, string signature, DateTime purchaseDate)
		{
			//HACK: haven't ported this stuff, so probably not as secure as it should be
			Console.WriteLine ("---------------------------------------");
			Console.WriteLine ("CheckReceiptSecurity is not implemented");
			Console.WriteLine ("---------------------------------------");
			return true;
		}
	}
}