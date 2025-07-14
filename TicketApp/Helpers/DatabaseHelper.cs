using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using TicketApp.Models;

namespace TicketApp.Helpers
{
    /// <summary>
    /// SQLite veritabanı işlemlerini yöneten yardımcı sınıf.
    /// </summary>
    public static class DatabaseHelper
    {
        // Veritabanı dosyasının fiziksel yolu
        private static readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tickets.db");

        // SQLite bağlantı dizesi
        private static readonly string connectionString = $"Data Source={dbPath};Version=3;";

        /// <summary>
        /// Veritabanı dosyası ve Tickets tablosunu oluşturur (eğer yoksa).
        /// </summary>
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
                        IsResolved INTEGER DEFAULT 0,
                        Status TEXT DEFAULT 'Beklemede'
                    );";

                    using (var cmd = new SQLiteCommand(createQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex); // Logger sınıfı varsa log kaydı al
                throw;
            }
        }

        /// <summary>
        /// Yeni bir ticket kaydı ekler.
        /// </summary>
        public static void InsertTicket(Ticket ticket)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string insertQuery = @"
                    INSERT INTO Tickets (Area, Issue, Description, CreatedAt, Status)
                    VALUES (@Area, @Issue, @Desc, @Date, @Status);";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Area", ticket.Area);
                    cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                    cmd.Parameters.AddWithValue("@Desc", ticket.Description);
                    cmd.Parameters.AddWithValue("@Date", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Status", ticket.Status ?? "Beklemede");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Tüm ticket'ları listeler.
        /// </summary>
        public static List<Ticket> GetAllTickets()
        {
            var tickets = new List<Ticket>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT Area, Issue, Description, CreatedAt, IsResolved, Status FROM Tickets ORDER BY CreatedAt DESC";

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
                            CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                            IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return tickets;
        }

        /// <summary>
        /// Belirli bir CreatedAt tarihine göre ticket siler.
        /// </summary>
        public static void DeleteTicket(DateTime createdAt)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM Tickets WHERE CreatedAt = @Date";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", createdAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Çözülmüş (IsResolved = 1) ticket'ları listeler.
        /// </summary>
        public static List<Ticket> GetResolvedTickets()
        {
            var tickets = new List<Ticket>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Tickets WHERE IsResolved = 1";

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
                            CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                            IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return tickets;
        }

        /// <summary>
        /// Tüm çözülmüş ticket'ları veritabanından siler.
        /// </summary>
        public static void DeleteResolvedTickets()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM Tickets WHERE IsResolved = 1";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Verilen ticket listesini başka bir veritabanına arşivler.
        /// </summary>
        public static void ArchiveTickets(List<Ticket> tickets, string archiveFileName)
        {
            string archivePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, archiveFileName);
            SQLiteConnection.CreateFile(archivePath);

            string archiveConnStr = $"Data Source={archivePath};Version=3;";

            using (var conn = new SQLiteConnection(archiveConnStr))
            {
                conn.Open();

                string createQuery = @"
                    CREATE TABLE Tickets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Area TEXT,
                        Issue TEXT,
                        Description TEXT,
                        CreatedAt DATETIME,
                        Status TEXT
                    );";

                using (var cmd = new SQLiteCommand(createQuery, conn))
                    cmd.ExecuteNonQuery();

                foreach (var ticket in tickets)
                {
                    string insertQuery = "INSERT INTO Tickets (Area, Issue, Description, CreatedAt, Status) VALUES (@a, @i, @d, @c, @s)";
                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@a", ticket.Area);
                        cmd.Parameters.AddWithValue("@i", ticket.Issue);
                        cmd.Parameters.AddWithValue("@d", ticket.Description);
                        cmd.Parameters.AddWithValue("@c", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@s", ticket.Status);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Ticket'ın durum bilgisini (Status) günceller.
        /// </summary>
        public static void UpdateTicketStatus(Ticket ticket)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Tickets SET Status = @Status WHERE CreatedAt = @CreatedAt";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", ticket.Status);
                    cmd.Parameters.AddWithValue("@CreatedAt", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
