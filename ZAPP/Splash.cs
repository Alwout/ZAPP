using System;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ZAPP
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Splash", MainLauncher = true)]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _database db = new _database(this);

            Thread.Sleep(5000);

            if (db.isUserLoggedIn())
            {
                StartActivity(typeof(Overview));
            } else
            {
                StartActivity(typeof(Login));
            }
        }

    }
}
