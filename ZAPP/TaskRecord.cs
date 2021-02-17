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
    public class TaskRecord
    {
        public string id;
        public string titel;
        public string omschrijving;
        public int voltooid;

        public TaskRecord(string id, string titel, string omschrijving, int voltooid)
        {
            this.id = id;
            this.titel = titel;
            this.omschrijving = omschrijving;
            this.voltooid = voltooid;
        }
    }
}