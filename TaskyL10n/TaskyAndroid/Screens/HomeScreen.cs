using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.BL;
using Android.Graphics;
using Android.Views;

using Tasky.BL.Managers;
using Tasky.DL.SQLite;
using Android.Content.PM;
using System;

namespace TaskyAndroid.Screens {
	[Activity (Label = "@string/app_name", MainLauncher = true, Icon="@drawable/launcher",
		ConfigurationChanges =  ConfigChanges.Orientation | ConfigChanges.Locale)]			
	public class HomeScreen : Activity {
		protected Adapters.TaskListAdapter taskList;
		protected IList<TodoItem> tasks;
		protected Button addTaskButton = null;
		protected ListView taskListView = null;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			

			View titleView = Window.FindViewById(Android.Resource.Id.Title);
			if (titleView != null) {
			  IViewParent parent = titleView.Parent;
			  if (parent != null && (parent is View)) {
			    View parentView = (View)parent;
			    parentView.SetBackgroundColor(Color.Rgb(0x26, 0x75 ,0xFF)); //38, 117 ,255
			  }
			}


			// set our layout to be the home screen
			SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			taskListView = FindViewById<ListView> (Resource.Id.lstTasks);
			addTaskButton = FindViewById<Button> (Resource.Id.btnAddTask);

			// wire up add task button handler
			if(addTaskButton != null) {
				addTaskButton.Click += (sender, e) => {
					StartActivity(typeof(TaskDetailsScreen));
				};
			}
			
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					taskDetails.PutExtra ("TaskID", tasks[e.Position].ID);
					StartActivity (taskDetails);
				};
			}

			// HACK: just testing
			var taskcount = 0;
			var quant = Resources.GetQuantityString (Resource.Plurals.numberOfTasks, taskcount, taskcount + 1);
			Console.WriteLine ("quant0: " + quant);
			taskcount = 1;
			quant = Resources.GetQuantityString (Resource.Plurals.numberOfTasks, taskcount, taskcount + 1);
			Console.WriteLine ("quant1: " + quant);
			taskcount = 2;
			quant = Resources.GetQuantityString (Resource.Plurals.numberOfTasks, taskcount, taskcount + 1);
			Console.WriteLine ("quant2: " + quant);
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();



			var sqliteFilename = "TaskDB.db3";
			// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
			// (they don't want non-user-generated data in Documents)
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); // Documents folder
			var path = System.IO.Path.Combine(documentsPath, sqliteFilename);
			var conn = new Connection(path);
			var TaskMgr = new TodoManager(conn);
			tasks = TaskMgr.GetTodos();
			
			// create our adapter
			taskList = new Adapters.TaskListAdapter(this, tasks);

			//Hook up our adapter to our ListView
			taskListView.Adapter = taskList;
		}
	}
}