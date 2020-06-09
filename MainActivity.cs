using System;                                                                                               /*                                       */
using System.Net.Http;                                                                                      /*                                       */
using System.Collections.Generic;                                                                           /*                                       */
using Xamarin.Essentials;                                                                                   /*                                       */
using Android.App;                                                                                          /*                                       */
using Android.Net;                                                                                          /*                                       */
using Android.OS;                                                                                           /*   _________________________________   */
using Android.Runtime;                                                                                      /*  |                                 |  */
using Android.Support.Design.Widget;                                                                        /* |  code created by inst:stanley.df  | */
using Android.Support.V4.View;                                                                              /*  |_________________________________|  */
using Android.Support.V4.Widget;                                                                               /*                                 */
using Android.Support.V7.App;                                                                                     /*                           */
using Android.Views;                                                                                                 /*                     */
using Android.Widget;                                                                                                     /*           */
using Android.Support.V7.Widget;                                                                                             /*    */
using Newtonsoft.Json;                                                                                                         /**/

namespace SkyCinema
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private bool mayExit = false;
        static int Page = 1;
        private const string ApiKey = "a49cf8a5f42225880f049917a2262e81";
        Button button;
        Button sort1;
        Button sort2;

        readonly string MainLink = "https://api.themoviedb.org/3";
        string SearchLink;
        string LangLink = "&language=ru";
        readonly string PageLink = "&page=";

        //АДАПТИВНАЯ ЧАСТЬ

        private RecyclerView recyclerView;
        private ResVievAdapter adapter;
        private RecyclerView.LayoutManager layoutmanager;

        private readonly List<ResData> listData = new List<ResData>();


        private void MainVoid()
        {
            int totalPage;

            if (Preferences.Get("Sort", 1) == 1)
            {
                if (Preferences.Get("Total", 100) > (Page + 4))
                    totalPage = Page + 4;
                else if (Preferences.Get("Total", 100) > (Page + 1))
                    totalPage = Page + 1;
                else
                    totalPage = 0;
            }
            else
            {
                totalPage = Preferences.Get("Total", 100);
            }

                Toast.MakeText(this, "Загрузка ...", ToastLength.Short).Show();

                do
                {

                        GetHttp(CreateApiLink(MainLink, SearchLink, ApiKey, LangLink, PageLink, Convert.ToString(Page)));

                    Page += 1;

                } while (Page < totalPage);
            
        }

        private async void GetHttp(string Link)
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
               var cont = JsonConvert.DeserializeObject<JsonData>(json);

            Preferences.Set("Total", cont.TotalPages);


                for (int y = 0; y < cont.Results.Length; y++)
                {
                    string localGenre = "";

                    for (int i = 0; i < cont.Results[y].Ganre.Length; i++)
                    {
                        localGenre += Genre(cont.Results[y].Ganre[i]) + " ";
                    }

                    string rateLang, dateLang;

                    if (Preferences.Get("Lang", 1) == 1)
                    {
                        rateLang = "Оценка ";
                        dateLang = "Дата выхода ";
                    }
                    else
                    {
                        rateLang = "Rate ";
                        dateLang = "Release date ";
                    }

                string sub = cont.Results[y].Overview;
                if (sub.Length > 110)
                    sub = cont.Results[y].Overview.Substring(0, 110);

               // if (Preferences.Get("Sort", 1) == 1)
                    AddItem("https://image.tmdb.org/t/p/w500" + cont.Results[y].ImgLink, cont.Results[y].Title, localGenre, sub
                        + "... " + "\n" + rateLang + Convert.ToString((cont.Results[y].Vote)) + "/10 " + dateLang, cont.Results[y].Release,Convert.ToString(cont.Results[y].Id));
                /*else
                    AddItem("https://image.tmdb.org/t/p/w500" + cont.Results[y].ImgLink, cont.Results[y].Title,"", cont.Results[y].Release, "");*/

                AdaptiveInit();
                }
            
        }

        

        private static string CreateApiLink(string MainLink, string SearchLink, string Api, string LangLink,string PageLink, string Page12)
        {
            string ApiLink = MainLink + SearchLink + "api_key=" + Api + LangLink + PageLink + Page12;

            return ApiLink;
        }

        private static string Genre(int genre)
        {
            string lastGenre;
            string[] nameGenre = new string[] { "боевик", "приключения", "мультфильм", "комедия", "криминал",
                "документальный", "драма", "семейный", "фэнтези", "история", "ужасы", "музыка", "детектив",
                "мелодрама","фантастика", "телевизионный фильм", "триллер", "военный", "вестерн" };
            int[] genreId = new int[] { 28, 12, 16, 35, 80, 99, 18, 10751, 14, 36, 27, 10402, 9648, 10749, 878, 10770, 53, 10752, 37 };
            int count = 0;

            do
            {
                if (count < 18)
                    lastGenre = nameGenre[count + 1];
                else
                    lastGenre = "обновите библиотеку жанров";
                count++;
            } while (genre != genreId[count] && count < 18);

            return lastGenre;
        }

        private void AddItem(string imgUrl, string title, string genre, string main, string release, string id)
        {
                 listData.Add(new ResData() { imageUrl = imgUrl, titleText = title, releaseText = release, id = id, ganreText = genre, mainText = main  });
        }
        //REC
        private void AdaptiveInit()
        {
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            recyclerView.HasFixedSize = true;

            if(Preferences.Get("Sort",1) == 1)
                layoutmanager = new LinearLayoutManager(this);
            else
                layoutmanager = new GridLayoutManager(this,3);

            recyclerView.SetLayoutManager(layoutmanager);
            adapter = new ResVievAdapter(listData); 
            recyclerView.SetAdapter(adapter);

        }

        //STANDART
        private void ToolBarOn()
        {
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            var cm = (ConnectivityManager)GetSystemService(Android.App.Application.ConnectivityService);
            bool isConnected = cm.ActiveNetworkInfo.IsConnected;

            ToolBarOn();

            if (isConnected == true)
            {
                button = FindViewById<Button>(Resource.Id.buttonNext);
                sort1 = FindViewById<Button>(Resource.Id.buttonSort1);
                sort2 = FindViewById<Button>(Resource.Id.buttonSort2);

                if (Preferences.Get("Lang", 1) == 1)
                { LangLink = "&language=ru"; sort1.Text = "Таблица"; sort2.Text = "Список"; }
                else
                { LangLink = "&language=en"; sort1.Text = "Table"; sort2.Text = "List"; }

                if (Preferences.Get("Search", 1) == 1)
                { SearchLink = "/movie/top_rated?"; }
                else
                { SearchLink = "/movie/upcoming?"; }

                MainVoid();
            
                button.Click += delegate (object sender, EventArgs e)
                {
                    listData.Clear();
                    MainVoid();
                };

                sort1.Click += delegate (object sender, EventArgs e)
                {
                    Preferences.Set("Sort", 2);
                    AdaptiveInit(); 
                };
                sort2.Click += delegate (object sender, EventArgs e)
                {
                     Preferences.Set("Sort", 1);
                     AdaptiveInit();

                };
            }
            else
                Toast.MakeText(this, "Ошибка интернет подключения", ToastLength.Short).Show();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                if (mayExit)
                {
                    this.FinishAffinity();
                }
                else
                {
                    Toast.MakeText(this, "Нажмите ещё раз что бы выйти", ToastLength.Short).Show();
                    mayExit = true;
                }
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            Android.Views.View view = (Android.Views.View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

             if (id == Resource.Id.nav_share)
            {
                StartActivity(typeof(ActivityProf));
                OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            }
            else if (id == Resource.Id.nav_send)
            {
                Preferences.Set("Acces", false);
                StartActivity(typeof(Activitylog));
                OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);

            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
