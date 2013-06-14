using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace StoryboardTables
{
	public partial class TaskListViewController : UITableViewController
	{
		TaskViewModel vm;

		public TaskListViewController (IntPtr handle) : base (handle)
		{
			vm = new TaskViewModel ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			TableView.Source = new TaskDataSource (vm.GetAll ());
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "newtask") { // set in Storyboard
				var navctlr = segue.DestinationViewController as TaskViewController;
				if (navctlr != null) 
					navctlr.currentTask = new Task ();
			}
			if (segue.Identifier == "edittask") { // set in Storyboard
				var navctlr = segue.DestinationViewController as TaskViewController;
				if (navctlr != null) {
					var source = TableView.Source as TaskDataSource;
					var rowPath = TableView.IndexPathForSelectedRow;
					var item = source.GetItem(rowPath.Row);
					navctlr.currentTask = item;
				}
			}
		}
	}
}