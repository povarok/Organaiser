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
        DataBase db;

        


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

            db = new DataBase();
            db.createDataBase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);


            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view_dataTemplate, parent, false);

            var txtName = view.FindViewById<TextView>(Resource.Id.textView1);
            var txtPriority = view.FindViewById<TextView>(Resource.Id.textView2);
            var txtCategory = view.FindViewById<TextView>(Resource.Id.textView3);
            var txtDateTime = view.FindViewById<TextView>(Resource.Id.textView4);
            var txtDone = view.FindViewById<TextView>(Resource.Id.textView5);
            var checkBox = view.FindViewById<CheckBox>(Resource.Id.chkBox);
            string getStatus;
            //Ставим пустой слушатель на проверочнуюкоробку
            checkBox.SetOnCheckedChangeListener(null);
            checkBox.Checked = lstPerson[position].Done;
            checkBox.SetOnCheckedChangeListener(new CheckedChangeListener(this.activity));


            var selected_Element = db.get_Element(lstPerson[position].Id)[0];

            checkBox.Click += delegate 
            {
                if(checkBox.Checked == true)
                {
                    lstPerson[position].Done = true;
                    Console.WriteLine("Name from list"+lstPerson[position].Name);
                    selected_Element.Done = true;
                    db.updateTablePerson(selected_Element);
                }
                else
                {
                    lstPerson[position].Done = false;
                    selected_Element.Done = false;
                    db.updateTablePerson(selected_Element);

                }
            };
            //Не работает
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
                getStatus = "Выполнено";
            }
            else
            {
                getStatus = "Не выполнено";
            }

            txtName.Text = lstPerson[position].Name;
            txtPriority.Text ="Приоритет: " + lstPerson[position].Priority;
            txtCategory.Text = "Категория: " + lstPerson[position].Category;
            txtDateTime.Text = lstPerson[position].Time.ToShortTimeString() + ", " + lstPerson[position].Date.ToLongDateString();
            txtDone.Text = "Статус: " + getStatus;


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