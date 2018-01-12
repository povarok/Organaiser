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
using AndroidSQLite.Resources.DataHelper;
using Android.Support.V4.App;
using AndroidSQLite.Resources.Model;

namespace AndroidSQLite
{
    public class SettingsWindow: Android.App.DialogFragment
    {
        public MainActivity _activity;

        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }

        public static SettingsWindow NewInstance(Bundle bundle)
        {
            SettingsWindow fragment = new SettingsWindow();
            fragment.Arguments = bundle;

            return fragment;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            DataBase.db = DataBase.getDataBase();
            View view = inflater.Inflate(Resource.Layout.settings_layout, container, false);
            CheckBox fastDel = view.FindViewById<CheckBox>(Resource.Id.fastDelChBx);
            fastDel.Checked = DataBase.db.getSettings()[0].fastDel;

            fastDel.Click += delegate
            {
                DataBase.db.updateSettings(fastDel.Checked);
            };

            Button closeBtn = view.FindViewById<Button>(Resource.Id.closeBtn);
            closeBtn.Click += delegate
            {
                Dismiss();
            };
            
            return view;
        }

        public override void OnActivityCreated(Bundle bundle)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(bundle);
            //Обращение к файлу стилей по "name"
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }

    }
}