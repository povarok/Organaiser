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
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace AndroidSQLite
{
    [Activity(Label = "AndroidSQLite", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {
        //ListView lstData ;
        //List<Person> lstSource = new List<Person>();
        DataBase db;
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            mViewPager.Adapter = new SamplePagerAdapter(SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;

            //Create DataBase
            db = new DataBase();
            db.createDataBase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

           
            

          

        }
       

        //public void LoadData()
        //{
        //    lstSource = db.selectTablePerson();
        //    var adapter = new ListViewAdapter(this, lstSource);
        //    lstData.Adapter = adapter;
        //}
    }

    public class SamplePagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;

        public SamplePagerAdapter(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {
            mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
            mFragmentHolder.Add(new Fragment1());
            mFragmentHolder.Add(new Fragment2());
            mFragmentHolder.Add(new Fragment3());
        }

        public override int Count
        {
            get { return mFragmentHolder.Count; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return mFragmentHolder[position];
        }
    }

    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        private TextView mTextView;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FirstFragmentLayout, container, false);

           
            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Fragment 1";
        }
    }

    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        ListView lstData;
        List<Person> lstSource = new List<Person>();
        DataBase db;
        private EditText mTxt;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           // MainActivity ma = (MainActivity)this.Activity;
            db = new DataBase();
            db.createDataBase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            long elementId;
            var view = inflater.Inflate(Resource.Layout.SecondFragmentLayout, container, false);
            var lstData = view.FindViewById<ListView>(Resource.Id.listView);

            var edtName = view.FindViewById<EditText>(Resource.Id.edtName);
            var edtAge = view.FindViewById<EditText>(Resource.Id.edtAge);
            var edtEmail = view.FindViewById<EditText>(Resource.Id.edtEmail);

            var btnAdd = view.FindViewById<Button>(Resource.Id.btnAdd);
            //var btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            var btnDelete = view.FindViewById<Button>(Resource.Id.btnDelete);

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

                Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                Android.Support.V4.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
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
            lstData.ItemClick += (s, e) => {
                //lstData
                //Set background for selected item
                for (int i = 0; i < lstData.Count; i++)
                {
                    if (e.Position == i)
                    {
                        //спросить про Intent
                        //  Intent intent = new Intent(this,  )
                        // lstData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.DarkGray);
                        //Получаем id выбранного в списке элемента
                        elementId = db.selectQuery(lstData.Adapter.GetItemId(e.Position));
                        Console.WriteLine("Выбран элемент с id= " + elementId);

                        Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                        //Remove fragment else it will crash as it is already added to backstack
                        Android.Support.V4.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
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


            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Fragment 2";
        }


        public void LoadData()
        {
            lstSource = db.selectTablePerson();
            var adapter = new ListViewAdapter(this, lstSource);
            lstData.Adapter = adapter;
        }

    }

    public class Fragment3 : Android.Support.V4.App.Fragment
    {
        private Button mButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ThirdFragmentLaout, container, false);

            //mButton = view.FindViewById<Button>(Resource.Id.button1);
            //mButton.Click += delegate
            //{

            //    Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
            //    //Remove fragment else it will crash as it is already added to backstack
            //    Android.Support.V4.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            //    if (prev != null)
            //    {
            //        ft.Remove(prev);
            //    }
            //    ft.AddToBackStack(null);
            //    // Create and show the dialog.
            //    DialogFragment1 newFragment = DialogFragment1.NewInstance(null);
            //    //Add fragment
            //    newFragment.Show(ft, "dialog");

            //};
            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Fragment 3";
        }
    }
}