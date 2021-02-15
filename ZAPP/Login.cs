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
    [Activity(Label = "Login", Theme = "@style/AppTheme.NoActionBar")]
    public class Login : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Login);

            

            Button submitButton = FindViewById<Button>(Resource.Id.submitButton);

            submitButton.Click += OnButtonClicked;
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {

            //await db.postUserData();

            EditText usernameInput = FindViewById<EditText>(Resource.Id.inputUsername);
            EditText passwordInput = FindViewById<EditText>(Resource.Id.inputPassword);

            _database db = new _database(this);

            Console.WriteLine(usernameInput.Text + passwordInput.Text);

            db.verifyUser(usernameInput.Text, passwordInput.Text);

        }
    }
}