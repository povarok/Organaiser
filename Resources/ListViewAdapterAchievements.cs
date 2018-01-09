using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidSQLite.Resources.Model;
using Java.Lang;
using SQLite;
using Android.Support.V4.App;
using Android.Support.V4.View;
using AndroidSQLite.Resources.DataHelper;
using Android.Util;
using System.Threading.Tasks;

namespace AndroidSQLite.Resources
{
    
    public class ViewHolderAchievements : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Description { get; set; }
        public TextView Type { get; set; }
        public ImageView Stars { get; set; }
    }
    public class ListViewAdapterAchievements : BaseAdapter
    {

        public MainActivity _activityByListViewAdapter;

        public void SetActivity(MainActivity activity)
        {
            _activityByListViewAdapter = activity;
        }

        public void SetFrActivity(Android.Support.V4.App.Fragment FrActivity)
        {
            this.activity = FrActivity;
        }

        public void SetList(List<Achievement> lstAch)
        {
            this.lstAch = lstAch;
        }



        private Android.Support.V4.App.Fragment activity;
        private List<Achievement> lstAch;

        public override int Count
        {
            get
            {
                return lstAch.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            DataBase.db = DataBase.getDataBase();
            DataBase.db.createDataBaseExp();   
            DataBase.db.createDataBaseAchivment();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);


            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view_achTemplate, parent, false);

            var Name = view.FindViewById<TextView>(Resource.Id.tName);
            var Description = view.FindViewById<TextView>(Resource.Id.tDescription);
            var Star = view.FindViewById<ImageView>(Resource.Id.Star);
            //Ставим нужное количество звезд за опыт пользователя
            if (lstAch[position].Stars == 0)
            {
                Star.SetImageResource(Resource.Drawable.star0);
            }
            else if (lstAch[position].Stars == 1)
            {
                Star.SetImageResource(Resource.Drawable.star1);
            }
            else if (lstAch[position].Stars == 2)
            {
                Star.SetImageResource(Resource.Drawable.star2);

            }
            else
            {
                Star.SetImageResource(Resource.Drawable.star3);
            }

            Name.Text = lstAch[position].Name;
            Description.Text = lstAch[position].Description;

            return view;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }
    }
}