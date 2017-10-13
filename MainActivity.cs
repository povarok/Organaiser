using System;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using AndroidSQLite.Resources.Model;
using AndroidSQLite.Resources.DataHelper;
using AndroidSQLite.Resources;
using Android.Util;
using SQLite;
using Android.Content;
using static Android.Views.View;
using Android.Views;

namespace AndroidSQLite
{
    [Activity(Label = "AndroidSQLite", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ListView lstData;
        List<Person> lstSource = new List<Person>();
        DataBase db;
       
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            long elementId;
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Create DataBase
            db = new DataBase();
            db.createDataBase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            lstData = FindViewById<ListView>(Resource.Id.listView);

            var edtName = FindViewById<EditText>(Resource.Id.edtName);
            var edtAge = FindViewById<EditText>(Resource.Id.edtAge);
            var edtEmail = FindViewById<EditText>(Resource.Id.edtEmail);

            var btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            //var btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            var btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
            

            //LoadData
            LoadData();

            //Event
            btnAdd.Click += delegate
            {
                

                Person person = new Person()
                {
                    Name = "",
                    Age = 0,
                    Email = ""
                };
                db.insertIntoTablePerson(person);
                //LoadData();

                FragmentTransaction ft = FragmentManager.BeginTransaction();
                Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                Bundle frag_bundle = new Bundle();
                frag_bundle.PutLong("Id", person.Id);


                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);
                DialogFragment1 newFr = DialogFragment1.NewInstance(frag_bundle);
                //newFr.Arguments.PutLong("Id", elementId);


                newFr.Show(ft, "dialog");

            };

            //btnEdit.Click += delegate {
            //    Person person = new Person()
            //    {
            //        Id=int.Parse(edtName.Tag.ToString()),
            //        Name = edtName.Text,
            //        Age = int.Parse(edtAge.Text),
            //        Email = edtEmail.Text
            //    };
            //    db.updateTablePerson(person);
            //    LoadData();
            //};
            //lstData.SetOnTouchListener(new IOnTouchListener)
            btnDelete.Click += delegate {
                Person person = new Person()
                {
                    Id = int.Parse(edtName.Tag.ToString()),
                    Name = edtName.Text,
                    Age = int.Parse(edtAge.Text),
                    Email = edtEmail.Text
                };
                db.deleteTablePerson(person);
                LoadData();
            };
            //Эдик разбирайся
            //Хрень для свайпа10!)
            //lstData.Touch += (s, e) =>
            //{
            //    var handled = false;
            //    if (e.Event.Action == OverScroll)//MotionEventActions.Down)
            //    {
            //        // do stuff
            //        Console.WriteLine("Doim stuff");
            //        handled = true;
            //    }
                

            //    e.Handled = handled;
            //};
            lstData.ItemClick += (s,e) =>{
                //lstData
                //Set background for selected item
                for(int i = 0; i < lstData.Count; i++)
                {
                    if (e.Position == i)
                    {
                        //спросить про Intent
                      //  Intent intent = new Intent(this,  )
                       // lstData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.DarkGray);
                       //Получаем id выбранного в списке элемента
                        elementId = db.selectQuery(lstData.Adapter.GetItemId(e.Position));
                        Console.WriteLine("Выбран элемент с id= " + elementId);

                        FragmentTransaction ft = FragmentManager.BeginTransaction();
                        Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                        Bundle frag_bundle = new Bundle();
                        frag_bundle.PutLong("Id", elementId);
                        
                        
                        if (prev != null)
                        {
                            ft.Remove(prev);
                        }
                        ft.AddToBackStack(null);
                        DialogFragment1 newFr = DialogFragment1.NewInstance(frag_bundle);
                        //newFr.Arguments.PutLong("Id", elementId);
                        
                        
                        newFr.Show(ft, "dialog");
                        
                        //Toast.MakeText(this, db.get_Element(elementId)[0].Name, ToastLength.Long).Show();


                    }


                    //else
                    //    lstData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);

                }




                //Binding Data
                var txtName = e.View.FindViewById<TextView>(Resource.Id.textView1);
                var txtAge = e.View.FindViewById<TextView>(Resource.Id.textView2);
                var txtEmail = e.View.FindViewById<TextView>(Resource.Id.textView3);

                edtName.Text = txtName.Text;
                edtName.Tag = e.Id;

                edtAge.Text = txtAge.Text;

                edtEmail.Text = txtEmail.Text;

            };

        }
       

        public void LoadData()
        {
            lstSource = db.selectTablePerson();
            var adapter = new ListViewAdapter(this, lstSource);
            lstData.Adapter = adapter;
        }
    }
}