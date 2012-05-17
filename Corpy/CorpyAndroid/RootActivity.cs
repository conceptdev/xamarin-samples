using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Corpy {
    [Activity(Label = "CorpyAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class RootActivity : ListActivity {
        
        List<Employee> employees;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            employees = EmployeeManager.GetAll();
            ListAdapter = new RootAdapter(this, employees);

            ListView.ItemClick += (s, ea) => {
                var employeeDetails = new Intent(this, typeof(DetailActivity));
                employeeDetails.PutExtra("EmployeeId", employees[ea.Position].Id);
                StartActivity(employeeDetails);
            };
        }
    }
}

