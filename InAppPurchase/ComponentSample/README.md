In-App Purchase Xamarin.iOS Component Sample
============================================

Xamarin has just (April 2014) released an [In App Purchase Component](https://components.xamarin.com/view/xamarin.inapppurchase) that helps to implement purchasing in iOS.

The code (OneCoolThing app) contains a minimal example of using In App Purchase to unlock a "feature" using the Non-Consumable product type. It has a single 'buy' button and alse implements 'restore'.

![screenshot](https://github.com/conceptdev/xamarin-samples/raw/master/InAppPurchase/ComponentSample/Screenshots/buy_all_sml.png "Component Sample ")

A lot of the complexity of the StoreKit APIs is wrapped in the component API - just wire up a few event handlers and the component code will help manage the purchasing process **and** keeping track of purchases in a secure manner.

The steps to build this sample were:

1. Start by visiting [iTunes Connect](https://itunesconnect.apple.com) 

	1.1 Create a new iOS Application

	1.2 Create a new In App Purchase Product
	
	1.3 Create test-users (in the *Manage Users* section)

2. Create a Provisioning Profile

	2.1 You need a provisioning profile specific to the Bundle ID for this app - you can't use the shared Team Profile for apps that use In App Purchase.

3. Create a new iOS "Single View" Application

	3.1 Add the **iOS In-App Purchase Component**
	
	3.2 Implement the `InAppPurchaseManager` class
	
4. In App Purchase apps should be tested on real devices. 
**BEFORE YOU START go to Settings > iTunes & App Store > Apple ID and *log out* of any existing Apple ID. REAL Apple IDs cannot be used to test In App Purchases that are not live. **
	
    4.1 Start debugging the app - when you touch **Buy** login to the App Store using the *test* account created in step 1.3 (NOT a real App Store account)
   