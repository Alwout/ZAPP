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
    [Activity(Label = "OverviewListViewAdapter")]
    public class OverviewListViewAdapter : BaseAdapter<VisitRecord>
    {
        List<VisitRecord> items;
        Activity context;

        public OverviewListViewAdapter(Activity context, List<VisitRecord> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override VisitRecord this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.VisitRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.NaamText).Text = item.naam;
            view.FindViewById<TextView>(Resource.Id.AdresText).Text = String.Format("{0}, {1} {2}", item.adres, item.postcode, item.woonplaats);
            view.FindViewById<TextView>(Resource.Id.DatumText).Text = String.Format("{0} {1} uur", item.datum, item.tijd);

            return view;
        }
    }
}