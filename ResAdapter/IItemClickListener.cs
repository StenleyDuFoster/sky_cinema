using Android.Views;

namespace SkyCinema
{
    public interface IItemClickListener
    {
        void OnClick(View itemView, int i, bool isLongClick);
    }
}
