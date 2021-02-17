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
        public string bezoek_id;
        public string client_id;
        public string naam;
        public string adres;
        public string postcode;
        public string woonplaats;
        public string datum;
        public string tijd;
        public int aanwezig = 0;

        public VisitRecord(string bezoek_id, string client_id,string naam, string adres, string postcode, string woonplaats, string datum, string tijd, int aanwezig)
        {
            this.bezoek_id = bezoek_id;
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