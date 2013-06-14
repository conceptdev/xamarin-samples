using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace StoryboardTables
{
	public class TaskDataSource : UITableViewSource
	{
		IList<Task> taskList;
		public TaskDataSource (IList<Task> tasks) {
			taskList = tasks;
		}

		public override int RowsInSection (UITableView tableview, int section) {
			return taskList.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("taskcell");

			cell.TextLabel.Text = taskList [indexPath.Row].Title;

			if (taskList [indexPath.Row].Done)
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			else
				cell.Accessory = UITableViewCellAccessory.None;

			return cell;
		}

		
		public Task GetItem(int id) {
			return taskList[id];
		}
	}
}

