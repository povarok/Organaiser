using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidSQLite.Resources.DataHelper;
using AndroidSQLite.Resources.Model;
using AndroidSQLite.Resources;
using static Android.App.DatePickerDialog;
using Java.Util;
using static Android.App.TimePickerDialog;

namespace AndroidSQLite
{
    public class DialogWindow : Android.Support.V4.App.DialogFragment, IDialogInterfaceOnDismissListener, IOnDateSetListener, IOnTimeSetListener
    {

        public long id;

        public void setId(long id)
        {
            this.id = id;
        }

        public long getId()
        {
            return this.id;
        }
        
        List<Task> lstSource = new List<Task>();
        
        DateTime selectedDateDate;
        DateTime selectedDateTime;
        Calendar currentDate;
        EditText setTime;
        EditText setDateTime;
        // не работает 
        int globalHour = 21;
        int globalMin = 44;
        public MainActivity _activity;

        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }
       

        public override void OnDismiss(IDialogInterface dialog)
        {
            //SetActivity(ma);
            //_activity.OnDismiss();
            Console.WriteLine("ONDISMIS from Dialog Window");
            //SetActivity(_activity);
            
        }

      
        public static DialogWindow NewInstance(Bundle bundle)
        {
            DialogWindow fragment = new DialogWindow();
            fragment.Arguments = bundle;

            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            DataBase.db = DataBase.getDataBase();

            DataBase.db.createDataBase();
            lstSource = new List<Task>();
            currentDate = Calendar.Instance;

            //SetActivity()
            //ma.lstData = View.FindViewById<ListView>(Resource.Id.listView);


            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);



            View view = inflater.Inflate(Resource.Layout.fragment_layout, container, false);

            var lstData = view.FindViewById<ListView>(Resource.Id.listView);

            Button buttonSave = view.FindViewById<Button>(Resource.Id.btnSaveFr);
            Button buttonCl = view.FindViewById<Button>(Resource.Id.CloseButton);
            Button buttonDel = view.FindViewById<Button>(Resource.Id.DeleteButton);

            setTime = view.FindViewById<EditText>(Resource.Id.stTime);
            setDateTime = view.FindViewById<EditText>(Resource.Id.stDate);

            EditText editName = view.FindViewById<EditText>(Resource.Id.edtNameFr);
            EditText editDescription = view.FindViewById<EditText>(Resource.Id.edtDescription);

            Spinner setCategory = view.FindViewById<Spinner>(Resource.Id.stCategory);
            Spinner setPriority = view.FindViewById<Spinner>(Resource.Id.stPriority);
            //var getID = savedInstanceState.GetLong("Id",1288);
            var getID = Arguments.GetLong("Id", 0);
            setId(getID);
            //Console.Write("asd");
            //Bundle new_bundle = this.Arguments.GetBundle("ID");

            //var selected_Element = new AndroidSQLite.Resources.Model.Person();

            //if (getID != ((long)0))
            //{
            
            var selected_Element = DataBase.db.get_Element(getID)[0];


            Console.WriteLine("Выбран " + selected_Element.Id + " ый эл-т");
            Console.WriteLine("дата выбранного элемента" + selected_Element.Date);
            Console.WriteLine("время выбранного элемента" + selected_Element.Time);
            //Console.WriteLine("Name - " + selected_Element.Name);
            //Console.WriteLine("Date - " + selected_Element.Date.ToLongDateString());
            //Console.WriteLine("Time - " + selected_Element.Time.ToLongTimeString());


            editName.Text = selected_Element.Name;
            editDescription.Text = selected_Element.Description;
            
            setDateTime.Text = selected_Element.Date.Day.ToString() + "/" + selected_Element.Date.Month.ToString() + "/" + selected_Element.Date.Year.ToString();
            selectedDateDate = selected_Element.Date;
            setTime.Text = selected_Element.Time.Hour.ToString() + " : " + selected_Element.Time.Minute.ToString();
            selectedDateTime = selected_Element.Time;
            if (selected_Element.Category == "Спорт")
            {
                setCategory.SetSelection(0);
            }
            if (selected_Element.Category == "Образоване")
            {
                setCategory.SetSelection(1);
            }
            if (selected_Element.Category == "Финансы")
            {
                setCategory.SetSelection(2);
            }
            if (selected_Element.Category == "Прочее")
            {
                setCategory.SetSelection(3);
            }

            if (selected_Element.Priority == "!")
            {
                setPriority.SetSelection(0);
            }
            if (selected_Element.Priority == "!!")
            {
                setPriority.SetSelection(1);
            }
            if (selected_Element.Priority == "!!!")
            {
                setPriority.SetSelection(2);
            }

            //Обработка выбора элемента из выпадающего списка
            setCategory.ItemSelected += (s, e) =>
            {
               
            };

            setPriority.ItemSelected += (s, e) =>
            {
                
            };

            setTime.Click += delegate
            {
                int Hour = currentDate.Get(CalendarField.Hour);
                int Min = currentDate.Get(CalendarField.Minute);
                TimePickerDialog timeDialog = new TimePickerDialog(this.Context, Android.App.AlertDialog.ThemeDeviceDefaultLight, this, Hour, Min, true);
                
                timeDialog.Show();
            };

            setDateTime.Click += delegate
            {
                int Year = currentDate.Get(CalendarField.Year);
                int Month = currentDate.Get(CalendarField.Month);
                int Day = currentDate.Get(CalendarField.DayOfMonth);
                
                DatePickerDialog dateDialog = new DatePickerDialog(this.Context, Android.App.AlertDialog.ThemeDeviceDefaultLight, this, Year, Month, Day);
                dateDialog.Show();
            };

            buttonDel.Click += delegate {
                Task task = new Task()
                {
                    Id = selected_Element.Id, 
                    Category = selected_Element.Category,
                    Date = selected_Element.Date, 
                    Description = selected_Element.Description,
                    Done = selected_Element.Done,
                    Name = selected_Element.Name,
                    Priority = selected_Element.Priority  
               };
                DataBase.db.deleteTableTask(task);
                _activity._fragment2.LoadData();
                Dismiss();


            };
            buttonSave.Click += delegate
            {
                selected_Element.Id = int.Parse(getID.ToString());

                if (editName.Text != null)
                { selected_Element.Name = editName.Text; }
                else
                { selected_Element.Name = "Null"; }

                //--------------------------------------------------------------------------------

                if (editDescription.Text != null)
                { selected_Element.Description = editDescription.Text; }
                else
                { selected_Element.Description = "Null"; }

                selected_Element.Category = setCategory.SelectedItem.ToString();
                selected_Element.Priority = setPriority.SelectedItem.ToString();

                //--------------------------------------------------------------------------------

                if (selectedDateDate != null)
                {
                    selected_Element.Date = selectedDateDate;
                    Console.WriteLine("selected date: " + selectedDateDate.ToLongDateString());
                }
                //else
                //{
                //   // selected_Element.Date = DateTime.Now;
                //    //Было
                //    selected_Element.Date = new DateTime(1, 1, 2018, 21, 21 , 59);
                    
                //    Console.WriteLine("selected date = NULL " + selectedDateDate.ToLongDateString());
                //}

                //--------------------------------------------------------------------------------

                if (selectedDateTime != null)
                {
                    selected_Element.Time = selectedDateTime;
                    Console.WriteLine("selected time: " + selectedDateDate.ToLongTimeString());
                }
                //else
                //{
                //    // selected_Element.Date = DateTime.Now;
                //    //Было
                //    selected_Element.Time = new DateTime(1, 1, 2018, 21, 21, 59);

                //    Console.WriteLine("selected date = NULL " + selectedDateDate.ToLongTimeString());
                //}


                

                if (selected_Element.Name == "Null" || selected_Element.Name == "")
                {
                    Toast.MakeText(Activity, "Название заметки не может быть пустым!", ToastLength.Short).Show();
                }
                else
                {
                    DataBase.db.updateTableTask(selected_Element);
                    _activity._fragment2.LoadData();
                    _activity._fragment2.StartAlarm(selectedDateDate.Year, selectedDateDate.Month, selectedDateDate.Day,
                        selectedDateTime.Hour, selectedDateTime.Minute, getID);
                    Dismiss();

                }
                //Fragment2 fragment2 = new Fragment2();
                //fragment2.LoadData();
                //ma.LoadData(lstData);

                //Console.WriteLine("Сохраняю " + selected_Element.Id + " ый эл-т");
                //Console.WriteLine("Name - " + selected_Element.Name);
                //Console.WriteLine("Date - " + selected_Element.Date.ToLongDateString());
                //Console.WriteLine("Time - " + selected_Element.Time.ToLongTimeString());

                

            };

            buttonCl.Click += delegate
            {
                //Удаляем пустую заметку
                if (selected_Element.Name == "" || selected_Element.Name == "Null")
                {
                    Toast.MakeText(Activity, "Отмена создания", ToastLength.Short).Show();
                    DataBase.db.deleteTableTask(selected_Element);
                    Dismiss();

                }
                //Обработать диалогом подтверждение закрытия без сохранения!!!
                else
                {

                    Dismiss();
                    Toast.MakeText(Activity, "Dialog fragment dismissed!", ToastLength.Short).Show();
                }
            };
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return view;
        }
        //Так происходит привязка анимации к диалогу
        //Файлы с анимацией лежат в папке Resources/anim
        //Файл стилей объединяет все анимации для одного объекта
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            //Обращение к файлу стилей по "name"
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
        //Отсюда берем значения
        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            Toast.MakeText(this.Context, $"{dayOfMonth}-{month + 1}-{year}", ToastLength.Long).Show();
            selectedDateDate = new DateTime(year, month+1, dayOfMonth ,0, 0, 0);
            setDateTime.Text = selectedDateDate.Day.ToString()  + "/" + selectedDateDate.Month.ToString() + "/" + selectedDateDate.Date.Year.ToString();
            //setTime.Text = globalHour.ToString() + " : " + globalMin.ToString();

        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            globalHour = hourOfDay;
            globalMin = minute;
            //Console.WriteLine("globalHour " + globalHour + "globalMin " + globalMin);
            selectedDateTime = new DateTime(1,1,1, globalHour, globalMin, 0);
            setTime.Text = globalHour.ToString() + " : " + globalMin.ToString();
        }



        //Найти как обратится к майн активити
        //Или починить
        //Найти метод onResume или че нибудь такое... И в него запилить Loaddata 
        //private void LoadData()
        //{
        //    lstSource = db.selectTableTask();
        //    var adapter = new ListViewAdapter(, lstSource);
        //    lstData.Adapter = adapter;
        //}
    }
}