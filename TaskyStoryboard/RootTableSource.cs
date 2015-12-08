using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UIKit;

namespace StoryboardTables {
	public class RootTableSource : UITableViewSource {
		// ##
		// there is NO database or storage of Tasks in this example, just an in-memory List<>
		// refer to the other Tasky samples on github for an implementation using SQLite-NET
		// ##
		Task[] tableItems;
	    string cellIdentifier = "taskcell";
	 
		public RootTableSource (Task[] items)
		{
			tableItems = items; 
		}
	    
	    public override nint RowsInSection (UITableView tableview, nint section)
	    {
			return (nint)tableItems.Length;
	    }
	    public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
	    {
			// in a Storyboard, Dequeue will ALWAYS return a cell, 
	        UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
	        cell.TextLabel.Text = tableItems[indexPath.Row].Name;
			
			if (tableItems[indexPath.Row].Done) 
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			else
				cell.Accessory = UITableViewCellAccessory.None;

	        return cell;
	    }

		public Task GetItem(int id) {
			return tableItems[id];
		}
	}
}