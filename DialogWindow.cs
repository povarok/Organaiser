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

namespace AndroidSQLite
{
    public class DialogFragment1 : DialogFragment
    {
        ListView lstData;
        List<Person> lstSource = new List<Person>();
        DataBase db;
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
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            View view = inflater.Inflate(Resource.Layout.fragment_layout, container, false);
            Button buttonSave = view.FindViewById<Button>(Resource.Id.btnSaveFr);
            Button buttonCl = view.FindViewById<Button>(Resource.Id.CloseButton);
            TextView txtView = view.FindViewById<TextView>(Resource.Id.textView2);
            EditText editName = view.FindViewById<EditText>(Resource.Id.edtNameFr);
            EditText editEmail = view.FindViewById<EditText>(Resource.Id.edtEmailFr);
            EditText editAge = view.FindViewById<EditText>(Resource.Id.edtAgeFr);
            //var getID = savedInstanceState.GetLong("Id",1288);
            var getID = Arguments.GetLong("Id", 1243);
            //Bundle new_bundle = this.Arguments.GetBundle("ID");

            var selected_Element = db.get_Element(getID)[0];
            editName.Text = selected_Element.Name;
            editAge.Text = selected_Element.Age.ToString();
            editEmail.Text = selected_Element.Email;
           
            //var getID = this.Arguments.GetLong("Id", 1488);
            txtView.Text =  selected_Element.Age.ToString();
            buttonSave.Click += delegate
            {
                selected_Element.Id = int.Parse(getID.ToString());//int.Parse(editName.Tag.ToString());
                selected_Element.Name = "Nsmr";//editName.Text;
                selected_Element.Age = 15;//int.Parse(editAge.Text);
                selected_Element.Email = "dfefwe";//editEmail.Text;
               //Person person = new Person()
               // {
               //     Id = int.Parse(editName.Tag.ToString()),
               //     Name = editName.Text,
               //     Age = int.Parse(editAge.Text),
               //     Email = editEmail.Text
               // };
                db.updateTablePerson(selected_Element);
                //Закрыть фрагмент
                Dismiss();
               // LoadData();
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