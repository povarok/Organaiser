using System;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using AndroidSQLite.Resources.Model;
using AndroidSQLite.Resources.DataHelper;
using AndroidSQLite.Resources;
//using AndroidSQLite.BroadCast;
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
using AndroidSQLite.BroadCast;
using System.Threading.Tasks;
using Refractored.Fab;

namespace AndroidSQLite
{
    [Activity(Label = "AndroidSQLite", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity, IOnDateSetListener
    {

        



        public  Fragment1 _fragment1 = new Fragment1();
        public  Fragment2 _fragment2 = new Fragment2();
        public  Fragment3 _fragment3 = new Fragment3();


        public ListViewAdapter adapter = new ListViewAdapter();

        public DialogFragment1 _dialogFragment1 = new DialogFragment1();

        Java.Util.Calendar mCurrentDate;
        Bitmap mGenerateDateIcon;
        ImageGenerator mGeneratorImage;
        ImageView mDisplayGeneratedImage;
        
        //-------------------------------
        //Изменил на public, Было private
        public ViewPager mViewPager;   
        public SlidingTabScrollView mScrollView;
        //-------------------------------
        DateTime calendarDate;
       

        
        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            
            //DialogFragment1
            //Передаем выбранную дату куда нибудь
            Toast.MakeText(this, $"{dayOfMonth}-{month + 1}-{year}", ToastLength.Long).Show();
            mCurrentDate.Set(year, month, dayOfMonth);
            mGenerateDateIcon = mGeneratorImage.GenerateDateImage(mCurrentDate, Resource.Drawable.EmptyCalendar);
            mDisplayGeneratedImage.SetImageBitmap(mGenerateDateIcon);
            //Это нужно отдать методу LoadDataByDate()

            // ВОПРОС СЕМЕНОВУ: почему в datePicker месяц съехал на 1 ? ? ?

            calendarDate = new DateTime(year, month+1, dayOfMonth, 1, 1, 1);
            Console.WriteLine(calendarDate);
            mViewPager.SetCurrentItem(1, true);
           


            // Как было
            //Fragment2 fragment2 = new Fragment2();
            //fragment2.LoadDataByDate(calendarDate);

            // Семенов
            _fragment2.LoadDataByDate(calendarDate);

        }


        public void OnDismiss()
        {
            //Fragment2 fragment2 = new Fragment2();
            //fragment2.LoadData();
            //Console.WriteLine("ONDISMIS from MA");
        }

        protected override void OnCreate(Bundle bundle)
        {
            _fragment1.SetActivity(this);
            _fragment2.SetActivity(this);
            _fragment3.SetActivity(this);

            base.OnCreate(bundle);
            //dialogwindow.SetActivity(

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
            // Семенов
            mViewPager.Adapter = new SamplePagerAdapter(this, SupportFragmentManager);
            // Как было
            //mViewPager.Adapter = new SamplePagerAdapter( SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;

            //Create DataBase
            DataBase.db = DataBase.getDataBase();

            //db = new DataBase();
            DataBase.db.createDataBase();
            DataBase.db.createDataBaseAchivments();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            //var imgBtn = new ImageButton
            //var imgBtn = FindViewById<ImageButton>(Resource.Id.imgBtn);
            
            var settingsBtn = FindViewById<Button>(Resource.Id.settingsBtn);

            //var btnNot = FindViewById<Button>(Resource.Id.btncheeee);
            //------------------------------------------------------------------
            // Instantiate the builder and set notification elements:
            //Notification.Builder builder = new Notification.Builder(this)
            //    .SetContentTitle("Sample Notification")
            //    .SetContentText("Hello World! This is my first notification!")
            //    .SetSmallIcon(Resource.Drawable.ic_launcher);

            //builder.SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis());
            
            //// Build the notification:
            //Notification notification = builder.Build();

            //// Get the notification manager:
            //NotificationManager notificationManager =
            //    GetSystemService(Context.NotificationService) as NotificationManager;

            //// Publish the notification:
            //const int notificationId = 0;
            //notificationManager.Notify(notificationId, notification);
            //------------------------------------------------------------------

         

            //Пока что костыль
            settingsBtn.Click += delegate
            {
                Console.WriteLine("calendarDate= " + calendarDate.ToLongDateString());
                Console.WriteLine("SETTINGS BUTTON PRESSED");
                //Fragment2 fr2 = new Fragment2();
                //fr2.LoadDataByDate(calendarDate);
                
            };

            //imgBtn.Click += delegate
            //{

            //    Console.WriteLine("IMAGE BUTTON PRESSED");

            //};

        }

        
        // БУДИЛЬНИК
        //private void StartAlarm()
        //{
        //    AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
        //    Intent myIntent;
        //    PendingIntent pendingIntent;
        //    myIntent = new Intent(this, typeof(AlarmNotificationReceiver));
        //    pendingIntent = PendingIntent.GetBroadcast(this, 0, myIntent, 0);
        //    manager.Set(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + 3000, pendingIntent);
        //}
    }

    public class SamplePagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;
        // Семенов
        public SamplePagerAdapter(MainActivity activity, Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {

            mFragmentHolder = new List<Android.Support.V4.App.Fragment>
            {
                activity._fragment1,
                activity._fragment2,
                activity._fragment3
            };
        }
        // Как было
        //public SamplePagerAdapter(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        //{


        //    mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
        //    mFragmentHolder.Add(new Fragment1());
        //    mFragmentHolder.Add(new Fragment2());
        //    mFragmentHolder.Add(new Fragment3());
        //}

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
        public MainActivity _activity;

        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            var view = inflater.Inflate(Resource.Layout.FirstFragmentLayout, container, false);
            
            var img = view.FindViewById<ImageView>(Resource.Id.imageClick);

            //img.Clickable = true;

            //img.Click += delegate
            //{
            //    Console.WriteLine("image pressed");
            //};
           
            var house1 = view.FindViewById<LinearLayout>(Resource.Id.house1);
            var house2 = view.FindViewById<LinearLayout>(Resource.Id.house2);
            var house3 = view.FindViewById<LinearLayout>(Resource.Id.house3);
            var house4 = view.FindViewById<LinearLayout>(Resource.Id.house4);
            house1.Click += delegate
            {
                Console.WriteLine("H1 clicked");
                _activity.mViewPager.SetCurrentItem(1, true);
                _activity._fragment2.SortCategory("Спорт");
            };
            house2.Click += delegate
            {
                Console.WriteLine("H2 clicked");
                _activity.mViewPager.SetCurrentItem(1, true);
                _activity._fragment2.SortCategory("Прочее");
            };
            house3.Click += delegate
            {
                Console.WriteLine("H3 clicked");
                _activity.mViewPager.SetCurrentItem(1, true);
                _activity._fragment2.SortCategory("Образоване");
            };
            house4.Click += delegate
            {
                Console.WriteLine("H4 clicked");
                _activity.mViewPager.SetCurrentItem(1, true);
                _activity._fragment2.SortCategory("Финансы");
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
        public List<Person> lstSource = new List<Person>();
        //DataBase db;
        private EditText mTxt;
        public MainActivity _activity;

        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {



            //MainActivity ma = (MainActivity)this.Activity;
            DataBase.db = DataBase.getDataBase();
            DataBase.db.createDataBase();


            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            long elementId;
            var view = inflater.Inflate(Resource.Layout.SecondFragmentLayout, container, false);
            lstData = view.FindViewById<ListView>(Resource.Id.listView);

            var btnAdd = view.FindViewById<FloatingActionButton>(Resource.Id.btnAdd);
            var btnRefresh = view.FindViewById<Button>(Resource.Id.btnRefresh);
            var btnLoadDataByDateByLast = view.FindViewById<Button>(Resource.Id.btnLoadDataByDateByLast);

           
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
                var t = Task.Factory.StartNew(() =>
                {
                    DataBase.db.insertIntoTablePerson(person);
                });
                t.Wait();
                t.Dispose();
               
                

                Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                Android.Support.V4.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                Bundle frag_bundle = new Bundle();
                frag_bundle.PutLong("Id", person.Id);
                //frag_bundle.

                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);
                
                _activity._dialogFragment1 = DialogFragment1.NewInstance(frag_bundle);
                //newFr.Arguments.PutLong("Id", elementId);
                var act = _activity._dialogFragment1.Activity;

                _activity._dialogFragment1.SetActivity(_activity);
                _activity._dialogFragment1.Show(ft, "dialog");
                
            };

            btnRefresh.Click += delegate
            {
                LoadData();
            };

            // Эта кнопка берет последний элемент из БД и выводит на экран
            btnLoadDataByDateByLast.Click += delegate
            {

                var Last = DataBase.db.get_Last();
                Console.WriteLine("Last = " + Last.Id);
                LoadDataByDate(Last.Date);

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
                Console.WriteLine("ГАЛОЧКА");
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
                        var t = Task.Factory.StartNew(() =>
                        {
                            elementId = DataBase.db.selectQuery(lstData.Adapter.GetItemId(e.Position));
                            return elementId;
                        });
                        t.Wait();
                        
                        
                        //elementId = DataBase.db.selectQuery(lstData.Adapter.GetItemId(e.Position));
                        
                        Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                        //Remove fragment else it will crash as it is already added to backstack
                        Android.Support.V4.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                        Bundle frag_bundle = new Bundle();
                        frag_bundle.PutLong("Id", t.Result);
                        t.Dispose();

                        if (prev != null)
                        {
                            ft.Remove(prev);
                        }
                        ft.AddToBackStack(null);

                        

                       
                        _activity._dialogFragment1 = DialogFragment1.NewInstance(frag_bundle);
                        _activity._dialogFragment1.SetActivity(_activity);
                        //newFr.Arguments.PutLong("Id", elementId);

                        _activity._dialogFragment1.Show(ft, "dialog");
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
        public void StartAlarm(int Year, int Month, int Day, int Hour, int Minute)
        {
            var Current = Java.Lang.JavaSystem.CurrentTimeMillis();

            AlarmManager manager = (AlarmManager)_activity.GetSystemService(Context.AlarmService);
            Intent myIntent;
            PendingIntent pendingIntent;
            myIntent = new Intent(_activity, typeof(AlarmNotificationReceiver));
            pendingIntent = PendingIntent.GetBroadcast(_activity, 0, myIntent, 0);

            var unixTime = new DateTime(Year, Month, Day, Hour, Minute, 1);
            var CurrentTime = DateTime.Now;
            var AlarmTime = unixTime - CurrentTime;
            long ATMillis = (long)AlarmTime.TotalMilliseconds;
            Console.WriteLine("hours - " + Hour);
           // Console.WriteLine("Debug Clock" + (JavaSystem.CurrentTimeMillis()));
          //  Console.WriteLine("Debug ClockUNIX" + (unixTime));
//Console.WriteLine("Debug ClockYear " + Year + " Debug ClockMonth " + Month + " Debug ClockDay " + Day + " Debug ClockHour " + Hour + " Debug ClockMin " + Minute);
            //Console.WriteLine("Debug ClockYear" + (lYear  + Month + Day * 24 * 3600 * 1000 + Hour * 3600 * 1000 + Minute * 60000));
           // Console.WriteLine("Debug ClockCurr" + (CurrentTime));
            Console.WriteLine("Debug ClockDiff" + (ATMillis));
            
            manager.Set(AlarmType.RtcWakeup, JavaSystem.CurrentTimeMillis()+ATMillis, pendingIntent);

        }
        //Сортировка по id
        public void LoadData()
        {
            lstSource = DataBase.db.selectTablePerson();

            _activity.adapter.SetFrActivity(this);
            _activity.adapter.SetList(lstSource);
           _activity.adapter.SetActivity(_activity);
            lstData.Adapter = _activity.adapter;
        }
        //Сортировка по дате
        //Наверное так
        public void LoadDataByDate(DateTime date)
        {     
            //Console.WriteLine("LoadDataByDate started");
            lstSource = DataBase.db.selectTablePerson();
            Console.WriteLine("LoadDataByDate started LstSource length - "+ lstSource.Count);
            var lstSource2 = new List<Person>();
            foreach (var value in lstSource)
            {
                Console.WriteLine("from DB - " + value.Date.Date + "заданная дата - " + date.Date);
                if (value.Date.Date == date.Date)
                {
                    lstSource2.Add(value);
                    Console.WriteLine("длина lstSource2 - " + lstSource2.Count);

                }
            }

            this.lstSource = lstSource2;

            // lstSource.Find(person);
            _activity.adapter.SetFrActivity(this);
            _activity.adapter.SetList(lstSource);
            this.lstData.Adapter = _activity.adapter;
            Console.WriteLine("loadDataByDate completed " + lstSource2.Count);
        }
        //Передать категорию
        public void SortCategory(string _category)
        {
            lstSource = DataBase.db.selectTablePerson();
            var lstSource2 = new List<Person>();

            foreach(var value in lstSource)
            {
                if(value.Category == _category)
                {
                    lstSource2.Add(value);
                }
            }

            this.lstSource = lstSource2;
            _activity.adapter.SetFrActivity(this);
            _activity.adapter.SetList(lstSource);
            this.lstData.Adapter = _activity.adapter;

        }



    }

    public class Fragment3 : Android.Support.V4.App.Fragment
    {
        ProgressBar progressMain;
        ProgressBar progressSport;
        ProgressBar progressEducation;
        ProgressBar progressFinance;
        ProgressBar progressOther;

        TextView txtMain;
        TextView txtSport;
        TextView txtEducation;
        TextView txtFinance;
        TextView txtOther;

        private Button mButton;

        MainActivity _activity;


        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }

              
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
                      
            DataBase.db = DataBase.getDataBase();
            var ach = DataBase.db.getAchievments();
            
            //Console.WriteLine("Ach name = "+ach[0].Name + " Exp = " + ach[0].MainExp);
            var view = inflater.Inflate(Resource.Layout.ThirdFragmentLaout, container, false);
            //Button refreshBtn = view.FindViewById<Button>(Resource.Id.refresh);
            //TextView consos = view.FindViewById<TextView>(Resource.Id.consos);
            //consos.Text = ach[0].Name + "\n MainExp = " + ach[0].MainExp + "\n OtherExp = " + ach[0].OtherExp +
              //  "\n SportExp = " + ach[0].SportExp + "\n Finansi Exp = " + ach[0].FinansiExp + "\n EducationExp = " + ach[0].EducationExp;
            
            progressMain = view.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            progressSport = view.FindViewById<ProgressBar>(Resource.Id.progressBar2);
            progressEducation = view.FindViewById<ProgressBar>(Resource.Id.progressBar3);
            progressFinance = view.FindViewById<ProgressBar>(Resource.Id.progressBar4);
            progressOther = view.FindViewById<ProgressBar>(Resource.Id.progressBar5);

            txtMain = view.FindViewById<TextView>(Resource.Id.txtMain); 
            txtSport = view.FindViewById<TextView>(Resource.Id.txtSport);
            txtEducation = view.FindViewById<TextView>(Resource.Id.txtEducation);
            txtFinance = view.FindViewById<TextView>(Resource.Id.txtFinace);
            txtOther = view.FindViewById<TextView>(Resource.Id.txtOther);
            //progressBar.Progress = 1;
            //int _progress = 0;

            //Первичное заполнение опыта
            if (ach.Count != 0)
            {
                if (ach[0].MainExp <= 100) txtMain.Text = (ach[0].MainExp % 100).ToString() + " / 100";
                else txtMain.Text = (ach[0].MainExp).ToString() + " / " + ((1 + ach[0].MainExp / 100) * 100).ToString();

                if (ach[0].SportExp <= 100) txtSport.Text = (ach[0].SportExp % 100).ToString() + " / 100";
                else txtSport.Text = (ach[0].SportExp).ToString() + " / " + ((1 + ach[0].SportExp / 100) * 100).ToString();

                if (ach[0].EducationExp <= 100) txtEducation.Text = (ach[0].EducationExp % 100).ToString() + " / 100";
                else txtEducation.Text = (ach[0].EducationExp).ToString() + " / " + ((1 + ach[0].EducationExp / 100) * 100).ToString();

                if (ach[0].FinansiExp <= 100) txtFinance.Text = (ach[0].FinansiExp % 100).ToString() + " / 100";
                else txtFinance.Text = (ach[0].FinansiExp).ToString() + " / " + ((1 + ach[0].FinansiExp / 100) * 100).ToString();

                if (ach[0].OtherExp <= 100) txtOther.Text = (ach[0].OtherExp % 100).ToString() + " / 100";
                else txtOther.Text = (ach[0].OtherExp).ToString() + " / " + ((1 + ach[0].OtherExp / 100) * 100).ToString();

                //txtSport.Text = (ach[0].MainExp / 100).ToString() ;
                //txtEducation.Text = (ach[0].MainExp / 100).ToString();
                //txtFinance.Text = (ach[0].MainExp / 100).ToString();
                //txtOther.Text = (ach[0].MainExp / 100).ToString();

                progressMain.Progress = (ach[0].MainExp % 100);
                progressSport.Progress = (ach[0].SportExp % 100);
                progressEducation.Progress = (ach[0].EducationExp % 100);
                progressFinance.Progress = (ach[0].FinansiExp % 100);
                progressOther.Progress = (ach[0].OtherExp % 100);

            }

            //refreshBtn.Click += delegate
            //{

            //    ach = DataBase.db.getAchievments();
            //    i++;
            //    consos.Text ="Обновлено " + i + "раз\n" + ach[0].Name + "\n MainExp = " + ach[0].MainExp + "\n OtherExp = " + ach[0].OtherExp +
            //        "\n SportExp = " + ach[0].SportExp + "\n Finansi Exp = " + ach[0].FinansiExp + "\n EducationExp = " + ach[0].EducationExp;
            //    progressMainByString = ach[0].MainExp / 100;
            //    progressSportByString = ach[0].MainExp / 100;
            //    progressEducationByString = ach[0].MainExp / 100;
            //    progressFinanceByString = ach[0].MainExp / 100;
            //    progressOtherByString = ach[0].MainExp / 100;


            //    progressMain.Progress = ach[0].MainExp % 100;
            //    progressSport.Progress = ach[0].SportExp % 100;
            //    progressEducation.Progress = ach[0].EducationExp % 100;
            //    progressFinance.Progress = ach[0].FinansiExp % 100;
            //    progressOther.Progress = ach[0].OtherExp % 100;
            //};
            //mButton = view.FindViewById<Button>(Resource.Id.btncheeee);
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

        public void LoadDataByDateByFragment3()
        {
            var ach = DataBase.db.getAchievments();
            if (ach.Count != 0)
            {
                if (ach[0].MainExp <= 100) txtMain.Text = (ach[0].MainExp % 100).ToString() + " / 100";
                else txtMain.Text = (ach[0].MainExp).ToString() + " / " + ((1 + ach[0].MainExp / 100) * 100).ToString();

                if (ach[0].SportExp <= 100) txtSport.Text = (ach[0].SportExp % 100).ToString() + " / 100";
                else txtSport.Text = (ach[0].SportExp).ToString() + " / " + ((1 + ach[0].SportExp / 100) * 100).ToString();

                if (ach[0].EducationExp <= 100) txtEducation.Text = (ach[0].EducationExp % 100).ToString() + " / 100";
                else txtEducation.Text = (ach[0].EducationExp).ToString() + " / " + ((1 + ach[0].EducationExp / 100) * 100).ToString();

                if (ach[0].FinansiExp <= 100) txtFinance.Text = (ach[0].FinansiExp % 100).ToString() + " / 100";
                else txtFinance.Text = (ach[0].FinansiExp).ToString() + " / " + ((1 + ach[0].FinansiExp / 100) * 100).ToString();

                if (ach[0].OtherExp <= 100) txtOther.Text = (ach[0].OtherExp % 100).ToString() + " / 100";
                else txtOther.Text = (ach[0].OtherExp).ToString() + " / " + ((1 + ach[0].OtherExp / 100) * 100).ToString();

                progressMain.Progress = ach[0].MainExp % 100;
                progressSport.Progress = ach[0].SportExp % 100;
                progressEducation.Progress = ach[0].EducationExp % 100;
                progressFinance.Progress = ach[0].FinansiExp % 100;
                progressOther.Progress = ach[0].OtherExp % 100;
            }
            Console.WriteLine("Ach count = "+ach.Count);
        }
    }
}