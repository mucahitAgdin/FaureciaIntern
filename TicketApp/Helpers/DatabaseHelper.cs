// DatabaseHelper.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using TicketApp.Models;

namespace TicketApp.Helpers
{
    // Bu sınıf, SQLite veritabanı işlemlerini gerçekleştirir.
    public static class DatabaseHelper
    {
        private static readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tickets.db");
        private static readonly string connectionString = $"Data Source={dbPath};Version=3;";

        // Veritabanı oluşturulur ve tablo yoksa oluşturulur
        public static void InitializeDatabase()
        {
            try
            {
                if (!File.Exists(dbPath))
                    SQLiteConnection.CreateFile(dbPath);

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string createQuery = @"
                    CREATE TABLE IF NOT EXISTS Tickets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Area TEXT,
                        Issue TEXT,
                        Description TEXT,
                        CreatedAt DATETIME,
                        IsResolved INTEGER DEFAULT 0
                    );";

                    var cmd = new SQLiteCommand(createQuery, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        // Yeni ticket kaydı ekler
        public static void InsertTicket(Ticket ticket)
        {
            var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string insertQuery = @"
                INSERT INTO Tickets (Area, Issue, Description, CreatedAt)
                VALUES (@Area, @Issue, @Desc, @Date);";

            var cmd = new SQLiteCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@Area", ticket.Area);
            cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
            cmd.Parameters.AddWithValue("@Desc", ticket.Description);
            cmd.Parameters.AddWithValue("@Date", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

            cmd.ExecuteNonQuery();
        }

        // Tüm ticket'ları listeler
        public static List<Ticket> GetAllTickets()
        {
            var tickets = new List<Ticket>();

            var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string query = "SELECT Area, Issue, Description, CreatedAt, IsResolved FROM Tickets ORDER BY CreatedAt DESC";

            var cmd = new SQLiteCommand(query, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tickets.Add(new Ticket
                {
                    Area = reader["Area"].ToString(),
                    Issue = reader["Issue"].ToString(),
                    Description = reader["Description"].ToString(),
                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                    IsResolved = Convert.ToBoolean(reader["IsResolved"])
                });
            }

            return tickets;
        }

        // Belirli bir tarihli ticket’ı siler
        public static void DeleteTicket(DateTime createdAt)
        {
            var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string query = "DELETE FROM Tickets WHERE CreatedAt = @Date";
            var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Date", createdAt.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }

        // Çözülmüş ticket'ları getirir
        public static List<Ticket> GetResolvedTickets()
        {
            var tickets = new List<Ticket>();

            var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM Tickets WHERE IsResolved = 1";

             var cmd = new SQLiteCommand(query, conn);
             var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tickets.Add(new Ticket
                {
                    Area = reader["Area"].ToString(),
                    Issue = reader["Issue"].ToString(),
                    Description = reader["Description"].ToString(),
                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                    IsResolved = Convert.ToBoolean(reader["IsResolved"])
                });
            }

            return tickets;
        }

        // Çözülmüş tüm ticket'ları siler
        public static void DeleteResolvedTickets()
        {
             var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string query = "DELETE FROM Tickets WHERE IsResolved = 1";
            var cmd = new SQLiteCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        // Ticket'ları başka bir veritabanına arşivler
        public static void ArchiveTickets(List<Ticket> tickets, string archiveFileName)
        {
            string archivePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, archiveFileName);
            SQLiteConnection.CreateFile(archivePath);

            string archiveConnStr = $"Data Source={archivePath};Version=3;";

            var conn = new SQLiteConnection(archiveConnStr);
            conn.Open();

            string createQuery = @"
                CREATE TABLE Tickets (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Area TEXT,
                    Issue TEXT,
                    Description TEXT,
                    CreatedAt DATETIME
                );";

            using (var cmd = new SQLiteCommand(createQuery, conn))
                cmd.ExecuteNonQuery();

            foreach (var ticket in tickets)
            {
                string insertQuery = "INSERT INTO Tickets (Area, Issue, Description, CreatedAt) VALUES (@a, @i, @d, @c)";
                var cmd = new SQLiteCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@a", ticket.Area);
                cmd.Parameters.AddWithValue("@i", ticket.Issue);
                cmd.Parameters.AddWithValue("@d", ticket.Description);
                cmd.Parameters.AddWithValue("@c", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
