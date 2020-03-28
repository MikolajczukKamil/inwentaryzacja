using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

namespace Kalkulator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += OnButtonClick;

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            double var1 = Convert.ToDouble(FindViewById<EditText>(Resource.Id.editText1).Text);
            double var2 = Convert.ToDouble(FindViewById<EditText>(Resource.Id.editText2).Text);

            TextView text = FindViewById<TextView>(Resource.Id.textView1);
            text.Text = "Dodawanie: " + (var1 + var2) + "\nOdejmowanie: " + (var1 - var2) + "\nMnożenie: " + (var1 * var2) + "\nDzielenie: ";
            if (var2 == 0)
                text.Text += "Dzielenie przez 0";
            else if (var1 == 0)
                text.Text += "0";
            else
                text.Text += (var1 + var2);

        }
    }
}