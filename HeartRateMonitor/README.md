HeartRateMonitor
===========

iOS example based on the [Xamarin.Mac HeartRateMonitor](https://github.com/xamarin/mac-samples/tree/master/HeartRateMonitor) by [abock](https://github.com/abock).

The `HeartBeat`, `HeartBeatEventArgs`, `HeartRateMonitorLocation` classes are unchanged. The `HeartRateMonitor` class only required namespace changes and the removal of `unsafe` code. The `CoreBluetooth` APIs work identically across OS X and iOS.

It has been modified from the Xamarin.Mac sample in the following ways:

* A list of heart rate monitor devices is no longer presented; the code automatically chooses the first device it 'finds'.

* The heart-beat animation has been removed.

A screenshot of the app running is shown below:

![screenshot](https://raw.githubusercontent.com/conceptdev/xamarin-samples/master/HeartRateMonitor/Screenshots/HeartRateMonitor-sml.png "Heart Rate Monitor")

Authors
-------

Aaron Bockover, Craig Dunn
