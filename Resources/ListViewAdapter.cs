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
    public class ListViewAdapter : BaseAdapter
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

        public void SetList(List<Model.Task> lstTask)
        {
            this.lstSource = lstTask;
        }



        private Android.Support.V4.App.Fragment activity;
        private List<Model.Task> lstSource;

        public override int Count
        {
            get
            {
                return lstSource.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstSource[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            DataBase.db = DataBase.getDataBase();
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
            checkBox.Checked = lstSource[position].Done;
            var selected_Element = DataBase.db.get_Element(lstSource[position].Id)[0];

            checkBox.Click += delegate
            {
                Console.WriteLine("нажата галка");
                if (checkBox.Checked == true)
                {
                    lstSource[position].Done = true;
                    Console.WriteLine("Name from list" + lstSource[position].Name);
                    selected_Element.Done = true;
                    DataBase.db.updateTableTask(selected_Element);

                    var someData = DataBase.db.selectTableTask();

                    BannedId bannedId = new BannedId()
                    {
                        Id = lstSource[position].Id
                    };
                    var checkId = DataBase.db.findBannedId(lstSource[position].Id);
                    Console.WriteLine("checkId = " + checkId.Count());

                    // ветка если БД еще не создана
                    if (someData.Count() == 0)
                    {
                        Console.WriteLine("заходим в ветку бд не создана");
                        Exp exp = new Exp()
                        {
                            Name = "Достижения",
                            EducationExp = 0,
                            MainExp = 0,
                            FinansiExp = 0,
                            OtherExp = 0,

                        };
                        DataBase.db.insertExp(exp);


                        someData = DataBase.db.selectTableTask();
                        //если в БД с забаненными id ничего нет то добавляем текущий id в нее и начисляем опыт
                        if (checkId.Count() == 0)
                        {
                            //var t = System.Threading.Tasks.Task.Factory.StartNew(() => {
                            DataBase.db.insertIntoTableBannedId(bannedId);
                            DataBase.db.updateTableExp(lstSource[position].Category, lstSource[position].Id);
                            //});                        
                            //t.Wait();
                            //t.Dispose();
                            _activityByListViewAdapter._fragment3.LoadExpData();
                            _activityByListViewAdapter._fragment3.LoadAchevementsData();

                        }

                    }
                    // ветка если БД уже существует
                    else
                    {
                        Console.WriteLine("заходм в ветку где бд создана");
                        if (checkId.Count() == 0)
                        {
                            //var t = System.Threading.Tasks.Task.Factory.StartNew(() => {
                            DataBase.db.insertIntoTableBannedId(bannedId);
                            DataBase.db.updateTableExp(lstSource[position].Category, lstSource[position].Id);
                            //});
                            //t.Wait();
                            //t.Dispose();
                            _activityByListViewAdapter._fragment3.LoadExpData();

                            _activityByListViewAdapter._fragment3.LoadAchevementsData();
                        }
                    }
                }
                else
                {
                    lstSource[position].Done = false;
                    selected_Element.Done = false;
                    var t = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        DataBase.db.updateTableTask(selected_Element);
                    });
                    t.Wait();
                    t.Dispose();

                    _activityByListViewAdapter._fragment2.LoadData();

                }

                //Обновляем звездочки в достижениях

            };


            if (lstSource[position].Done == true)
            {
                getStatus = "Выполнено";
            }
            else
            {
                getStatus = "Не выполнено";
            }

            txtName.Text = lstSource[position].Name;
            txtPriority.Text = "Приоритет: " + lstSource[position].Priority;
            txtCategory.Text = "Категория: " + lstSource[position].Category;
            txtDateTime.Text = lstSource[position].Time.ToShortTimeString() + ", " + lstSource[position].Date.ToShortDateString();
            return view;
        }
    }
}