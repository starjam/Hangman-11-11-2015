using System;
using System.IO;
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
    public class DatabaseManager
    {
        static string dbName = "hangmanwords.sqlite";
        string dbPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);

        public DatabaseManager()
        {

        }
        public List<ToDo> ViewAll()
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    // cmd.CommandText = "Select * from words";
                    //SELECT ONE WORD RANDOMLY
                    cmd.CommandText = "SELECT * FROM words ORDER BY RANDOM() LIMIT 1";
                    var NoteList = cmd.ExecuteQuery<ToDo>();
                    return NoteList;
                }

            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e.Message);
                return null;

            }

        }
    }
}
