using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZAPP
{
    [Activity(Label = "Overview")]
    public class Overview : Activity
    {
        ListView listView;
        List<VisitRecord> records;
        ArrayList[] result;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Overview);

            _database db = new _database(this);
            result = db.getAllData();
            records = new List<VisitRecord>();

            int visitIndex = (int)_database.collection.visits;
            int clientIndex = (int)_database.collection.clients;
            Console.WriteLine(result);



            foreach (_database.visitRecord visitrecord in result[visitIndex])
            {

                foreach(_database.clientRecord clientrecord in result[clientIndex])
                {
                    if (clientrecord._id == visitrecord.client_id)
                    {
                        VisitRecord row = new VisitRecord(visitrecord.client_id, clientrecord.naam, clientrecord.adres, clientrecord.postcode, clientrecord.woonplaats, visitrecord.datum, visitrecord.tijd, visitrecord.aanwezig);

                        records.Add(row);
                        break;
                    }
                }
            }

            foreach (_database.clientRecord clientrecord in result[clientIndex])
            {

            }

            listView = FindViewById<ListView>(Resource.Id.listView1);
            listView.Adapter = new OverviewListViewAdapter(this, records);
            listView.ItemClick += OnListItemClick;
        }

        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var t = records[e.Position];

            var intent = new Intent(this, typeof(Detail));
            //intent.PutExtra("ID", t);
            //intent.PutExtra("code", t.code);
            //intent.PutExtra("description", t.description);
            StartActivityForResult(intent, 0);
        }

    }
}