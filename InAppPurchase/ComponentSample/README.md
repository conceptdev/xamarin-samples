In-App Purchase sample for MonoTouch
====================================

This sample contains two projects, one that demonstrates Consumable purchases and one that demonstrates NonConsumable purchases.

NOTE: it does NOT YET demonstrate RECEIPT VERIFICATION, so you'll have to add this in yourself. 

![screenshot](https://github.com/conceptdev/xamarin-samples/raw/master/InAppPurchase/Screenshots/NonConsumable.png "NonConsumable") ![screenshot](https://github.com/conceptdev/xamarin-samples/raw/master/InAppPurchase/Screenshots/Consumable.png "Consumable") 

You might also consider services like http://urbanairship.com/ or http://www.beeblex.com/ (although I have not tried them, so can't recommend).

Check out @redth's server-side code to help build your own receipt verification logic with ASP.NET:

https://github.com/Redth/APNS-Sharp/tree/master/JdSoft.Apple.AppStore

FYI my sample code is based in-part on @jtclancey's AppStore code here: 

https://github.com/Clancey/ClanceyLib



Setup
-----

There's a bit of set-up required for In-App Purchases (registering your bank details with Apple, setting up the products in the iOS Developer Portal, Provisioning your app correctly). These steps are the same for MonoTouch and Objective-C, so Apple's setup doco might help [1]. You should also read Apple's In-App Purchase programming docs [2], for familiarity.

[1] http://developer.apple.com/library/ios/#technotes/tn2259/_index.html

[2] https://developer.apple.com/library/ios/#documentation/NetworkingInternet/Conceptual/StoreKitGuide/Introduction/Introduction.html