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
using Android.Support.V7.App;
using AndroidSQLite;
using AndroidSQLite.Resources.DataHelper;
using AndroidSQLite.Resources.Model;

namespace AndroidSQLite.BroadCast
{
    

    [BroadcastReceiver(Enabled = true)]
    public class AlarmNotificationReceiver : BroadcastReceiver
    {
        public MainActivity main;
        public void SetActivity(MainActivity activity)
        {
            main = activity;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);
            DataBase.db = DataBase.getDataBase();
            var person = DataBase.db.get_Element(intent.GetLongExtra("id", 0));

            //Console.WriteLine(intent.GetLongExtra("id", 228));

            //NullRefEx тут
            //SetActivity(main);

            //var id = main._dialogFragment1.getId();

            //Console.WriteLine("Полученный ID = " + id);
            string icon;
            if (person[0].Category == "Образоване")
            {
                builder.SetSmallIcon(Resource.Drawable.education);
            }
            if (person[0].Category == "Финансы")
            {
                builder.SetSmallIcon(Resource.Drawable.finance);
            }
            if (person[0].Category == "Прочее")
            {
                builder.SetSmallIcon(Resource.Drawable.other);
            }
            if (person[0].Category == "Спорт")
            {
                builder.SetSmallIcon(Resource.Drawable.sport);
            }


            builder.SetAutoCancel(true)
                .SetDefaults((int)NotificationDefaults.All)
                
                .SetContentTitle(person[0].Name)
                .SetContentText(person[0].Description)
                .SetContentInfo("Info");
            


            NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            manager.Notify(1, builder.Build());
        }
        
    }
}