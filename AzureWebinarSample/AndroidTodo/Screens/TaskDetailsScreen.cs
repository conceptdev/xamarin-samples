using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using AzurePortable;

namespace AndroidTodo {

	[Activity (Label = "Task Details", Icon="@drawable/ic_launcher", Theme = "@style/AppTheme")]			
	public class TaskDetailsScreen : Activity {
		protected TodoItem task = new TodoItem();
		protected Button cancelDeleteButton = null;
		protected EditText notesTextEdit = null;
		protected EditText nameTextEdit = null;
		protected Button saveButton = null;
		CheckBox doneCheckbox;
		
		protected async override void OnCreate (Bundle bundle)
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
			SetContentView(Resource.Layout.TaskDetails);
			nameTextEdit = FindViewById<EditText>(Resource.Id.txtName);
			notesTextEdit = FindViewById<EditText>(Resource.Id.txtNotes);
			saveButton = FindViewById<Button>(Resource.Id.btnSave);
			doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			cancelDeleteButton = FindViewById<Button>(Resource.Id.btnCancelDelete);

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };

			string taskID = Intent.GetStringExtra("TaskID");
			if(!String.IsNullOrEmpty(taskID)) {
				//HACK: task = AppDelegate.Current.TaskMgr.GetTask(taskID);
				task = await AppDelegate.Current.TaskMgr.GetTaskAsync(taskID);
			}

			// set the cancel delete based on whether or not it's an existing task
			cancelDeleteButton.Text = (String.IsNullOrEmpty(task.ID) ? "Cancel" : "Delete");

			nameTextEdit.Text = task.Name;
			notesTextEdit.Text = task.Notes;
			doneCheckbox.Checked = task.Done;
		}

		async void Save()
		{
			task.Name = nameTextEdit.Text;
			task.Notes = notesTextEdit.Text;
			task.Done = doneCheckbox.Checked;
			//HACK: AppDelegate.Current.TaskMgr.SaveTask(task);
			await AppDelegate.Current.TaskMgr.SaveTaskAsync(task);

//				try 
//				{
//					await AppDelegate.Current.TaskMgr.SaveTodoItemAsync(currentTodoItem);
//				} 
//				catch (MobileServiceInvalidOperationException ioe)
//				{
//					if (ioe.Response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
//						// configured in portal
//						CreateAndShowDialog (ioe.Message, "Invalid");
//					} else {
//						// another error that we are not expecting
//						CreateAndShowDialog (ioe.Response.Content.ToString (), "Error");
//					}
//				}

			Finish(); // pop off history
		}
		
		async void CancelDelete()
		{
			if(!String.IsNullOrEmpty(task.ID)) {
				//HACK: AppDelegate.Current.TaskMgr.DeleteTask(task);
				await AppDelegate.Current.TaskMgr.DeleteTaskAsync(task);
			}
			Finish();
		}

		void CreateAndShowDialog(string message, string title)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(this);

			builder.SetMessage(message);
			builder.SetTitle(title);
			builder.Create().Show();
		}
	}
}