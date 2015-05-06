using System;

using Xamarin.Forms;

namespace Teal
{
	public class App : Application
	{
		// http://blogs.msdn.com/b/windowsazurestorage/archive/2012/06/12/introducing-table-sas-shared-access-signature-queue-sas-and-update-to-blob-sas.aspx
		// http://www.dotnetcurry.com/showarticle.aspx?ID=901

		public App ()
		{
			// The root page of your application
			var tabs = new TabbedPage ();
			tabs.Children.Add (new Teal.Blob {Title = "Blob", Icon="glyphish_56_cloud" });
			tabs.Children.Add (new Teal.Table {Title = "Table",  Icon="glyphish_33_cabinet" });
			tabs.Children.Add (new Teal.Queue {Title = "Queue", Icon="glyphish_104_index_cards"});
			MainPage = tabs;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

