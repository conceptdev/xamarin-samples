using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using AzurePortable;
using Microsoft.WindowsAzure.MobileServices;

namespace iOSTodo
{
	public partial class RootViewController : UITableViewController
	{
		List<TodoItem> todoItems;

		public RootViewController (IntPtr handle) : base (handle)
		{
			Title = "AzureTodo";
		}

		/// <summary>
		/// Prepares for segue.
		/// </summary>
		/// <remarks>
		/// The prepareForSegue method is invoked whenever a segue is about to take place. 
		/// The new view controller has been loaded from the storyboard at this point but 
        /// it’s not visible yet, and we can use this opportunity to send data to it.
		/// </remarks>
		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "TaskSegue") { // set in Storyboard
				var navctlr = segue.DestinationViewController as TaskDetailViewController;
				if (navctlr != null) {
					var source = TableView.Source as RootTableSource;
					var rowPath = TableView.IndexPathForSelectedRow;
					var item = source.GetItem(rowPath.Row);
					navctlr.SetTask(item);
				}
			}
		}

		public void CreateTask () {
			// first, add the task to the underlying data
			var newTodo = new TodoItem();

			// then open the detail view to edit it
			var detail = Storyboard.InstantiateViewController("detail") as TaskDetailViewController;
			detail.SetTask (newTodo);
			NavigationController.PushViewController (detail, true);

			// Could to this instead of the above, but need to create 'new Task()' in PrepareForSegue()
			//this.PerformSegue ("TaskSegue", this);
		}



		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		#region View lifecycle
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			AddButton.Clicked += (sender, e) => {
				CreateTask ();
			};
		}

		public async override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			// HACK: todoItems = AppDelegate.Current.TaskMgr.GetTasks ();

			// NO AUTH
			todoItems = await AppDelegate.Current.TaskMgr.GetTasksAsync ();
			TableView.Source = new RootTableSource (todoItems.ToArray ());
			TableView.ReloadData ();

			// AUTH
//			if (AzureStorageImplementation.DefaultService.User == null)
//				await AzureStorageImplementation.DefaultService.Authenticate (this);
//
//			if (AzureStorageImplementation.DefaultService.User != null) {
//				Console.WriteLine ("Logged in user: " + AzureStorageImplementation.DefaultService.User.UserId);
//				todoItems = await AppDelegate.Current.TaskMgr.GetTasksAsync ();
//				TableView.Source = new RootTableSource (todoItems.ToArray ());
//				TableView.ReloadData ();
//			} else {
//				Console.WriteLine ("Didn't log in");
//			}

		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		
		#endregion
	}
}

