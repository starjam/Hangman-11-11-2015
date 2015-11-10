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

namespace hangmanGame
{
    public class ToDo
    {   //ToDo.cs choudl be call word...
        // [PrimaryKey,AutoIncrement]
        public int WordID { get; set; }
        public string WORD { get; set; }

        public ToDo()
        {
        }

    }
}