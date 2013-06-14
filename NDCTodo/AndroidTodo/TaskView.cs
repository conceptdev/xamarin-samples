using System;
using Android.Widget;
using Android.App;

namespace NDCTodo
{
	[Activity (Label = "Task")]	
	public class TaskView : Activity
	{
		Task currentTask;
		TaskViewModel vm;

		EditText titleText;
		CheckBox doneCheckBox;
		Button saveButton;

		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			vm = new TaskViewModel ();

			int taskID = Intent.GetIntExtra("TaskId", 0);
			if (taskID > 0) {
				currentTask = vm.Get (taskID);
			} else {
				currentTask = new Task ();
			}


			SetContentView(Resource.Layout.TaskView);

			titleText = FindViewById<EditText>(Resource.Id.TitleText);
			doneCheckBox = FindViewById<CheckBox>(Resource.Id.DoneCheckBox);

			titleText.Text = currentTask.Title;
			doneCheckBox.Checked = currentTask.Done;

			saveButton = FindViewById<Button>(Resource.Id.SaveButton);

			saveButton.Click += (sender, e) => {
				currentTask.Title = titleText.Text;
				currentTask.Done = doneCheckBox.Checked;

				currentTask.Id = vm.Save (currentTask); // unnecessary

				Finish();
			};


			var cancelButton = FindViewById<Button> (Resource.Id.CancelButton);
			cancelButton.Click += (sender, e) => {
				Finish();
			};
		}
	}
}

