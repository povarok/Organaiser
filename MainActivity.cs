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
using AndroidSQLite.BroadCast;
using System.Threading.Tasks;
using Refractored.Fab;

namespace AndroidSQLite
{
    [Activity(Label = "BuildUp", MainLauncher = true, Icon = "@drawable/app_icon")]
    public class MainActivity : FragmentActivity, IOnDateSetListener
    {
        //Основные экраны приложения
        public Fragment1 _fragment1 = new Fragment1();
        public Fragment2 _fragment2 = new Fragment2();
        public Fragment3 _fragment3 = new Fragment3();


        public ListViewAdapter adapter = new ListViewAdapter();
        public ListViewAdapterAchievements achievments_adapter = new ListViewAdapterAchievements();
        //Диалоговые окна
        public DialogWindow _taskWindow = new DialogWindow();
        public SettingsWindow _settingsWindow = new SettingsWindow();

        Java.Util.Calendar mCurrentDate;
        Bitmap mGenerateDateIcon;
        ImageGenerator mGeneratorImage;
        ImageView mDisplayGeneratedImage;

        public ViewPager mViewPager;
        public SlidingTabScrollView mScrollView;
        DateTime calendarDate;



        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            //Передаем выбранную дату
            Toast.MakeText(this, $"{dayOfMonth}-{month + 1}-{year}", ToastLength.Long).Show();
            mCurrentDate.Set(year, month, dayOfMonth);
            mGenerateDateIcon = mGeneratorImage.GenerateDateImage(mCurrentDate, Resource.Drawable.EmptyCalendar);
            calendarDate = new DateTime(year, month + 1, dayOfMonth, 0, 0, 0);
            mViewPager.SetCurrentItem(1, true);
            _fragment2.SortByDate(calendarDate);

        }

        protected override void OnCreate(Bundle bundle)
        {
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            _fragment1.SetActivity(this);
            _fragment2.SetActivity(this);
            _fragment3.SetActivity(this);
            _settingsWindow.SetActivity(this);
            base.OnCreate(bundle);
            //dialogwindow.SetActivity(

            SetContentView(Resource.Layout.Main);
            mGeneratorImage = new ImageGenerator(this);
            //mDisplayGeneratedImage = FindViewById<ImageView>(Resource.Id.imageGen);
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
            //REPAIR PLEASE
            mCurrentDate.Set(primaryYear, primaryMonth, primaryDay);
            mGenerateDateIcon = mGeneratorImage.GenerateDateImage(mCurrentDate, Resource.Drawable.EmptyCalendar);
            //mDisplayGeneratedImage.SetImageBitmap(mGenerateDateIcon);
            //var drawCalendar = mDisplayGeneratedImage.Background;
            FloatingActionButton btnCalendar = FindViewById<FloatingActionButton>(Resource.Id.btnCalendar);
            //btnCalendar.SetBackgroundDrawable(drawCalendar);
            //Кнопка для сортировки задач по дате
            btnCalendar.Click += delegate
            {

                //mCurrentDate = Java.Util.Calendar.Instance;

                int mYear = mCurrentDate.Get(CalendarField.Year);
                int mMonth = mCurrentDate.Get(CalendarField.Month);
                int mDay = mCurrentDate.Get(CalendarField.DayOfMonth);
                DatePickerDialog datePickerDialog = new DatePickerDialog(this, Android.App.AlertDialog.ThemeDeviceDefaultDark, this, mYear, mMonth, mDay);
                datePickerDialog.Show();



            };

            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            mViewPager.Adapter = new SamplePagerAdapter(this, SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;

            //Получаем экземпляр бд
            DataBase.db = DataBase.getDataBase();
            DataBase.db.createDataBase();
            DataBase.db.createDataBaseExp();
            DataBase.db.createDataBaseSettings();
            //Первичные настройки приложения
            if (DataBase.db.getSettings().Count == 0)
            {
                DataBase.db.insertStartSettings();
            }
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            //var imgBtn = new ImageButton
            //var imgBtn = FindViewById<ImageButton>(Resource.Id.imgBtn);

            FloatingActionButton settingsBtn = FindViewById<FloatingActionButton>(Resource.Id.settingsBtn);

            settingsBtn.Click += delegate
            {
                //Создаем окно для добавления новой задачи
                Android.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                Android.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                //Передаем id новой заметки для корректной записи в бд
                Bundle frag_bundle = new Bundle();
                //frag_bundle.PutLong("Id", task.Id);

                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);

                _settingsWindow = SettingsWindow.NewInstance(frag_bundle);
                var act = _settingsWindow.Activity;
                //Показываем окно
                _settingsWindow.SetActivity(this);
                _settingsWindow.Show(ft, "dialog");
            };
        }
    }

    public class SamplePagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;
        public SamplePagerAdapter(MainActivity activity, Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {
            //Добавляем фрагменты массив
            mFragmentHolder = new List<Android.Support.V4.App.Fragment>
            {
                activity._fragment1,
                activity._fragment2,
                activity._fragment3
            };
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

    //Сортировка по типу задачи
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

            var house1 = view.FindViewById<LinearLayout>(Resource.Id.house1);
            var house2 = view.FindViewById<LinearLayout>(Resource.Id.house2);
            var house3 = view.FindViewById<LinearLayout>(Resource.Id.house3);
            var house4 = view.FindViewById<LinearLayout>(Resource.Id.house4);
            //Сортировка по категории спорт
            house1.Click += delegate
            {
                Console.WriteLine("H1 clicked");
                _activity.mViewPager.SetCurrentItem(1, true);
                _activity._fragment2.SortCategory("Спорт");
            };
            //Прочее
            house2.Click += delegate
            {
                Console.WriteLine("H2 clicked");
                _activity.mViewPager.SetCurrentItem(1, true);
                _activity._fragment2.SortCategory("Прочее");
            };
            //Образование
            house3.Click += delegate
            {
                Console.WriteLine("H3 clicked");
                _activity.mViewPager.SetCurrentItem(1, true);
                _activity._fragment2.SortCategory("Образоване");
            };
            //Финансы
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
            return "Город";
        }
    }

    //Основной экран
    //Создание, удалени и отображение заметок
    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        ListView lstData;
        public List<Resources.Model.Task> lstSource = new List<Resources.Model.Task>();
        private EditText mTxt;
        public MainActivity _activity;

        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            DataBase.db = DataBase.getDataBase();
            DataBase.db.createDataBase();

            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            long elementId;
            var view = inflater.Inflate(Resource.Layout.SecondFragmentLayout, container, false);
            lstData = view.FindViewById<ListView>(Resource.Id.listView);
            //Задаем текст для пустого списка
            var emptyView = inflater.Inflate(Resource.Layout.emptyList, container, false);
            lstData.EmptyView = emptyView.FindViewById<TextView>(Resource.Id.empty);

            var btnAdd = view.FindViewById<FloatingActionButton>(Resource.Id.btnAdd);
            var btnRefresh = view.FindViewById<Button>(Resource.Id.btnRefresh);
            var btnDeleteAll = view.FindViewById<Button>(Resource.Id.btnDeleteAll);
            //Загружаем данные из бд и обновляем список задач
            LoadData();
            //Добавить новую задачу
            btnAdd.Click += delegate
            {
                //Создаем пустую запись в бд


                Resources.Model.Task task = new Resources.Model.Task()
                {

                    Name = "",
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    Time = new DateTime(1, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                    Description = "",
                    Category = "Спорт",
                    Priority = "0",
                    Done = false
                };
                var t = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    DataBase.db.insertIntoTableTask(task);
                });
                t.Wait();
                t.Dispose();
                //Создаем окно для добавления новой задачи
                Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                Android.Support.V4.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                //Передаем id новой заметки для корректной записи в бд
                Bundle frag_bundle = new Bundle();
                frag_bundle.PutLong("Id", task.Id);

                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);

                _activity._taskWindow = DialogWindow.NewInstance(frag_bundle);
                var act = _activity._taskWindow.Activity;
                //Показываем окно
                _activity._taskWindow.SetActivity(_activity);
                _activity._taskWindow.Show(ft, "dialog");

            };

            btnRefresh.Click += delegate
            {
                LoadData();
            };

            // Эта кнопка удаляет все выполненные задачи
            btnDeleteAll.Click += delegate
            {
                Android.App.AlertDialog.Builder delDoneDialog = new Android.App.AlertDialog.Builder(_activity);
                delDoneDialog.SetTitle("Удалить все выполненные задачи");
                delDoneDialog.SetMessage("Вы уверены?");
                delDoneDialog.SetPositiveButton("Да", (senderAlert, args) =>
                {
                    DataBase.db.delAllDoneTask();
                    _activity._fragment2.LoadData();
                });
                delDoneDialog.SetNegativeButton("Нет", (senderAlert, args) =>
                {
                    return;
                });
                Dialog dialog = delDoneDialog.Create();
                dialog.Show();

                //delDoneWindow.Show;


            };
            //Просмотр/ редактирование существующей задачи
            lstData.ItemClick += (s, e) =>
            {

                for (int i = 0; i < lstData.Count; i++)
                {
                    if (e.Position == i)
                    {
                        //Получаем id выбранного в списке элемента
                        var t = System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            elementId = DataBase.db.selectQuery(lstData.Adapter.GetItemId(e.Position));

                            return elementId;
                        });
                        t.Wait();

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
                        _activity._taskWindow = DialogWindow.NewInstance(frag_bundle);
                        _activity._taskWindow.SetActivity(_activity);
                        _activity._taskWindow.Show(ft, "dialog");

                    }
                }

                //Заполняем поля данными
                var txtName = e.View.FindViewById<TextView>(Resource.Id.textView1);
                var checkedBox = e.View.FindViewById<CheckBox>(Resource.Id.chkBox);
                //var txtDone = e.View.FindViewById<TextView>(Resource.Id.textView5);
                checkedBox.SetOnCheckedChangeListener(null);

            };
            //Удержание на элементе
            lstData.ItemLongClick += (s, e) =>
            {
                if (DataBase.db.getSettings()[0].fastDel == true || DataBase.db.getSettings()[0].fastDel == null)
                {
                    Android.App.AlertDialog.Builder delDialog = new Android.App.AlertDialog.Builder(_activity);
                    delDialog.SetTitle("Удалить задачу");
                    delDialog.SetMessage("Вы уверены?");
                    delDialog.SetPositiveButton("Да", (senderAlert, args) =>
                    {

                        for (int i = 0; i < lstData.Count; i++)
                        {
                            if (e.Position == i)
                            {
                                var selected_Element = DataBase.db.get_Element(lstData.Adapter.GetItemId(e.Position))[0];
                                DataBase.db.delTask(selected_Element.Id);
                                _activity._fragment2.LoadData();
                                Toast.MakeText(_activity, "Задача удалена", ToastLength.Long).Show();
                            }
                        }
                    });
                    delDialog.SetNegativeButton("Нет", (senderAlert, args) =>
                    {
                        return;
                    });
                    Dialog dialog = delDialog.Create();
                    dialog.Show();
                }
            };
            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Задачи";
        }
        //Устанавливаем напоминание о задаче
        public void StartAlarm(int Year, int Month, int Day, int Hour, int Minute, long id)
        {
            var Current = Java.Lang.JavaSystem.CurrentTimeMillis();

            AlarmManager manager = (AlarmManager)_activity.GetSystemService(Context.AlarmService);
            Intent myIntent;
            PendingIntent pendingIntent;
            myIntent = new Intent(_activity, typeof(AlarmNotificationReceiver));
            myIntent.PutExtra("id", id);

            pendingIntent = PendingIntent.GetBroadcast(_activity, 0, myIntent, 0);

            var unixTime = new DateTime(Year, Month, Day, Hour, Minute, 1);
            var CurrentTime = DateTime.Now;
            var AlarmTime = unixTime - CurrentTime;
            long ATMillis = (long)AlarmTime.TotalMilliseconds;
            manager.Set(AlarmType.RtcWakeup, JavaSystem.CurrentTimeMillis() + ATMillis, pendingIntent);

        }

        //Сортировка по id
        public void LoadData()
        {
            lstSource = DataBase.db.selectTableTask();
            _activity.adapter.SetFrActivity(this);
            _activity.adapter.SetList(lstSource);
            _activity.adapter.SetActivity(_activity);
            lstData.Adapter = _activity.adapter;
        }

        //Сортировка по дате
        public void SortByDate(DateTime date)
        {
            lstSource = DataBase.db.showDate(date);
            _activity.adapter.SetFrActivity(this);
            _activity.adapter.SetList(lstSource);
            this.lstData.Adapter = _activity.adapter;
        }

        //Сортировать по категории
        public void SortCategory(string _category)
        {
            lstSource = DataBase.db.showCategory(_category);
            _activity.adapter.SetFrActivity(this);
            _activity.adapter.SetList(lstSource);
            this.lstData.Adapter = _activity.adapter;
        }
    }

    //Достижения пользователя
    public class Fragment3 : Android.Support.V4.App.Fragment
    {
        ListView lstDataAch;
        public List<Achievement> lstAch = new List<Achievement>();
        //DataBase db;

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
        bool created = false;

        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            DataBase.db = DataBase.getDataBase();
            DataBase.db.createDataBaseAchivment();
            DataBase.db.createDataBaseExp();
            var ach = DataBase.db.getExp();


            var view = inflater.Inflate(Resource.Layout.ThirdFragmentLayout, container, false);


            lstDataAch = view.FindViewById<ListView>(Resource.Id.listViewAch);
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

                progressMain.Progress = (ach[0].MainExp % 100);
                progressSport.Progress = (ach[0].SportExp % 100);
                progressEducation.Progress = (ach[0].EducationExp % 100);
                progressFinance.Progress = (ach[0].FinansiExp % 100);
                progressOther.Progress = (ach[0].OtherExp % 100);

            }

            LoadAchevementsData();
            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Достижения";
        }

        // Заполняем XP бары запросом в БД
        public void LoadExpData()
        {
            var ach = DataBase.db.getExp();
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
            Console.WriteLine("Ach count = " + ach.Count);
        }

        public void LoadAchevementsData()
        {
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db"));
            var ach = connection.Query<Exp>("SELECT * FROM Exp");

            // ветка если БД еще не создана
            if (ach.Count == 0)
            {
                Exp achievement = new Exp()
                {
                    Name = "Достижения",
                    EducationExp = 0,
                    MainExp = 0,
                    FinansiExp = 0,
                    OtherExp = 0,


                };
                connection.Insert(achievement);

                ach = connection.Query<Exp>("SELECT * FROM Exp");
            }

            int numberOfStars = 0;
            if (ach[0].SportExp > 10) { numberOfStars = 1; }
            else if (ach[0].SportExp > 20) { numberOfStars = 2; }
            else if (ach[0].SportExp > 30) { numberOfStars = 3; }
            if (created == false)
            {
                Achievement sportAch = new Achievement()
                {
                    Name = "Спорт",
                    Description = "Выполните 10 заданий в категории спорт",
                    Type = "sport",
                    Stars = numberOfStars
                };


                lstAch.Add(sportAch);
            }


            numberOfStars = 0;
            if (ach[0].FinansiExp > 1) { numberOfStars = 1; }
            else if (ach[0].FinansiExp > 2) { numberOfStars = 2; }
            else if (ach[0].FinansiExp > 3) { numberOfStars = 3; }
            if (created == false)
            {
                Achievement financeAch = new Achievement()
                {
                    Name = "Финансы",
                    Description = "Выполните 10 заданий в категории финансы",
                    Type = "financi",
                    Stars = numberOfStars
                };


                lstAch.Add(financeAch);

            }

            numberOfStars = 0;
            if (ach[0].OtherExp > 10) { numberOfStars = 1; }
            else if (ach[0].OtherExp > 20) { numberOfStars = 2; }
            else if (ach[0].OtherExp > 30) { numberOfStars = 3; }
            if (created == false)
            {
                Achievement otherAch = new Achievement()
                {
                    Name = "Прочее",
                    Description = "Выполните 10 заданий в категории прочее",
                    Type = "other",
                    Stars = numberOfStars
                };


                lstAch.Add(otherAch);

            }


            numberOfStars = 0;
            if (ach[0].EducationExp > 10) { numberOfStars = 1; }
            else if (ach[0].EducationExp > 20) { numberOfStars = 2; }
            else if (ach[0].EducationExp > 30) { numberOfStars = 3; }
            if (created == false)
            {
                Achievement educationAch = new Achievement()
                {
                    Name = "Образование",
                    Description = "Выполните 10 заданий в категории образование",
                    Type = "edication",
                    Stars = numberOfStars
                };


                lstAch.Add(educationAch);

            }

            _activity.achievments_adapter.SetFrActivity(this);
            _activity.achievments_adapter.SetList(lstAch);
            _activity.achievments_adapter.SetActivity(_activity);
            lstDataAch.Adapter = _activity.achievments_adapter;
            created = true;
        }
    }
}