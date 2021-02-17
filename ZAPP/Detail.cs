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
    [Activity(Label = "Detail")]
    public class Detail : Activity 
    {
        ListView listView;
        List<TaskRecord> records;
        ArrayList[] result;

        _database db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Detail);

            _database db = new _database(this);
            this.db = db;
            result = db.getAllData();
            records = new List<TaskRecord>();

            int taskIndex = (int)_database.collection.tasks;
            int clientIndex = (int)_database.collection.clients;

            var bezoek_id = Intent.GetStringExtra("bezoek_id");
            var client_id = Intent.GetStringExtra("client_id");
            var aanwezig = Intent.GetIntExtra("aanwezig", 0);

            //Aanmeld Button
            Button aanmeldButton = (Button)FindViewById(Resource.Id.aanmeldButton);
            if (aanwezig == 1) {
                aanmeldButton.Text = "Afmelden";
            } else if (aanwezig == 0)
            {
                aanmeldButton.Text = "Aanmelden";
            }
            aanmeldButton.Click += OnAanmeldButtonClicked;


            ListView taskList = (ListView)FindViewById(Resource.Id.DetailListView);
            LinearLayout adresView = (LinearLayout)FindViewById(Resource.Id.adresLayout);

            //Taken Button
            Button takenButton = (Button)FindViewById(Resource.Id.taskButton);
            takenButton.Click += (object sender, EventArgs args) =>
            {
                taskList.Visibility = ViewStates.Visible;
                adresView.Visibility = ViewStates.Gone;
            };

            //Adres Button
            Button adresButton = (Button)FindViewById(Resource.Id.adresButton);
            adresButton.Click += (object sender, EventArgs args) =>
            {
                taskList.Visibility = ViewStates.Gone;
                adresView.Visibility = ViewStates.Visible;
            };

            //Loop through the taskrecords and only add it if it is the task belonging to that visit
            foreach (_database.taskRecord taskrecord in result[taskIndex])
            {
                if (bezoek_id == taskrecord.bezoek_id)
                {

                    TaskRecord row = new TaskRecord(taskrecord._id, taskrecord.titel, taskrecord.omschrijving, taskrecord.voltooid);

                    records.Add(row);
                }
            }

            foreach (_database.clientRecord clientRecord in result[clientIndex])
            {
                if (clientRecord._id == client_id)
                {
                    setAdresView(clientRecord);
                    break;
                }
            }

            listView = FindViewById<ListView>(Resource.Id.DetailListView);
            listView.Adapter = new TaskListViewAdapter(this, records);
            //listView.ItemClick += OnListItemClick;
        }

        void setAdresView(_database.clientRecord record)
        {
            TextView adresText = (TextView)FindViewById(Resource.Id.adresText);
            adresText.Text = String.Format("{0} \n{1} \n{2}\n", record.adres, record.postcode, record.woonplaats);

            TextView telefoonText = (TextView)FindViewById(Resource.Id.telefoonText);
            telefoonText.Text = String.Format("Telefoon: {0}", record.telefoonnummer);
        }

        void OnAanmeldButtonClicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            var bezoek_id = Intent.GetStringExtra("bezoek_id");
            int aanwezig = 0;
            if (btn.Text == "Afmelden")
            {
                db.postAanmelding(bezoek_id, aanwezig);
                setAanwezig(bezoek_id, aanwezig);
                btn.Text = "Aanmelden";
                return;
            }
            else if (btn.Text == "Aanmelden")
            {
                aanwezig = 1;
                db.postAanmelding(bezoek_id, aanwezig);
                setAanwezig(bezoek_id, aanwezig);
                btn.Text = "Afmelden";
            }
        }

        private void setAanwezig(string bezoek_id, int aanwezig)
        {
            //Set the visit record to present or not present
            List<VisitRecord> records = Overview.records;
            foreach (VisitRecord visitrecord in records)
            {
                if (visitrecord.bezoek_id == bezoek_id)
                {
                    Console.WriteLine("set aanwezig " + aanwezig + " of " + visitrecord.naam);
                    visitrecord.aanwezig = aanwezig;
                }
            }
        }
    }
}