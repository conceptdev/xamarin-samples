using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UIKit;

namespace TaskyA11y 
{
	public class RootTableSource : UITableViewSource 
	{
		// ##
		// there is NO database or storage of todo items in this example, just an in-memory List<>
		// refer to the other Tasky samples on github for an implementation using SQLite-NET
		// ##
		TodoItem[] tableItems;
	    string cellIdentifier = "todocell";
	 
		public RootTableSource (TodoItem[] items)
		{
			tableItems = items; 
		}
	    
	    public override nint RowsInSection (UITableView tableview, nint section)
	    {
	        return tableItems.Length;
	    }
	    public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
	    {
			// in a Storyboard, Dequeue will ALWAYS return a cell, 
	        UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);

			var todo = tableItems [indexPath.Row];
			cell.TextLabel.Text = todo.Name;
			
			if (todo.Done) 
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			else
				cell.Accessory = UITableViewCellAccessory.None;

			// TODO: review accessibility label for cell
			cell.IsAccessibilityElement = true;
			cell.AccessibilityLabel = todo.Name + (todo.Done ? " is done" : " is not complete");
			cell.AccessibilityTraits = UIAccessibilityTrait.Button;

	        return cell;
	    }

		public TodoItem GetItem(int id) {
			return tableItems[id];
		}
	}
}