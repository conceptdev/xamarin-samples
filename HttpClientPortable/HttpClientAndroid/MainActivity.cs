using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using HttpPortable;

namespace HttpClient
{
	[Activity (Label = "HttpClientAndroid", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public const string WisdomUrl = "http://httpbin.org/ip";
		public const string SecureUrl = "https://gmail.com";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.button);
			Button button1 = FindViewById<Button> (Resource.Id.button1);
			EditText output = FindViewById<EditText> (Resource.Id.Output);

			button.Click += async delegate {

				var r = new Renderer(this, output);
				var nh = new NetHttp(r);
				await nh.HttpSample (WisdomUrl);

			};

			button1.Click += async delegate {

				var r = new Renderer(this, output);
				var nh = new NetHttp(r);
				await nh.HttpSample (SecureUrl);

			};
		}
	}
}


