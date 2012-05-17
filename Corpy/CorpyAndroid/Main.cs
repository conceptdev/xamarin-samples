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
using System.IO;

namespace Corpy {
    [Application]
    public class CorpyApp : Application {
        public CorpyApp(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            if (!EmployeeManager.HasDataAlready) {
                Console.WriteLine("MAIN Load Employees.xml");
                Stream seedDataStream = Assets.Open(@"Employees.xml");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                using (StreamReader reader = new StreamReader(seedDataStream)) {
                    //This is an arbitrary size for this example.
                    char[] c = null;

                    while (reader.Peek() >= 0) {
                        c = new char[4096];
                        reader.Read(c, 0, c.Length);
                        sb.Append(c);
                    }
                }
                string xml = sb.ToString();

                EmployeeManager.UpdateFromString(xml);
            }
        }
    }
}