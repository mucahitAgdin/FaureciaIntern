using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using TicketApp.Models;

namespace TicketApp.Helpers
{
    public static class DatabaseHelper
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tickets.db");
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        // Database ve tabloyu oluştur
        public static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string tableQuery = @"CREATE TABLE IF NOT EXISTS Tickets (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Area TEXT,
                                        Issue TEXT,
                                        Description TEXT,
                                        CreatedAt TEXT
                                      );";
                using (var cmd = new SQLiteCommand(tableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Yeni ticket ekle
        public static void InsertTicket(Ticket ticket)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Tickets (Area, Issue, Description, CreatedAt) VALUES (@Area, @Issue, @Desc, @Date)";
                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Area", ticket.Area);
                    cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                    cmd.Parameters.AddWithValue("@Desc", ticket.Description);
                    cmd.Parameters.AddWithValue("@Date", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<Ticket> GetAllTickets()
        {
            var tickets = new List<Ticket>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Area, Issue, Description, CreatedAt FROM Tickets ORDER BY CreatedAt DESC";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tickets.Add(new Ticket
                        {
                            Area = reader["Area"].ToString(),
                            Issue = reader["Issue"].ToString(),
                            Description = reader["Description"].ToString(),
                            CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString())
                        });
                    }
                }
            }

            return tickets;
        }
        public static void DeleteTicket(DateTime createdAt)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM Tickets WHERE CreatedAt = @Date";

                using (var cmd = new SQLiteCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", createdAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
