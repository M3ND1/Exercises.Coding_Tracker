using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Tracker
{
    internal class DatabaseManager
    {
        public bool OpenConnection()
        {
            string conn_string = Program.connection_string;
            using(var conn = new SQLiteConnection(conn_string))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS CodingSession (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration REAL
                )";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return true;
        }

    }
}
