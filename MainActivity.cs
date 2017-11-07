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
using Android.Database;
using Android.Support.V7.App;
using System.Globalization;
using Android.Graphics;
using Com.KD.Dynamic.Calendar.Generator;
using Java.Util;
using static Android.App.DatePickerDialog;
using Java.Lang;

namespace AndroidSQLite
{
    [Activity(Label = "AndroidSQLite", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity, IOnDateSetListener
    {
        Java.Util.Calendar mCurrentDate;
        Bitmap mGenerateDateIcon;
        ImageGenerator mGeneratorImage;
        ImageView mDisplayGeneratedImage;
        //ListView lstData ;
        //List<Person> lstSource = new List<Person>();
        DataBase db;
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            //Передаем выбранную дату куда нибудь
            Toast.MakeText(this, $"{dayOfMonth}-{month + 1}-{year}", ToastLength.Long).Show();
            mCurrentDate.Set(year, month, dayOfMonth);
            mGenerateDateIcon = mGeneratorImage.GenerateDateImage(mCurrentDate, Resource.Drawable.EmptyCalendar);
            mDisplayGeneratedImage.SetImageBitmap(mGenerateDateIcon);

        }


        public void OnDismiss()
        {
            Fragment2 fragment2 = new Fragment2();
            fragment2.LoadData();
            Console.WriteLine("ONDISMIS from MA");
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            mGeneratorImage = new ImageGenerator(this);
            mDisplayGeneratedImage = FindViewById<ImageView>(Resource.Id.imageGen);
            mCurrentDate = Java.Util.Calendar.Instance;

            mGeneratorImage.SetIconSize(50, 50);
            mGeneratorImage.SetDateSize(30);
            mGeneratorImage.SetMonthSize(10);
            mGeneratorImage.SetDatePosition(42);
            mGeneratorImage.SetMonthPosition(14);
            mGeneratorImage.SetDateColor(Color.ParseColor("#3c6eaf"));
            mGeneratorImage.SetMonthColor(Color.White);
            mGeneratorImage.SetStorageToSDCard(true);

            //Первичная отрисовка иконки календаря
            int primaryYear = mCurrentDate.Get(CalendarField.Year);
            int primaryMonth = mCurrentDate.Get(CalendarField.Month);
            int primaryDay = mCurrentDate.Get(CalendarField.DayOfMonth);

            mCurrentDate.Set(primaryYear, primaryMonth, primaryDay);
            mGenerateDateIcon = mGeneratorImage.GenerateDateImage(mCurrentDate, Resource.Drawable.EmptyCalendar);
            mDisplayGeneratedImage.SetImageBitmap(mGenerateDateIcon);

            mDisplayGeneratedImage.Click += delegate
            {

                //mCurrentDate = Java.Util.Calendar.Instance;
               
                int mYear = mCurrentDate.Get(CalendarField.Year);
                int mMonth = mCurrentDate.Get(CalendarField.Month);
                int mDay = mCurrentDate.Get(CalendarField.DayOfMonth);
                //Android.App.AlertDialog.ThemeDeviceDefaultDark
                DatePickerDialog datePickerDialog = new DatePickerDialog(this, Android.App.AlertDialog.ThemeDeviceDefaultDark, this, mYear, mMonth, mDay);
                datePickerDialog.Show();



            };

            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            mViewPager.Adapter = new SamplePagerAdapter( SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;

            //Create DataBase
            db = new DataBase();
            db.createDataBase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            //var imgBtn = new ImageButton
            //var imgBtn = FindViewById<ImageButton>(Resource.Id.imgBtn);

            var settingsBtn = FindViewById<Button>(Resource.Id.settingsBtn);
            


            settingsBtn.Click += delegate
            {

                Console.WriteLine("SETTINGS BUTTON PRESSED");
            };

            //imgBtn.Click += delegate
            //{

            //    Console.WriteLine("IMAGE BUTTON PRESSED");

            //};

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
            
            var img = view.FindViewById<ImageView>(Resource.Id.imageClick);

            img.Clickable = true;

            img.Click += delegate
            {
                Console.WriteLine("image pressed");
            };

            var house1 = view.FindViewById<LinearLayout>(Resource.Id.house1);
            house1.Click += delegate
            {
                Console.WriteLine("Layout clicked");
            };

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
            lstData = view.FindViewById<ListView>(Resource.Id.listView);

            //var edtName = view.FindViewById<EditText>(Resource.Id.edtName);
            //var edtAge = view.FindViewById<EditText>(Resource.Id.edtAge);
            //var edtEmail = view.FindViewById<EditText>(Resource.Id.edtEmail);

            var btnAdd = view.FindViewById<Button>(Resource.Id.btnAdd);
            //var btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            //var btnDelete = view.FindViewById<Button>(Resource.Id.btnDelete);
            var btnRefresh = view.FindViewById<Button>(Resource.Id.btnRefresh);

            //LoadData
            LoadData();

            //Event
            btnAdd.Click += delegate
            {
                //Создаем пустую запись в бд

                Person person = new Person()
                {
                    Name = "",
                    Date = DateTime.Now,
                    Time = DateTime.Now, 
                    Description = "", 
                    Category = "Спорт", 
                    Priority = "0", 
                    Done = false
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
            //btnDelete.Click += delegate {
            //    Person person = new Person()
            //    {
            //        Id = int.Parse(edtName.Tag.ToString()),
            //        Name = edtName.Text,
            //        Age = int.Parse(edtAge.Text),
            //        Email = edtEmail.Text
            //    };
            //    db.deleteTablePerson(person);
            //    LoadData();
            //};

            

            btnRefresh.Click += delegate
            {
                //lstData = view.FindViewById<ListView>(Resource.Id.listView);
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
                        Console.WriteLine("Click element");
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
                var checkedBox = e.View.FindViewById<CheckBox>(Resource.Id.chkBox);
                var txtDone = e.View.FindViewById<TextView>(Resource.Id.textView5);
                checkedBox.SetOnCheckedChangeListener(null);

                
               
                //var txtAge = e.View.FindViewById<TextView>(Resource.Id.textView2);
                //var txtEmail = e.View.FindViewById<TextView>(Resource.Id.textView3);

                //edtName.Text = txtName.Text;
                //edtName.Tag = e.Id;

                //edtAge.Text = txtAge.Text;

                //edtEmail.Text = txtEmail.Text;

            };


            return view;
        }

        

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Fragment 2";
        }

        //ListView lstData;
        
        public void LoadData()
        {
            lstSource = db.selectTablePerson();
            var adapter = new ListViewAdapter(this, lstSource);
            //Console.WriteLine("LoadData" );
            //Console.WriteLine("lstdata " + lstData);
            //Console.WriteLine("adapter "+adapter);
            
            //Console.WriteLine("массив с элементами" + db.get_Last()[0].Name);

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