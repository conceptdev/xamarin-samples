HeartRateMonitor
===========

iOS example based on the [Xamarin.Mac HeartRateMonitor](https://github.com/xamarin/mac-samples/tree/master/HeartRateMonitor) by [abock](https://github.com/abock).

The `HeartBeat`, `HeartBeatEventArgs`, `HeartRateMonitorLocation` classes are unchanged. The `HeartRateMonitor` class only required namespace changes. The `CoreBluetooth` APIs work identically across OS X and iOS.

It has been modified from the Xamarin.Mac sample in the following ways:

* A list of heart rate monitor devices is no longer presented; the code automatically chooses the first device it 'finds'.

* The heart-beat animation hasn't been converted yet (I hope to add the animation to this sample soon).

* I've commented out some `unsafe` code from the Mac version. This code still works in iOS (if you 'tick' **Project > Options > Build > General > Allow 'unsafe' code**) but because many .NET devs aren't familiar with this I've replaced it with a safe version. If you're wondering why you'd use the `unsafe` version, Aaron says:

>(generally) alloc + copy is expensive. You're allocating that array on the heap each time, then copying memory to it, then reading it *vs* just reading the original memory. The unsafe dereference/read avoids both, which can be critical in fast paths


A screenshot of the app running is shown below:

![screenshot](https://raw.githubusercontent.com/conceptdev/xamarin-samples/master/HeartRateMonitor/Screenshots/HeartRateMonitor-sml.png "Heart Rate Monitor")

Authors
-------

Aaron Bockover, Craig Dunn
