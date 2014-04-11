In-App Purchase samples for Xamarin.iOS
=======================================

This sample contains THREE projects, a new (April 2014) sample for the Xamarin In App Purchase Component, and two older samples: one that demonstrates Consumable purchases and one that demonstrates NonConsumable purchases.

Component Sample
----------------

Xamarin has just (April 2014) released an [In App Purchase Component](https://components.xamarin.com/view/xamarin.inapppurchase) that helps to implement purchasing in iOS.

The code in the **ComponentSample** folder (OneCoolThing app) contains a minimal example of using In App Purchase to unlock a "feature" using the Non-Consumable product type. It has a single 'buy' button and alse implements 'restore'.

![screenshot](https://github.com/conceptdev/xamarin-samples/raw/master/InAppPurchase/ComponentSample/Screenshots/buy_all_sml.png "Component Sample ")

A lot of the complexity of the StoreKit APIs is wrapped in the component API - just wire up a few event handlers and the component code will help manage the purchasing process **and** keeping track of purchases in a secure manner.



Old samples (using StoreKit APIs directly)
---------

The older samples implement the StoreKit APIs directly

NOTE: it does NOT YET demonstrate RECEIPT VERIFICATION, so you'll have to add this in yourself. 

![screenshot](https://github.com/conceptdev/xamarin-samples/raw/master/InAppPurchase/Screenshots/NonConsumable.png "NonConsumable") ![screenshot](https://github.com/conceptdev/xamarin-samples/raw/master/InAppPurchase/Screenshots/Consumable.png "Consumable") 

You might also consider services like http://urbanairship.com/ or http://www.beeblex.com/ (although I have not tried them, so can't recommend).

Check out @redth's server-side code to help build your own receipt verification logic with ASP.NET:

[https://github.com/Redth/APNS-Sharp/tree/master/JdSoft.Apple.AppStore](https://github.com/Redth/APNS-Sharp/tree/master/JdSoft.Apple.AppStore)

FYI my sample code is based in-part on @jtclancey's AppStore code here: 

[https://github.com/Clancey/ClanceyLib](https://github.com/Clancey/ClanceyLib)



Setup
-----

There's a bit of set-up required for In-App Purchases (registering your bank details with Apple, setting up the products in the iOS Developer Portal, Provisioning your app correctly). These steps are the same for MonoTouch and Objective-C, so Apple's setup doco might help [1]. You should also read Apple's In-App Purchase programming docs [2], for familiarity.

[1] [http://developer.apple.com/library/ios/#technotes/tn2259/_index.html](http://developer.apple.com/library/ios/#technotes/tn2259/_index.html)

[2] [https://developer.apple.com/library/ios/#documentation/NetworkingInternet/Conceptual/StoreKitGuide/Introduction/Introduction.html](https://developer.apple.com/library/ios/#documentation/NetworkingInternet/Conceptual/StoreKitGuide/Introduction/Introduction.html)