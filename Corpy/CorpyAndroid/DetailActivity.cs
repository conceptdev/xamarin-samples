using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace Corpy {
    [Activity(Label = "Employee")]
    public class DetailActivity : Activity {

        int employeeId;
        Employee employee;

        TextView NameTextView, DepartmentTextView;
        Button CallWorkButton, CallCellButton, EmailButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            employeeId = Intent.GetIntExtra("EmployeeId", 0);
            if (employeeId > 0) {
                employee = EmployeeManager.Get(employeeId);
            }

            SetContentView(Resource.Layout.Employee);

            NameTextView = FindViewById<TextView>(Resource.Id.NameTextView);
            DepartmentTextView = FindViewById<TextView>(Resource.Id.DepartmentTextView);
            CallWorkButton = FindViewById<Button>(Resource.Id.CallWorkButton);
            CallCellButton = FindViewById<Button>(Resource.Id.CallCellButton);
            EmailButton = FindViewById<Button>(Resource.Id.EmailButton);

            NameTextView.Text = employee.NameFormatted;
            DepartmentTextView.Text = employee.Department;

            CallWorkButton.Click += (s, e) => { Call(employee.Work); };

            CallCellButton.Click += (s, e) => { Call(employee.Mobile); };

            EmailButton.Click += (s, e) => { initShareItent("mail"); };
        }

        /// <summary>
        /// REQUIRES android.permission.CALL_PHONE
        /// </summary>
        void Call(int number)
        {
            String uri = "tel:" + number.ToString();
            Intent intent = new Intent(Intent.ActionCall);
            intent.SetData(Android.Net.Uri.Parse(uri));
            StartActivity(intent); 
        }



        // http://stackoverflow.com/questions/6827407/how-to-customize-share-intent-in-android/9229654#9229654
        private void initShareItent(String type)
        {
            bool found = false;
            Intent share = new Intent(Android.Content.Intent.ActionSend);
            share.SetType("image/jpeg");
            // gets the list of intents that can be loaded.    
            List<ResolveInfo> resInfo = PackageManager.QueryIntentActivities(share, 0).ToList();
            if (resInfo.Count > 0) {
                foreach (ResolveInfo info in resInfo) {
                    if (info.ActivityInfo.PackageName.ToLower().Contains(type) ||
                        info.ActivityInfo.Name.ToLower().Contains(type)) {
                        share.PutExtra(Intent.ExtraSubject, "[Corpy] hi");
                        share.PutExtra(Intent.ExtraText, "Hi " + employee.Firstname);
                        //                    share.PutExtra(Intent.EXTRA_STREAM, Uri.fromFile(new File(myPath)) );
                        // class atrribute               
                        share.SetPackage(info.ActivityInfo.PackageName);
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return;
                StartActivity(Intent.CreateChooser(share, "Select"));
            }
        } 
    }
}