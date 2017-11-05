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
        public TextView txtPriority { get; set; }
        public TextView txtCategory { get; set; }
        public TextView txtDateTime { get; set; }
        public TextView txtDone { get; set; }
        public CheckBox checkBox { get; set; }
        
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
            var txtPriority = view.FindViewById<TextView>(Resource.Id.textView2);
            var txtCategory = view.FindViewById<TextView>(Resource.Id.textView3);
            var txtDateTime = view.FindViewById<TextView>(Resource.Id.textView4);
            var txtDone = view.FindViewById<TextView>(Resource.Id.textView5);
            var checkBox = view.FindViewById<CheckBox>(Resource.Id.chkBox);
            string getStatus;
            //������ ������ ��������� �� ������������������
            checkBox.SetOnCheckedChangeListener(null);
            checkBox.Checked = lstPerson[position].Done;
            checkBox.SetOnCheckedChangeListener(new CheckedChangeListener(this.activity));


            //�� ��������
            //if(checkBox.Checked == false)
            //{
            //    lstPerson[position].Done = false;
                
            //}
            //else
            //{
            //    lstPerson[position].Done = true;
            //    Console.WriteLine(lstPerson[position].Name + " 231");
            //}

            if (lstPerson[position].Done == true)
            {
                getStatus = "���������";
            }
            else
            {
                getStatus = "�� ���������";
            }

            txtName.Text = lstPerson[position].Name;
            txtPriority.Text ="���������: " + lstPerson[position].Priority;
            txtCategory.Text = "���������: " + lstPerson[position].Category;
            txtDateTime.Text = lstPerson[position].Time.ToShortTimeString() + ", " + lstPerson[position].Date.ToLongDateString();
            txtDone.Text = "������: " + getStatus;


            return view;
        }

        public class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        {
            //private Activity activity;
            private Android.Support.V4.App.Fragment activity;

            public CheckedChangeListener(Android.Support.V4.App.Fragment activity)
            {
                this.activity = activity;
            }

            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                
                if (isChecked)
                {
                    
                    Console.WriteLine("CheckedListiner");
                    //Toast.MakeText(this.activity, "Checked Listiner", ToastLength.Short).Show();
                }
            }
        }
    }
}