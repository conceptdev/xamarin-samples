using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.App;
using Android.Views;

namespace Corpy {
    public class RootAdapter : BaseAdapter<Employee> {
        Activity context = null;
        IList<Employee> employees = new List<Employee>();

        public RootAdapter(Activity context, IList<Employee> employees)
            : base()
        {
            this.context = context;
            this.employees = employees;
        }

        public override Employee this[int position]
        {
            get { return employees[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return employees.Count; }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var item = employees[position];

            var view = (convertView ??
                context.LayoutInflater.Inflate(
                Android.Resource.Layout.SimpleListItem2,
                parent, false)) as View;

            var txt = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            var dep = view.FindViewById<TextView>(Android.Resource.Id.Text2);

            txt.Text = item.NameFormatted;
            dep.Text = item.Department;

            return view;
        }
    }
}
