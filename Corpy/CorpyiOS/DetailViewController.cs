using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Twitter;

namespace Corpy
{
	public partial class DetailViewController : UIViewController
	{
		Employee detailItem;
		
		public Employee DetailItem {
			get {
				return detailItem;
			}
			set {
				detailItem = value;
				if (IsViewLoaded)
					ConfigureView ();
			}
		}
		
		public DetailViewController (IntPtr handle) : base (handle)
		{
			this.Title = NSBundle.MainBundle.LocalizedString ("Detail", "Detail");
		}

		[Action("actionTakeAPicture:withEvent:")]
		public void actionTakeAPicture (NSObject sender, MonoTouch.UIKit.UIEvent @event) {
			Console.WriteLine ("actionTakeAPicture");
			var u = new NSUrl("tel:" + DetailItem.Mobile);
			UIApplication.SharedApplication.OpenUrl (u);
		}

		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (DetailItem != null) {
				Title = DetailItem.NameFormatted;

				NameLabel.Text = DetailItem.NameFormatted;
				DepartmentLabel.Text = DetailItem.Department;
				ShowMapButton.Alpha = 0f;
				ShowMapButton.TouchUpInside += (sender, e) => {
					// Address has been hardcoded 
					var u = new NSUrl("http://maps.google.com/maps?q=" + "401 Van Ness Ave, San Francisco, USA".Replace(" ","%20"));
					UIApplication.SharedApplication.OpenUrl (u);
				};
				CallCellButton.TouchUpInside += (sender, e) => {
					var u = new NSUrl("tel:" + DetailItem.Mobile);
					UIApplication.SharedApplication.OpenUrl (u);
				};

				CallWorkButton.TouchUpInside += (sender, e) => {
					var u = new NSUrl("tel:" + DetailItem.Work);
					UIApplication.SharedApplication.OpenUrl (u);
				};
				EmailButton.TouchUpInside += (sender, e) => {
					var u = new NSUrl("mailto:" + DetailItem.Email);
					UIApplication.SharedApplication.OpenUrl (u);
				};
				TweetButton.TouchUpInside += (sender, e) => {
					if (TWTweetComposeViewController.CanSendTweet) {
						var tvc = new TWTweetComposeViewController();
						tvc.SetInitialText("@" + DetailItem.Firstname + " ");
						this.PresentModalViewController(tvc, true);
					}
				};
			}
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		#region View lifecycle
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		
		#endregion
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

