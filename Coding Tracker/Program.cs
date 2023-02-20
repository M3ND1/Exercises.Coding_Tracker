using Coding_Tracker;
using System.Data.SQLite;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

class Program
{
    public static string connection_string = @"Data Source=CodingTracker.db;Version=3;";
    static void Main(string[] args)
    {
        DatabaseManager databaseManager = new();
        if (!databaseManager.OpenConnection())
        {
            Console.WriteLine("Could not connect to DB");
        }
        else
        {
            UserInput.MainMenu();
        }
    } 
}
