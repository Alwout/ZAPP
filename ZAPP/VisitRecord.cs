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
    public class VisitRecord
    {
        private string client_id;
        public string naam;
        public string adres;
        public string postcode;
        public string woonplaats;
        public string datum;
        public string tijd;
        private int aanwezig = 0;

        public VisitRecord(string client_id, string naam, string adres, string postcode, string woonplaats, string datum, string tijd, int aanwezig)
        {
            this.client_id = client_id;
            this.naam = naam;
            this.adres = adres;
            this.postcode = postcode;
            this.woonplaats = woonplaats;
            this.datum = datum;
            this.tijd = tijd;
            this.aanwezig = aanwezig;
        }
    }
}