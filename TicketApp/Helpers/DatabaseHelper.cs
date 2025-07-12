using System;
using System.Data.SQLite;
using System.IO;
using TicketApp.Models;

namespace TicketApp.Helpers
{
    public static class DatabaseHelper
    {
        private static string dbFile = "tickets.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
                using (var connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
                {
                    connection.Open();
                    string sql = @"CREATE TABLE Tickets (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Area TEXT NOT NULL,
                                    Issue TEXT NOT NULL,
                                    Description TEXT,
                                    CreatedAt DATETIME NOT NULL
                                   )";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void InsertTicket(Ticket ticket)
        {
            using (var connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();
                string sql = "INSERT INTO Tickets (Area, Issue, Description, CreatedAt) VALUES (@area, @issue, @desc, @created)";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@area", ticket.Area);
                command.Parameters.AddWithValue("@issue", ticket.Issue);
                command.Parameters.AddWithValue("@desc", ticket.Description);
                command.Parameters.AddWithValue("@created", ticket.CreatedAt);
                command.ExecuteNonQuery();
            }
        }
    }
}
