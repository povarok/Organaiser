﻿using System;
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
    public class DialogFragment1 : Android.Support.V4.App.DialogFragment, IDialogInterfaceOnDismissListener
    {
        
        
        //Fragment2 ma = new Fragment2();
        List<Person> lstSource = new List<Person>();
        DataBase db;

        public override void OnDismiss(IDialogInterface dialog)
        {

            Console.WriteLine("ONDISMIS");
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
            lstSource =  new List<Person>();


            //ma.lstData = View.FindViewById<ListView>(Resource.Id.listView);


            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            


            View view = inflater.Inflate(Resource.Layout.fragment_layout, container, false);

            var lstData = view.FindViewById<ListView>(Resource.Id.listView);

            Button buttonSave = view.FindViewById<Button>(Resource.Id.btnSaveFr);
            Button buttonCl = view.FindViewById<Button>(Resource.Id.CloseButton);
            
            EditText editName = view.FindViewById<EditText>(Resource.Id.edtNameFr);
            EditText editEmail = view.FindViewById<EditText>(Resource.Id.edtEmailFr);
            EditText editAge = view.FindViewById<EditText>(Resource.Id.edtAgeFr);
            //var getID = savedInstanceState.GetLong("Id",1288);
            var getID = Arguments.GetLong("Id", 0);
            //Console.Write("asd");
            //Bundle new_bundle = this.Arguments.GetBundle("ID");

            //var selected_Element = new AndroidSQLite.Resources.Model.Person();

            //if (getID != ((long)0))
            //{
              var  selected_Element = db.get_Element(getID)[0];
                editName.Text = selected_Element.Name;
                editAge.Text = selected_Element.Age.ToString();
                editEmail.Text = selected_Element.Email;
            //}

            //else
            //{

            //    //editName.Text = "";
            //    //editAge.Text = "";
            //    //editEmail.Text = "";


            //}
            
            buttonSave.Click += delegate
            {
                selected_Element.Id = int.Parse(getID.ToString());

                if(editName.Text != null)
                    { selected_Element.Name = editName.Text;}
                else
                { selected_Element.Name = "Null"; }

                if (editAge.Text != null)
                { selected_Element.Age =int.Parse( editAge.Text); }
                else
                { selected_Element.Age = 0; }
                if (editEmail.Text != null)
                { selected_Element.Email = editEmail.Text; }
                else
                { selected_Element.Name = "Null"; }
                
              
                db.updateTablePerson(selected_Element);
                //Закрыть фрагмент
                
                
                
                Dismiss();
                Fragment2 fragment2 = new Fragment2();
                fragment2.LoadData();
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