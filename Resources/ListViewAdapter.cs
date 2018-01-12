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

        public void SetFrActivity(Android.Support.V4.App.Fragment FrActivity)
        {
            this.activity = FrActivity;
        }

        public void SetList(List<Model.Task> lstPerson)
        {
            this.lstPerson = lstPerson;
        }



        private Android.Support.V4.App.Fragment activity;
        private List<Model.Task> lstPerson;

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

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            DataBase.db = DataBase.getDataBase();//new DataBase();
            DataBase.db.createDataBase();
            DataBase.db.createDataBaseExp();
            DataBase.db.createDataBaseBannedId();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);


            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view_dataTemplate, parent, false);

            var txtName = view.FindViewById<TextView>(Resource.Id.textView1);
            var txtPriority = view.FindViewById<TextView>(Resource.Id.textView2);
            var txtCategory = view.FindViewById<TextView>(Resource.Id.textView3);
            var txtDateTime = view.FindViewById<TextView>(Resource.Id.textView4);
            //var txtDone = view.FindViewById<TextView>(Resource.Id.textView5);
            var checkBox = view.FindViewById<CheckBox>(Resource.Id.chkBox);
            string getStatus;
            //Ставим пустой слушатель на проверочнуюкоробку
            checkBox.SetOnCheckedChangeListener(null);
            checkBox.Checked = lstPerson[position].Done;
            var selected_Element = DataBase.db.get_Element(lstPerson[position].Id)[0];

            checkBox.Click += delegate 
            {



                if(checkBox.Checked == true)
                {
                    lstPerson[position].Done = true;
                    Console.WriteLine("Name from list"+lstPerson[position].Name);
                    selected_Element.Done = true;
                    DataBase.db.updateTableTask(selected_Element);

                    var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db"));

                    var someData = connection.Query<Exp>("SELECT * FROM Exp");

                    BannedId bannedId = new BannedId()
                    {
                        Id = lstPerson[position].Id
                    };
                    var checkId = DataBase.db.findBannedId(lstPerson[position].Id);
                    Console.WriteLine("checkId = " + checkId.Count());

                    // ветка если БД еще не создана
                    if (someData.Count() == 0 )
                    {
                        Exp exp = new Exp()
                        {
                            Name = "Достижения",
                            EducationExp = 0,
                            MainExp = 0,
                            FinansiExp = 0,
                            OtherExp = 0,
                            
                            
                        };
                        connection.Insert(exp);
                        
                        someData = connection.Query<Exp>("SELECT * FROM Exp");
                        Console.WriteLine("MAIN EXP " + someData[0].MainExp);

                        if (checkId.Count() == 0)
                        {
                            var t = System.Threading.Tasks.Task.Factory.StartNew(() => Console.WriteLine("ну таск))"));
                            t.Wait();
                            t.Dispose();
                            DataBase.db.insertIntoTableBannedId(bannedId);
                            DataBase.db.updateTableExp(lstPerson[position].Category, lstPerson[position].Id);
                        }

                    }
                    // ветка если БД уже существует
                    else
                    {



                        if (checkId.Count() == 0)
                        {
                            var t = System.Threading.Tasks.Task.Factory.StartNew(() => {
                                DataBase.db.insertIntoTableBannedId(bannedId);
                                DataBase.db.updateTableExp(lstPerson[position].Category, lstPerson[position].Id);
                            });
                            t.Wait();
                            t.Dispose();                         
                        }
                    }
                }
                else
                {
                    lstPerson[position].Done = false;
                    selected_Element.Done = false;
                    var t = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        DataBase.db.updateTableTask(selected_Element);
                    });
                    t.Wait();
                    t.Dispose();
                    
                        _activityByListViewAdapter._fragment2.LoadData();
                   
                }
                _activityByListViewAdapter._fragment3.LoadExpData();
                //Обновляем звездочки в достижениях
                _activityByListViewAdapter._fragment3.LoadAchevementsData();


            };
            

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
            txtDateTime.Text = lstPerson[position].Time.ToShortTimeString() + ", " + lstPerson[position].Date.ToShortDateString();
            return view;
        }
    }
}