using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using System.IO;
using AndroidTodo;
using NDCPortable;

namespace AndroidTodo {
    [Application]
	public class AppDelegate : Application {

		public static AppDelegate Current { get; private set; }

		public TodoItemManager TaskMgr { get; set; }

		public AppDelegate(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer) {
                Current = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();

			// List
//			TaskMgr = new TodoItemManager();

			// XML
//			var sqliteFilename = "TaskDB.xml";
//			string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//			var path = Path.Combine(libraryPath, sqliteFilename);
//			var xmlStorage = new AndroidTodo.XmlStorageImplementation ();
//			TaskMgr = new TodoItemManager(path, xmlStorage);

			// SQL
//			var sqliteFilename = "TaskDB.db3";
//			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
//			var path = Path.Combine(documentsPath, sqliteFilename);
//			var conn = new Connection(path);
//			TaskMgr = new TodoItemManager(conn);

			// AZURE
			TaskMgr = new TodoItemManager(AzureStorageImplementation.DefaultService);

        }
    }
}
