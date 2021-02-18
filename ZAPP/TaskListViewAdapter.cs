using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZAPP
{
    [Activity(Label = "TaskListViewAdapter")]
    public class TaskListViewAdapter : BaseAdapter<TaskRecord>
    {

        List<TaskRecord> items;
        Activity context;
        _database db;

        public TaskListViewAdapter(Activity context, List<TaskRecord> items) : base()
        {
            this.context = context;
            this.items = items;
            this.db = new _database(context);
        }

        public override TaskRecord this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.TaskRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.TitelText).Text = String.Format("0{0} - {1}", position+1, item.titel);

            //Checkmark
            CheckBox voltooidMark = view.FindViewById<CheckBox>(Resource.Id.checkBox1);
            if (item.voltooid == 1)
            {
                voltooidMark.Checked = true;
            } else if (item.voltooid == 0)
            {
                voltooidMark.Checked = false;
            }
            voltooidMark.Click += (object sender, EventArgs args) =>
            {

                string task_id = item.id;
                if (voltooidMark.Checked)
                {
                    db.postVoltooid(task_id, 1);
                    //Console.WriteLine(item.titel + " is voltooid!");
                } else
                {
                    db.postVoltooid(task_id, 0);
                    //Console.WriteLine(item.titel + " is niet voltooid!");
                }
            };

            return view;
        }

    }
}