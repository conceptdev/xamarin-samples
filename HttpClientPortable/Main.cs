//	
// This sample shows how to use the two Http stacks in MonoTouch:
// The System.Net.WebRequest.
// The MonoTouch.Foundation.NSMutableUrlRequest
//

using System;
using System.IO;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using HttpPortable;

namespace HttpClient
{
	public class Application
	{
		// URL where we fetch the wisdom from
		public const string WisdomUrl = "http://httpbin.org/ip";
        public const string SecureUrl = "https://gmail.com";

		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}

		public static void Busy ()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}
		
		public static void Done ()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;	
		}
			
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
        public UINavigationController NavigationController {
            get { return navigationController; }
        }
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window.AddSubview (navigationController.View);
			
			button1.TouchDown += Button1TouchDown;
			TableViewSelector.Configure (this.stack, new string [] {
				"http  - WebRequest",
				"https - WebRequest",
				"http  - HttpClient",
                "https  - HttpClient"

                //"http  - NSUrlConnection",
			});
			                   
			window.MakeKeyAndVisible ();
			
			return true;
		}

		async void Button1TouchDown (object sender, EventArgs e)
		{
            var r = new Renderer(this);

			// Do not queue more than one request
			if (UIApplication.SharedApplication.NetworkActivityIndicatorVisible)
				return;
		
			switch (stack.SelectedRow ()){
			case 0:
                new DotNet(r).HttpSample("https://gmail.com");
				break;
			
			case 1:
                new DotNet(r).HttpSecureSample(Application.WisdomUrl);
				break;
				
			case 2:
                var nh = new NetHttp(r);
				await nh.HttpSample (Application.WisdomUrl);
				break;

            case 3:
                var nhs = new NetHttp(r);
                await nhs.HttpSample(Application.SecureUrl);
                break;
            //case 3:
            //    //				new Cocoa (this).HttpSample ();
            //    break;
			}
		}
		
		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{

		}
	}
}
