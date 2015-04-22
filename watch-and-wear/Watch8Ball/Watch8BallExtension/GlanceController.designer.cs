// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Watch8BallExtension
{
	[Register ("GlanceController")]
	partial class GlanceController
	{
		[Outlet]
		WatchKit.WKInterfaceLabel lastResult { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lastResult != null) {
				lastResult.Dispose ();
				lastResult = null;
			}
		}
	}
}
