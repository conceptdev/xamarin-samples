using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using NDCPortable;
using AndroidTodo;
using System;

namespace AndroidTodo {
	[Activity (Label = "NDCTodo", MainLauncher = true, Icon="@drawable/ic_launcher", Theme = "@style/AppTheme")]			
	public class HomeScreen : Activity {
		protected TodoItemListAdapter todoList;
		protected IList<TodoItem> todoItems;
		protected Button addTaskButton = null;
		protected ListView todoListView = null;
		
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
			todoListView = FindViewById<ListView> (Resource.Id.lstTasks);
			addTaskButton = FindViewById<Button> (Resource.Id.btnAddTask);

			// wire up add task button handler
			if(addTaskButton != null) {
				addTaskButton.Click += (sender, e) => {
					StartActivity(typeof(TaskDetailsScreen));
				};
			}
			
			// wire up task click handler
			if(todoListView != null) {
				todoListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					taskDetails.PutExtra ("TaskID", todoItems[e.Position].ID);
					StartActivity (taskDetails);
				};
			}


		}
		
		protected async override void OnResume ()
		{
			base.OnResume ();

			// NO AUTH
			//HACK: tasks = AppDelegate.Current.TaskMgr.GetTasks();
			todoItems = await AppDelegate.Current.TaskMgr.GetTasksAsync ();
			todoList = new TodoItemListAdapter(this, todoItems);
			todoListView.Adapter = todoList;

			// AUTH
//			if (AzureStorageImplementation.DefaultService.User == null)
//				await AzureStorageImplementation.DefaultService.Authenticate (this);
//
//			if (AzureStorageImplementation.DefaultService.User != null) {
//				Console.WriteLine ("Logged in user: " + AzureStorageImplementation.DefaultService.User.UserId);
//				todoItems = await AppDelegate.Current.TaskMgr.GetTasksAsync (); //AzureStorageImplementation.DefaultService.RefreshDataAsync ();
//				todoList = new TodoItemListAdapter(this, todoItems);
//				todoListView.Adapter = todoList;
//			} else {
//				Console.WriteLine ("Didn't log in");
//			}
		}
	}
}