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

        public MainActivity _activityByListViewAdapter;

        public void SetActivity(MainActivity activity)
        {
            _activityByListViewAdapter = activity;
        }




        //DataBase db;

        public void SetFrActivity(Android.Support.V4.App.Fragment FrActivity)
        {
            this.activity = FrActivity;
        }

        public void SetList(List<Person> lstPerson)
        {
            this.lstPerson = lstPerson;
        }



        private Android.Support.V4.App.Fragment activity;
        private List<Person> lstPerson;


        //public ListViewAdapter(Android.Support.V4.App.Fragment activity, List<Person> lstPerson)
        //{
        //    this.activity = activity;
        //    this.lstPerson = lstPerson;
        //}

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

            DataBase.db = DataBase.getDataBase();//new DataBase();
            DataBase.db.createDataBase();
            DataBase.db.createDataBaseAchivments();
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
            //checkBox.SetOnCheckedChangeListener(new CheckedChangeListener(this.activity));


            var selected_Element = DataBase.db.get_Element(lstPerson[position].Id)[0];

            checkBox.Click += delegate 
            {
                if(checkBox.Checked == true)
                {
                    lstPerson[position].Done = true;
                    Console.WriteLine("Name from list"+lstPerson[position].Name);
                    selected_Element.Done = true;
                    DataBase.db.updateTablePerson(selected_Element);

                    _activityByListViewAdapter._fragment2.LoadData();

                    //--------------------------
                    var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db"));

                    var someData = connection.Query<Achievement1>("SELECT * FROM Achievement1");
                    Console.WriteLine("SOMEDATA = " + someData);

                    if (someData.Count() == 0)
                    {
                        Achievement1 achievement = new Achievement1()
                        {
                            Name = "Достижения",
                            EducationExp = 0,
                            MainExp = 0,
                            FinansiExp = 0,
                            OtherExp = 0,
                            //BannedId = {}
                            
                        };
                        connection.Insert(achievement);
                        DataBase.db.updateTableAchievements(lstPerson[position].Category, lstPerson[position].Id);
                        someData = connection.Query<Achievement1>("SELECT * FROM Achievement1");
                        Console.WriteLine("MAIN EXP " + someData[0].MainExp);

                    }

                    else
                    {


                        DataBase.db.updateTableAchievements(lstPerson[position].Category, lstPerson[position].Id);
                        Console.WriteLine("MAIN EXP " + someData[0].MainExp);
                    }
   
                }
                else
                {
                    lstPerson[position].Done = false;
                    selected_Element.Done = false;
                    DataBase.db.updateTablePerson(selected_Element);
                    _activityByListViewAdapter._fragment2.LoadData();

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

            //_activityByListViewAdapter._fragment2.LoadData();


            return view;
        }

        //public class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        //{
        //    //private Activity activity;
        //    private Android.Support.V4.App.Fragment activity;

        //    public CheckedChangeListener(Android.Support.V4.App.Fragment activity)
        //    {
        //        this.activity = activity;
        //    }

        //    public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        //    {
                
        //        if (isChecked)
        //        {
                    
        //            Console.WriteLine("CheckedListiner");
        //            //Toast.MakeText(this.activity, "Checked Listiner", ToastLength.Short).Show();
        //        }
        //    }
        //}
    }
}