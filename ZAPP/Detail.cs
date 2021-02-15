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
    [Activity(Label = "Detail")]
    public class Detail : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Detail);

            var id = Intent.GetStringExtra("ID");
            var code = Intent.GetStringExtra("code");
            var description = Intent.GetStringExtra("description");
            Console.WriteLine("Got ID: " + id);
            //FindViewById<TextView>(Resource.Id.detailText1).Text = code;
            //FindViewById<TextView>(Resource.Id.detailText2).Text = description;
        }
    }
}