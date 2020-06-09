using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Essentials;

namespace SkyCinema
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Activitylog : AppCompatActivity
    {
        EditText email;
        EditText pass;

        Button log;
        Button reg;

        public void StartLogin()
        {
            email = FindViewById<EditText>(Resource.Id.email);
            pass = FindViewById<EditText>(Resource.Id.pass);

            log = FindViewById<Button>(Resource.Id.log);
            reg = FindViewById<Button>(Resource.Id.reg);

            log.Click += delegate (object sender, EventArgs e)
            {
                for (int i = 0; i <= Preferences.Get("contains", 0); i++)
                {
                    if (string.Compare(email.Text, Preferences.Get("my_email" + i, null), true) == 0 && pass.Text == Preferences.Get("my_pass" + i, null)
                        && Preferences.Get("my_email" + i, null) != "null" && Preferences.Get("my_pass" + i, null) != "null"
                        && !Preferences.Get("Acces", false))
                    {
                        Preferences.Set("This_user", Preferences.Get("my_email" + i, null));
                        Preferences.Set("Acces", true);
                        Toast.MakeText(this, "Успешная авторизация", ToastLength.Short).Show();

                        StartActivity(typeof(MainActivity));
                        OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                    }
                    else
                    {
                        if(!Preferences.Get("Acces", false))
                        Toast.MakeText(this, "Ошибка авторизации", ToastLength.Short).Show();
                    }
                }
            };

            reg.Click += delegate (object sender, EventArgs e)
            {
                StartActivity(typeof(Activityreg));
                OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            };

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            if (Preferences.Get("Acces", false)==true)
            {
                StartActivity(typeof(MainActivity));
                OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            }
            else
            StartLogin();
        }

        public override void OnBackPressed()
        {
            this.FinishAffinity();
        }
    }
}