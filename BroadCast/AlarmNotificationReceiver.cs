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
        public MainActivity _activity;
        public void SetActivity(MainActivity activity)
        {
            _activity = activity;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);
            DataBase.db = DataBase.getDataBase();
            
            var task = DataBase.db.get_Last();
            
            builder.SetAutoCancel(true)
                .SetDefaults((int)NotificationDefaults.All)
                
                .SetContentTitle(task.Name)
                .SetContentText(task.Description)
                .SetContentInfo("Info");
            //Console.WriteLine("task[0].Category = " + task.Category);
            if (task.Category == "Образоване")
            {
                builder.SetSmallIcon(Resource.Drawable.education);
                //Console.WriteLine("Проверяем оповещения" + task.Category + " = Образование");
            }
            if (task.Category == "Финансы")
            {
                builder.SetSmallIcon(Resource.Drawable.finance);
                //Console.WriteLine("Проверяем оповещения" + task.Category + " = Финансы");
            }
            if (task.Category == "Прочее")
            {
                builder.SetSmallIcon(Resource.Drawable.other);
                //Console.WriteLine("Проверяем оповещения" + task.Category + " = Прочее");
            }
            if (task.Category == "Спорт")
            {
                builder.SetSmallIcon(Resource.Drawable.sport);
                //Console.WriteLine("Проверяем оповещения" + task.Category + " = Спорт");
            }


            NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            manager.Notify(1, builder.Build());
        }
        
    }
}