using Xamarin.Essentials;//          c                   l
using Android.Views;//                r       y i       n e
using Android.Widget;//                e     b   n     a   y
using Android.Webkit;//                 a         s   t     .
using Android.Support.V7.Widget;//       t d       t s       d
using System.Collections.Generic;//       e         :         f
using Android.Content;//                 /|\       /|\       /|\
using System;

namespace SkyCinema
{
    
    class RecyclerWiewHolder : RecyclerView.ViewHolder,View.IOnClickListener,View.IOnLongClickListener
    {
        public WebView ImageView { get; set; }
        public TextView TitleText { get; set; }
        public TextView GanreText { get; set; }
        public TextView MainText { get; set; }
        public Button Button1 { get; set; }// Fis
        public Button Button2 { get; set; }//
        
        // КЛИКАБЕЛЬНОСТЬ

        public IItemClickListener itemClickListener;

        public RecyclerWiewHolder(View itemView) : base(itemView)
        {
            if (Preferences.Get("Sort", 1) == 1)
            {
                ImageView = itemView.FindViewById<WebView>(Resource.Id.webView1);
                TitleText = itemView.FindViewById<TextView>(Resource.Id.textView21);
                GanreText = itemView.FindViewById<TextView>(Resource.Id.textView22);
                MainText = itemView.FindViewById<TextView>(Resource.Id.textView23);
                Button1 = itemView.FindViewById<Button>(Resource.Id.buttonItem);
                Button1.SetOnClickListener(this);
            }
            else
            {
                ImageView = itemView.FindViewById<WebView>(Resource.Id.webView2);
                TitleText = itemView.FindViewById<TextView>(Resource.Id.textView31);
                GanreText = itemView.FindViewById<TextView>(Resource.Id.textView32);
                Button2 = itemView.FindViewById<Button>(Resource.Id.buttonItemSort);
                Button2.SetOnClickListener(this);
            }

            ImageView.SetInitialScale(GetDisp());
        }

        int GetDisp()
        {
            double disp;

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            //var width = mainDisplayInfo.Width;
            var height = mainDisplayInfo.Height;

            disp = height / 38.4;//21.6

            return Convert.ToInt32(disp);
        }

        public void SetItemClickListener(IItemClickListener itemClickListener)
        {
            this.itemClickListener = itemClickListener;
        }

        public void OnClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, false);
        }

        public bool OnLongClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, true);
            return true;
        }
    }

    public class ResVievAdapter : RecyclerView.Adapter, IItemClickListener
    {
        private List<ResData> listData = new List<ResData>();
        string idLocal;

        public ResVievAdapter(List<ResData> listData)
        {
            this.listData = listData;
        }

        public override int ItemCount
        {
            get
            {
                return listData.Count;
            }
        }
        // VISIBILITY
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int i)
        {
            RecyclerWiewHolder recyclerWiewHolder = holder as RecyclerWiewHolder;
            recyclerWiewHolder.SetItemClickListener(this);

            if (Preferences.Get("Sort", 1) == 1)
            {
                recyclerWiewHolder.ImageView.LoadUrl(listData[i].imageUrl);
                recyclerWiewHolder.TitleText.Text = listData[i].titleText;
                recyclerWiewHolder.GanreText.Text = listData[i].ganreText;
                recyclerWiewHolder.MainText.Text = listData[i].mainText + listData[i].releaseText;
            }
            else
            {
                recyclerWiewHolder.ImageView.LoadUrl(listData[i].imageUrl);
                recyclerWiewHolder.TitleText.Text = listData[i].titleText;
                recyclerWiewHolder.GanreText.Text = listData[i].ganreText;
                recyclerWiewHolder.GanreText.Text = listData[i].mainText;
                recyclerWiewHolder.GanreText.Text = listData[i].releaseText;
            }
            idLocal = listData[i].id;
        }
        // SETUP ITEM DETALIS
        public void OnClick(View itemView, int i, bool isLongClick)
        {
            
            Intent intent = new Intent(itemView.Context, typeof(ActivityDetalis));

            itemView.Context.StartActivity(intent);

            Preferences.Set("Detalis", Convert.ToString(listData[i].id));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);

            int itemId;

            if (Preferences.Get("Sort", 1) == 1)
                itemId = Resource.Layout.item;
            else
                itemId = Resource.Layout.itemSort;

            View itemView = inflater.Inflate(itemId, parent, false);
            return new RecyclerWiewHolder(itemView);
        }
    }
}
