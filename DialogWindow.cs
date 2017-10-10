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

namespace AndroidSQLite
{
    public class DialogFragment1 : DialogFragment
    {
        public static DialogFragment1 NewInstance(Bundle bundle)
        {
            DialogFragment1 fragment = new DialogFragment1();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_layout, container, false);
            Button buttonCl = view.FindViewById<Button>(Resource.Id.CloseButton);
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
    }
}