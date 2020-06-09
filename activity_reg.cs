using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Essentials;

namespace SkyCinema
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Activityreg : AppCompatActivity
    {
        EditText email_reg;
        EditText pas_reg;

        Button back_to_log;
        Button reg_now;

        public void StartReg()
        {
            email_reg = FindViewById<EditText>(Resource.Id.email_reg);
            pas_reg = FindViewById<EditText>(Resource.Id.pas_reg);

            back_to_log = FindViewById<Button>(Resource.Id.back_to_log);
            reg_now = FindViewById<Button>(Resource.Id.reg_now);

            back_to_log.Click += delegate (object sender, EventArgs e)
            {
                StartActivity(typeof(Activitylog));
                OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            };

            reg_now.Click += delegate (object sender, EventArgs e)
            {
                bool go = true;

                if (email_reg.Text.Length > 7 && pas_reg.Text.Length > 3 && pas_reg.Text.Length < 21)
                {
                    if (email_reg.Text.Contains("@", StringComparison.Ordinal) && email_reg.Text.Contains(".com", StringComparison.Ordinal))
                    {
                       
                            for (int i = 1; go; i++)
                            {
                                if (email_reg.Text != Preferences.Get("my_email" + i, null))
                                {
                                    if (Preferences.Get("my_email" + i, null) == null && Preferences.Get("my_pass" + i, null) == null)
                                    {
                                        Preferences.Set("my_email" + i, email_reg.Text);
                                        Preferences.Set("my_pass" + i, pas_reg.Text);

                                        if (i > Preferences.Get("contains", 0))
                                        {
                                            Preferences.Set("contains", i);
                                            Toast.MakeText(this, "Пользователь зарегистрирован", ToastLength.Short).Show();
                                        }
                                        go = false;
                                    }
                                }
                                else
                                { go = false; Toast.MakeText(this, "Пользователь уже зарегистрирован", ToastLength.Short).Show(); }
                            }
                    }
                    else
                        Toast.MakeText(this, "Некоректный email", ToastLength.Short).Show();
                }
                else
                    Toast.MakeText(this, "Не заполнены поля регистрации. Длина пароля должна быть от 4 до 20 символов", ToastLength.Short).Show();

                email_reg.Text = "";
                pas_reg.Text = "";
            };
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_reg);

            StartReg();
        }
    }
}