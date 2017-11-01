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

namespace AndroidSQLite
{
    public class DialogFragment1 : Android.Support.V4.App.DialogFragment, IDialogInterfaceOnDismissListener, IOnDateSetListener
    {


        MainActivity ma = new MainActivity();

        List<Person> lstSource = new List<Person>();
        DataBase db;
        DateTime selectedDate;
        Calendar currentDate;



        // не работает 

        private MainActivity _activity;
        //public void SetActivity(MainActivity activity)
        //{
        //    //_activity = activity;
        //}

        


        //Fragment2 ma = new Fragment2();


        //List<Person> lstSource = new List<Person>();
        //DataBase db;

        public override void OnDismiss(IDialogInterface dialog)
        {
            //SetActivity(ma);
            //_activity.OnDismiss();
            Console.WriteLine("ONDISMIS from Dialog Window");
        }


      

        //private sealed class OnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
        //{
        //    private readonly Action action;

        //    public OnDismissListener(Action action)
        //    {
        //        this.action = action;
        //    }

        //    public void OnDismiss(IDialogInterface dialog)
        //    {
        //        this.action();
        //    }
        //}



        public static DialogFragment1 NewInstance(Bundle bundle)
        {
            DialogFragment1 fragment = new DialogFragment1();
            fragment.Arguments = bundle;

            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            db = new DataBase();

            db.createDataBase();
            lstSource = new List<Person>();
            currentDate = Calendar.Instance;

            //ma.lstData = View.FindViewById<ListView>(Resource.Id.listView);


            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);



            View view = inflater.Inflate(Resource.Layout.fragment_layout, container, false);

            var lstData = view.FindViewById<ListView>(Resource.Id.listView);

            Button buttonSave = view.FindViewById<Button>(Resource.Id.btnSaveFr);
            Button buttonCl = view.FindViewById<Button>(Resource.Id.CloseButton);
            Button buttonDel = view.FindViewById<Button>(Resource.Id.DeleteButton);

            EditText setDateTime = view.FindViewById<EditText>(Resource.Id.stDate);
            EditText editName = view.FindViewById<EditText>(Resource.Id.edtNameFr);
            EditText editDescription = view.FindViewById<EditText>(Resource.Id.edtDescription);

            Spinner setCategory = view.FindViewById<Spinner>(Resource.Id.stCategory);
            Spinner setPriority = view.FindViewById<Spinner>(Resource.Id.stPriority);
            //var getID = savedInstanceState.GetLong("Id",1288);
            var getID = Arguments.GetLong("Id", 0);
            //Console.Write("asd");
            //Bundle new_bundle = this.Arguments.GetBundle("ID");

            //var selected_Element = new AndroidSQLite.Resources.Model.Person();

            //if (getID != ((long)0))
            //{
            //Тут наверное надо дописать для бд всю хуйню
            var selected_Element = db.get_Element(getID)[0];


            editName.Text = selected_Element.Name;
            editDescription.Text = selected_Element.Description;
            
            setDateTime.Text = selected_Element.Date.Day.ToString() + "/" + selected_Element.Date.Month.ToString() + "/" + selected_Element.Date.Year.ToString();

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






            //else
            //{

            //    //editName.Text = "";
            //    //editAge.Text = "";
            //    //editDescription.Text = "";


            //}


            //Обработка выбора элемента из выпадающего списка
            setCategory.ItemSelected += (s, e) =>
            {
                //Пока что вывод в консоль
                Console.WriteLine("Выбрана категория: " + e.Parent.GetItemAtPosition(e.Position).ToString());
            };

            setPriority.ItemSelected += (s, e) =>
            {
                //Пока что вывод в консоль
                Console.WriteLine("Выбран приоритет: " + e.Parent.GetItemAtPosition(e.Position).ToString());
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
                Person person = new Person()
                {
                    Id = selected_Element.Id, 
                    Category = selected_Element.Category,
                    Date = selected_Element.Date, 
                    Description = selected_Element.Description,
                    Done = selected_Element.Done,
                    Name = selected_Element.Name,
                    Priority = selected_Element.Priority  
               };
                db.deleteTablePerson(person);
                Dismiss();


            };
            buttonSave.Click += delegate
            {
                selected_Element.Id = int.Parse(getID.ToString());

                if (editName.Text != null)
                { selected_Element.Name = editName.Text; }
                else
                { selected_Element.Name = "Null"; }


                if (editDescription.Text != null)
                { selected_Element.Description = editDescription.Text; }
                else
                { selected_Element.Description = "Null"; }

                selected_Element.Category = setCategory.SelectedItem.ToString();
                selected_Element.Priority = setPriority.SelectedItem.ToString();


                if (selectedDate != null)
                {
                    selected_Element.Date = selectedDate;
                }
                else
                {
                    selected_Element.Date = new DateTime(1, 1, 2000);
                }

                db.updateTablePerson(selected_Element);
                //Закрыть фрагмент



                Dismiss();
                //Fragment2 fragment2 = new Fragment2();
                //fragment2.LoadData();
                //ma.LoadData(lstData);


            };

            buttonCl.Click += delegate
            {
                Dismiss();
                Toast.MakeText(Activity, "Dialog fragment dismissed!", ToastLength.Short).Show();
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
            
            //Типо собираем структуру для бд. Надо проверить
            selectedDate = new DateTime(year, month, dayOfMonth); 
        }



        //Найти как обратится к майн активити
        //Или починить
        //Найти метод onResume или че нибудь такое... И в него запилить Loaddata 
        //private void LoadData()
        //{
        //    lstSource = db.selectTablePerson();
        //    var adapter = new ListViewAdapter(, lstSource);
        //    lstData.Adapter = adapter;
        //}
    }
}