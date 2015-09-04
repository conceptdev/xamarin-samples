using System;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace StoryboardTables
{
	public partial class RootViewController : UITableViewController
	{
		// The list of tasks is NOT persisted, even though you can add and delete tasks
		// in this sample, the changes are only in memory and will disappear when the app restarts
		List<Task> tasks;

		public RootViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Todo", "");
			
			// Custom initialization
			tasks = new List<Task> {
					new Task() {Name="Groceries", Notes="Buy bread, cheese, apples", Done=false},
					new Task() {Name="Devices", Notes="Buy Nexus, Galaxy, Droid", Done=false}
			};

			if (NSLocale.PreferredLanguages.Length > 0) {
				var pref = NSLocale.PreferredLanguages [0];
				Console.WriteLine ("preferred-language:" + pref);
			}

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
					navctlr.SetTask(this, item);
				}
			}
		}

		public void CreateTask () {
			// first, add the task to the underlying data
			var newId = tasks[tasks.Count - 1].Id + 1;
			var newTask = new Task(){Id=newId};
			tasks.Add (newTask);
			// then open the detail view to edit it
			var detail = Storyboard.InstantiateViewController("detail") as TaskDetailViewController;
			detail.SetTask (this, newTask);
			NavigationController.PushViewController (detail, true);

			// Could to this instead of the above, but need to create 'new Task()' in PrepareForSegue()
			//this.PerformSegue ("TaskSegue", this);
		}
		public void SaveTask (Task task) {
			Console.WriteLine("Save "+task.Name);
			var oldTask = tasks.Find(t => t.Id == task.Id);
			oldTask = task;
			NavigationController.PopViewController(true);
		}
		public void DeleteTask (Task task) {
			Console.WriteLine("Delete "+task.Name);
			var oldTask = tasks.Find(t => t.Id == task.Id);
			tasks.Remove (oldTask);
			NavigationController.PopViewController(true);
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
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
//			ReleaseDesignerOutlets ();
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			// bind every time, to reflect deletion in the Detail view
			TableView.Source = new RootTableSource(tasks.ToArray ());
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

