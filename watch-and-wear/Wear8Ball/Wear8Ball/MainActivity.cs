using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;

using Export = Java.Interop.ExportAttribute;

namespace wear8ball
{
	[Activity (Label = "Wear8Ball", MainLauncher = true, Icon = "@drawable/ic_launcher")]
	public class MainActivity : Activity
	{
		Button button;
		TextView result;
		string lastResult = "";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.main_activity);

			button = FindViewById<Button> (Resource.Id.shake);
			result = FindViewById<TextView> (Resource.Id.result);

			button.Click += (sender, e) => {
				var rnd = new System.Random();
				lastResult = options[rnd.Next(0, options.Length - 1)];

				result.Text = lastResult;
			};
		}

		string[] options = {
			"It is certain"
			, "It is decidedly so"
			, "Without a doubt"
			, "Yes definitely"
			, "You may rely on it"
			, "As I see it, yes"
			, "Most likely"
			, "Outlook good"
			, "Yes"
			, "Signs point to yes"

			, "Reply hazy try again"
			, "Ask again later"
			, "Better not tell you now"
			, "Cannot predict now"
			, "Concentrate and ask again"

			, "Don't count on it"
			, "My reply is no"
			, "My sources say no"
			, "Outlook not so good"
			, "Very doubtful"
		};
	

	/**
	 * Handles the button press to finish this activity and take the user back to the Home.
	 */
		[Export ("onFinishActivity")]
		public void OnFinishActivity (View v)
		{
			SetResult (Result.Ok);
			Finish ();
		}
	}
}