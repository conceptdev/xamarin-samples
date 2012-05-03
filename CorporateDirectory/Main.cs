using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SQLiteClient;

namespace CorporateDirectory1
{
	public class Application
	{
		static void Main (string[] args)
		{
			try 
			{
				UIApplication.Main (args);
			} 
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{		
		static NSString kCellIdentifier = new NSString ("CellIdentifier");
	
		private List<Employee> listData;
		
		public int ListCount {get{return listData.Count();}}
		
		public List<Employee> Employees {get{return listData;}}
		
		public Employee SelectedEmployee = null;
		
		EmployeeDataSource dataSource;
		EmployeeListDelegate @delegate;
		UIAlertViewDelegate alertDelegate;
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{

		dataSource = new EmployeeDataSource(this);
		@delegate = new EmployeeListDelegate(this);
		tableviewEmployee.DataSource = dataSource;
		tableviewEmployee.Delegate = @delegate;
		alertDelegate = new CallAlert (this);

			//System.MissingMethodException: Method not found: 'Default constructor not found...ctor() of CorporateDirectory1.Employee'.
			var e = new Employee();
			e.Firstname = "a"; e.Work=1; e.Mobile=2; e.Lastname="b";e.Department="c";e.Email="@";
			var f = e.Firstname; var g = e.Work; var h=e.Mobile; var i=e.Lastname; var j=e.Department; var k=e.Email;
			
			using (var db = new SQLiteClient.SQLiteConnection("phonebook")) {
				// Perform strongly typed queries
			    var users = db.Query<Employee>("SELECT Firstname, Lastname, Work, Mobile, Department, Email FROM Phonebook ORDER BY Lastname", 1000);
			    
				listData = users.ToList();
			}
			
			
			window.MakeKeyAndVisible ();
			return true;
		}

		void DialogCall (string name, string work, string mobile, string email)
		{
			using (var alert = new UIAlertView ("Make Contact"
				                                    , "Call or email " + name + " now?"
				                                    , alertDelegate
				                                    , "Cancel"
				                                    , work
				                           			, mobile
			                                    , email))
				{
			       alert.Show ();
				}
		}
		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
		public class CallAlert : UIAlertViewDelegate
		{
			private AppDelegate _appd;
			public CallAlert (AppDelegate appd)
			{
				_appd = appd;
			}
			public override void Clicked (UIAlertView alertview, int buttonIndex)
			{
				Console.WriteLine("Clicked " + buttonIndex);
				NSUrl u=null;
				if (buttonIndex == 1)
				{
					Console.WriteLine("tel:" + _appd.SelectedEmployee.Work);
					u = new NSUrl("tel:" + _appd.SelectedEmployee.Work);
				} 
				else if (buttonIndex == 2)
				{
					Console.WriteLine("tel:" + _appd.SelectedEmployee.Mobile);
					u = new NSUrl("tel:" + _appd.SelectedEmployee.Mobile);
				}
				else if (buttonIndex == 3)
				{
					Console.WriteLine("mailto:" + _appd.SelectedEmployee.Email);
					u = new NSUrl("mailto:" + _appd.SelectedEmployee.Email);
				}
				if (u != null)
				{
					if (!UIApplication.SharedApplication.OpenUrl(u))
					{
						Console.WriteLine("Not Supported");
						NotSupportedAlert(u.Scheme);
					}
				}
			}
			private void NotSupportedAlert(string scheme)
			{	
				var av = new UIAlertView("Not supported"
			                         , "Scheme '"+scheme+"' is not supported on this device"
			                         , null
			                         , "k thanks"
			                         , null);
				av.Show();
	     	}
		}
		public class EmployeeDataSource : UITableViewDataSource
		{
			private AppDelegate _appd;
			public EmployeeDataSource (AppDelegate appd)
			{
				_appd = appd;				
			}
			public override int RowsInSection (UITableView tableview, int section)
			{
				return _appd.ListCount;
			}
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (kCellIdentifier);
				if (cell == null)
				{
					cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
				}
				int row = indexPath.Row;
				Employee e = _appd.Employees[row];
				cell.TextLabel.Text = e.Firstname + " " + e.Lastname;
				return cell;
			}
		}
		public class EmployeeListDelegate : UITableViewDelegate
		{
			AppDelegate _appd;
			public EmployeeListDelegate (AppDelegate appd)
			{
				_appd = appd;
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				int row = indexPath.Row;
					Employee e = _appd.Employees[row];
				string rowValue = e.Firstname + " " + e.Lastname;
				Console.WriteLine("selected " + rowValue);
				_appd.SelectedEmployee = e;
				_appd.DialogCall(rowValue, e.Work.ToString(), e.Mobile.ToString(), e.Email);
				_appd.HideKeyboard();
			}
		}
	}
}