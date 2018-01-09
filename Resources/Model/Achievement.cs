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
using SQLite;

namespace AndroidSQLite.Resources.Model
{
    public class Achievement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Stars { get; set; }
        
    }
}