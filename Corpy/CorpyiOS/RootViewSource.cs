using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Corpy {
	public class RootViewSource : UITableViewSource {

		List<Employee> employees;

		NSString cellIdentifier = new NSString("EmployeeCell"); // set in Storyboard

		public RootViewSource ()
		{
			employees = EmployeeManager.GetAll();
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var employee = employees[indexPath.Row];
			var cell = tableView.DequeueReusableCell(cellIdentifier);
			
			cell.TextLabel.Text = employee.NameFormatted;
			cell.DetailTextLabel.Text = employee.Department;
			
			return cell;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return employees.Count;
		}
		public Employee GetItem(int rowId) {
			return employees[rowId];
		}
	}
}

