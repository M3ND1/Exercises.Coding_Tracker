using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Coding_Tracker
{
    public class UserInput
    {
        public static string connection_string = Program.connection_string;
        public static void MainMenu()
        {
            bool flag = false;
            while (flag == false)
            {
                Console.Clear();
                Console.WriteLine("What do you wanna do? Choose from 0 to 5");
                Console.WriteLine("0 - App Exit");
                Console.WriteLine("1 - See all users and habits");
                Console.WriteLine("2 - Insert your coding time"); // end - start = date
                Console.WriteLine("3 - Delete your coding time"); // end - start = date
                Console.WriteLine("4 - Edit your coding time"); // end - start = date

                string user_choice = Console.ReadLine();
                switch (user_choice)
                {
                    case "0":
                        flag = true;
                        break;
                    case "1":
                        Get();
                        break;
                    case "2":
                        Insert(); //TODO check if day is the same, 23 start and 02 end? 02 - 23 ??
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Edit();
                        break;
                    default:
                        Console.WriteLine("Write proper from 0-5");
                        break;
                }
            }
        }
        private static void Insert()
        {
            string startTime = GetDateTime();
            string endTime = GetDateTime();
            //check if days are the same
            float duration = GetDurationTime(startTime, endTime);
            //TimeSpan difference = (endTime - startTime).Days;
            using (var conn = new SQLiteConnection(connection_string))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"INSERT INTO CodingSession(StartTime, EndTime, Duration) VALUES ('{startTime}', '{endTime}', {duration})";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Console.Clear();
        }
        private static void Get()
        {
            Console.Clear();
            using(var conn = new SQLiteConnection(connection_string))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM CodingSession";

                List<CodingSession> coding_sesions = new();

                SQLiteDataReader reader = cmd.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        coding_sesions.Add(
                            new CodingSession
                            {
                                Id = reader.GetInt32(0),
                                StartTime = DateTime.Parse(reader.GetString(1)),
                                EndTime = DateTime.Parse(reader.GetString(2)),
                                Duration = reader.GetFloat(3)
                            });
                    }
                } else
                {
                    Console.WriteLine("Table empty");
                }
                conn.Close();
                //display here using library
                foreach(var c in coding_sesions)
                {
                    Console.WriteLine($"Person: {c.Id}, started at {c.StartTime}, ended at {c.EndTime}. " +
                        $"Total time learnt: {c.Duration} hours."); //temporary console wr
                }
                System.Threading.Thread.Sleep(2000);
            }
        }
        private static void Delete()
        {
            Console.Clear();
            Console.WriteLine("Provide what Id you want to delete. \t To quit from this section type 0\n\n");
            //System.Threading.Thread.Sleep(4000);
            Console.Clear();
            Console.WriteLine("Choose by Id what to delete \n");
            Get();
            Console.Write("Your Id to delete: ");
            int sol = Convert.ToInt32(Console.ReadLine());
            if (sol == 0) MainMenu();
            using(var conn = new SQLiteConnection(connection_string))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"DELETE FROM CodingSession WHERE Id = '{sol}'";
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    Console.WriteLine("Database Empty");
                }
                conn.Close();
            }
            Console.WriteLine($"Person {sol} learn time was deleted properly");
            System.Threading.Thread.Sleep(1000);
        }
        private static void Edit()
        {
            Console.Clear();
            Console.WriteLine("Choose what id do you want do update");
            Get();
            int id_get = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            string startTime = GetDateTime();
            string endTime = GetDateTime();
            float duration = GetDurationTime(startTime, endTime);
            using (var conn = new SQLiteConnection(connection_string)) 
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"UPDATE CodingSession SET startTime = '{startTime}', " +
                    $"endTime = '{endTime}', Duration = {duration} WHERE Id = {id_get}";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        static string GetDateTime()
        {
            Console.WriteLine("Enter Date ex. 01/01/01 00:00");
            string date = Console.ReadLine();
            DateTime dateTime;
            while(!DateTime.TryParseExact(date, "dd/MM/yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None ,out dateTime))
            {
                Console.WriteLine("Enter proper date ex. 01/01/01 00:00");
                date = Console.ReadLine();
                Console.Clear();
            }
            Console.WriteLine("WORKING");
            return date;
        }
        static float GetDurationTime(string sT, string eT)
        {
            DateTime start = DateTime.ParseExact(sT, "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(eT, "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);
            TimeSpan interval = end.Subtract(start);
            float duration = (float)interval.TotalHours;
            return duration;
        }
    }
}
