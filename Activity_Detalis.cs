using System;                                                                                     
using System.Net.Http;                                                                        
using Xamarin.Essentials;                                                                                 
using Android.App;                                                                                   
using Android.OS;                                                                          
using Android.Support.V7.App;                                                          
using Android.Widget;                                                                                       
using Newtonsoft.Json;
using Android.Webkit;

namespace SkyCinema
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ActivityDetalis : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_detalis);

            WebView webView;

            RatingBar ratingBar;

            TextView textLarge;
            TextView textMed;
            TextView textMed2;
            TextView textMed3;

            string lang, originalLang, bud, budNone;
            
            textLarge = FindViewById<TextView>(Resource.Id.textLarge);
            textMed = FindViewById<TextView>(Resource.Id.textMed); 
            textMed2 = FindViewById<TextView>(Resource.Id.textMed2);
            textMed3 = FindViewById<TextView>(Resource.Id.textMed3);

            ratingBar = FindViewById<RatingBar>(Resource.Id.ratingbar);

            webView = FindViewById<WebView>(Resource.Id.webView3); 
            
            webView.SetInitialScale(GetDisp());

            if (Preferences.Get("Lang", 1) == 1)
            {
                lang = "?api_key=a49cf8a5f42225880f049917a2262e81&language=ru"; textMed.Text = "Рейтинг "; originalLang = "Язык оригинала: "; bud = "Бюджет "; budNone = "Неизвестно";
            }
            else
            {
                lang = "?api_key=a49cf8a5f42225880f049917a2262e81&language=en"; textMed.Text = "Rate "; originalLang = "Original Lang: "; bud = "budget "; budNone = "Unknown";
            }

            string det = Preferences.Get("Detalis", "10003"); ;
            string Link = "https://api.themoviedb.org/3/movie/" + det + lang;

            GetHTTP(Link, textLarge, textMed2, textMed3, webView, ratingBar, originalLang, bud, budNone);
        }

        int GetDisp()
        {
            double disp;

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var height = mainDisplayInfo.Height;

            disp = height / 12.8;

            return Convert.ToInt32(disp);
        }

        async void GetHTTP(string Link, TextView textLarge, TextView textMed2, TextView textMed3,WebView webView, RatingBar ratingBar,string originalLang, string bud, string budNone)
        {
            try
            {


                var req = new HttpRequestMessage
                {
                    RequestUri = new System.Uri(Link),
                    Method = HttpMethod.Get
                };
                req.Headers.Add("Accept", "application/json");

                var client = new HttpClient();
                HttpResponseMessage res = await client.SendAsync(req).ConfigureAwait(true);

                HttpContent http = res.Content;
                var json = await http.ReadAsStringAsync().ConfigureAwait(true);
                var cont = JsonConvert.DeserializeObject<Solo>(json);

                textLarge.Text = cont.Title;

                webView.LoadUrl("https://image.tmdb.org/t/p/w500" + cont.Poster);

                textMed2.Text = "";
               
                textMed2.Text = "\n" + cont.ReleaseDate;
                ratingBar.Rating = cont.Vote;

                textMed3.Text = "";

                for (int i = 0; cont.Ganre.Length > i; i++)
                {
                    textMed3.Text += cont.Ganre[i].Name + " ";
                }

                textMed3.Text += "\n" + "\n" + "   " + cont.Overview + "\n" + originalLang + cont.OriginalTitle + "\n" + bud + " ";

                if(cont.Budget == "0")
                {
                    textMed3.Text += budNone;
                }
                else
                    textMed3.Text += cont.Budget;

            }
            catch(Exception ex)
            {
                Toast.MakeText(this, Convert.ToString(ex),ToastLength.Short).Show();
            }
        }
    }
}
