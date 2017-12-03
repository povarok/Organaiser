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
            //DataBase.db = DataBase.getDataBase();


            //NullRefEx тут
            //SetActivity(main);

            //var id = main._dialogFragment1.getId();
             
            //Console.WriteLine("Полученный ID = " + id);

            builder.SetAutoCancel(true)
                .SetDefaults((int)NotificationDefaults.All)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentTitle("Alarm Actived!")
                .SetContentText("Название")
                .SetContentInfo("Info");


            NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            manager.Notify(1, builder.Build());
        }
    }
}