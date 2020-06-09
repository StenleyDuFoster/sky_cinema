using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using Xamarin.Essentials;

namespace SkyCinema
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ActivityProf : AppCompatActivity
    {
        TextView email;
        TextView search;
        TextView content;
        TextView lang;

        Button top;
        Button pop;

        Button all;
        Button f;

        Button ru;
        Button eng;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_prof);

            email = FindViewById<TextView>(Resource.Id.textemail);
            search = FindViewById<TextView>(Resource.Id.textSearch);
            content = FindViewById<TextView>(Resource.Id.textContent);
            lang = FindViewById<TextView>(Resource.Id.textLang);

            pop = FindViewById<Button>(Resource.Id.contentSearchPop);
            top = FindViewById<Button>(Resource.Id.contentSearchTop);

            all = FindViewById<Button>(Resource.Id.contentAll);
            f = FindViewById<Button>(Resource.Id.contentF);

            ru = FindViewById<Button>(Resource.Id.langRu);
            eng = FindViewById<Button>(Resource.Id.langEN);

            email.Text = Preferences.Get("This_user", "error 1");

            Lang();
            Init();
            Main();

        }

        void MakeToast()
        {
            if (Preferences.Get("Lang", 1) == 1)
            {
                Toast.MakeText(this,"Некоторые изминения вступят в силу после перезапуска программы",ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Some changes will take effect after restarting the program", ToastLength.Short).Show();
            }
        }

        void Init()
        {
            if (Preferences.Get("Search", 1) == 1)
            {
                pop.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                top.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
            }
            else
            {
                pop.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                top.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
            }

            if (Preferences.Get("Sort", 1)==1)
            {
                all.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                f.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
            }
            else
            {
                all.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                f.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
            }

            if (Preferences.Get("Lang", 1)==1)
            {
                ru.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                eng.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
            }
            else
            {
                ru.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                eng.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
            }

        }

        void Main()
        {
            pop.Click += delegate (object sender, EventArgs e)
            {
                Preferences.Set("Search", 2);
                pop.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                top.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                MakeToast();
            };

            top.Click += delegate (object sender, EventArgs e)
            {
                Preferences.Set("Search", 1);
                pop.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                top.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                MakeToast();
            };

            all.Click += delegate (object sender, EventArgs e)
            {
                Preferences.Set("Sort", 2);
                all.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                f.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                MakeToast();
            };
            f.Click += delegate (object sender, EventArgs e)
            {
                Preferences.Set("Sort", 1);
                all.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                f.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                MakeToast();
            };

            ru.Click += delegate (object sender, EventArgs e)
            {
                Preferences.Set("Lang", 1);
                Lang();
                ru.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                eng.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                MakeToast();
            };

            eng.Click += delegate (object sender, EventArgs e)
            {
                Preferences.Set("Lang", 2);
                Lang();
                ru.SetBackgroundColor(new Android.Graphics.Color(128, 128, 128));
                eng.SetBackgroundColor(new Android.Graphics.Color(70, 130, 180));
                MakeToast();
            };

        }

        void Lang()
        {
            if (Preferences.Get("Lang", 1) == 1)
            {
                search.Text = "Искать";
                content.Text = "Загружать";
                lang.Text = "Язык";

                pop.Text = "Популярные";
                top.Text = "С найвысшими оценками";
                all.Text = "Весь контент";
                f.Text = "Страницами";
                ru.Text = "Русский";
                eng.Text = "Английский";
            }
            else
            {
                search.Text = "Search";
                content.Text = "Downloading";
                lang.Text = "Language";

                pop.Text = "Popular";
                top.Text = "Top rated";
                all.Text = "All";
                f.Text = "Pages";
                ru.Text = "Russian";
                eng.Text = "English";
            }
        }
    }
}
