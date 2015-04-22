using System;

using WatchKit;
using Foundation;

namespace Watch8BallExtension
{
	public partial class InterfaceController : WKInterfaceController
	{
		public InterfaceController (IntPtr handle) : base (handle)
		{
		}

		string lastResult = "";

		public override void Awake (NSObject context)
		{
			base.Awake (context);

			// Configure interface objects here.
			Console.WriteLine ("{0} awake with context", this);

			var rnd = new System.Random();
			lastResult = options[rnd.Next(0, options.Length - 1)];
			NSUserDefaults.StandardUserDefaults.SetString (lastResult, "lastResult");

			AddMenuItem (WKMenuItemIcon.Accept, "Thanks", new ObjCRuntime.Selector ("tapped"));
		}

		public override void WillActivate ()
		{
			// This method is called when the watch view controller is about to be visible to the user.
			Console.WriteLine ("{0} will activate", this);

			result.SetText (lastResult);
		}
		[Export("tapped")]
		void MenuItemTapped ()
		{
			result.SetText ("You're welcome!");
			Console.WriteLine ("A menu item was tapped.");
		}
		public override void DidDeactivate ()
		{
			// This method is called when the watch view controller is no longer visible to the user.
			Console.WriteLine ("{0} did deactivate", this);
		}

		partial void shake () {
			var rnd = new System.Random();
			lastResult = options[rnd.Next(0, options.Length - 1)];
			result.SetText (lastResult);
			NSUserDefaults.StandardUserDefaults.SetString (lastResult, "lastResult");
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
	}
}

