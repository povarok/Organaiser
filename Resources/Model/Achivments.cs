﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidSQLite.Resources.Model
{
    public class Achivments
    {
        public int MainExp { get; set; }
        public int SportExp { get; set; }
        public int OtherExp { get; set; }
        public int EducationExp { get; set; }
        public int FinansiExp { get; set; }
    }
}