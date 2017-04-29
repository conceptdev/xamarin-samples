using System;
using System.Collections.Generic;
using UIKit;

namespace NDCTodo
{
	public class TaskDataSource : UITableViewSource
	{
		IList<Task> taskList;
		public TaskDataSource (IList<Task> tasks) {
			taskList = tasks;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return taskList.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
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

