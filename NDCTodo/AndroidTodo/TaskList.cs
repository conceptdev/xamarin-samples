using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace NDCTodo
{
	[Activity (Label = "NDCTodo", MainLauncher = true, Icon="@drawable/Icon")]
	public class TaskListActivity : Activity
	{
		IList<Task> tasks;
		TaskListAdapter adapter;
		TaskViewModel vm;

		Button addButton;
		ListView taskList;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.TaskList);

			addButton = FindViewById<Button> (Resource.Id.AddButton);
			addButton.Click += (sender, e) => {
				StartActivity(typeof(TaskView));
			};

			taskList = FindViewById<ListView> (Resource.Id.TaskList);
			taskList.ItemClick += (sender, e) => {
				var taskDetails = new Intent (this, typeof (TaskView));
				taskDetails.PutExtra ("TaskId", tasks[e.Position].Id);
				StartActivity (taskDetails);
			};

			vm = new TaskViewModel ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			tasks = vm.GetAll();

			// create our adapter
			adapter = new TaskListAdapter(this, tasks);

			//Hook up our adapter to our ListView
			taskList.Adapter = adapter;
		}
	}
}


