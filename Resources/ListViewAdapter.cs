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

namespace AndroidSQLite.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtName { get; set; }
        public TextView txtAge { get; set; }
        public TextView txtEmail { get; set; }
    }
    public class ListViewAdapter:BaseAdapter
    {
        private Android.Support.V4.App.Fragment activity;
        private List<Person> lstPerson;
        public ListViewAdapter(Android.Support.V4.App.Fragment activity, List<Person> lstPerson)
        {
            this.activity = activity;
            this.lstPerson = lstPerson;
        }

        public override int Count
        {
            get
            {
                return lstPerson.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstPerson[position].Id;
        }
        //public bool OnTouch(View view, MotionEvent motionEvent)
        //{
        //    if (motionEvent.Action == MotionEventActions.Down)
        //    {

        //        return true;
        //    }
        //    return false;
        //}


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view_dataTemplate, parent, false);

            var txtName = view.FindViewById<TextView>(Resource.Id.textView1);
            var txtAge = view.FindViewById<TextView>(Resource.Id.textView2);
            var txtEmail = view.FindViewById<TextView>(Resource.Id.textView3);

            txtName.Text = lstPerson[position].Name;
            txtAge.Text = ""+lstPerson[position].Age;
            txtEmail.Text = lstPerson[position].Email;

            return view;
        }
    }
}