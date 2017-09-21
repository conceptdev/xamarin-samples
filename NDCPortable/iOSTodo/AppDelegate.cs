using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using System.IO;
using NDCPortable;

namespace iOSTodo
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public static AppDelegate Current { get; private set; }
		public TodoItemManager TaskMgr { get; set; }
		
		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			Current = this;

            // TODO: pick which storage library you wish to use...

			// LIST
//			TaskMgr = new TodoItemManager();

			// XML
			var sqliteFilename = "TodoList.xml";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..","Library"); // Library folder
			var path = Path.Combine(libraryPath, sqliteFilename);
			var xmlStorage = new XmlStorageImplementation ();
			TaskMgr = new TodoItemManager(path, xmlStorage);

            // ADO.NET
            //var sqliteFilename = "TodoADODB.db3";
            //string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            //string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            //var path = Path.Combine(libraryPath, sqliteFilename);
            //var db = new ADODatabase(path);
            //TaskMgr = new TodoItemManager(db);

			// SQLite
            //var sqliteFilename = "TodoSQLiteDB.db3";
            //string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
            //string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
            //var path = Path.Combine(libraryPath, sqliteFilename);
            //var conn = new Connection(path);
            //TaskMgr = new TodoItemManager(conn);


			return true;
		}
		//
		// This method is invoked when the application is about to move from active to inactive state.
		//
		// OpenGL applications should use this method to pause.
		//
		public override void OnResignActivation (UIApplication application)
		{
		}
		
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		
		// This method is called when the application is about to terminate. Save data, if needed. 
		public override void WillTerminate (UIApplication application)
		{
		}
	}

    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}

