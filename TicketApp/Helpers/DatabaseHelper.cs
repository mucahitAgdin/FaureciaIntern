// Helpers/DatabaseHelper.cs
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
                        Area TEXT NOT NULL,
                        Issue TEXT NOT NULL,
                        Description TEXT NOT NULL,
                        CreatedAt DATETIME NOT NULL,
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
                Logger.Log(ex);
                throw new Exception("Veritabanı başlatılamadı", ex);
            }
        }

        /// <summary>
        /// Yeni bir ticket kaydı ekler.
        /// </summary>
        public static void InsertTicket(Ticket ticket)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string insertQuery = @"
                        INSERT INTO Tickets (Area, Issue, Description, CreatedAt, Status, IsResolved)
                        VALUES (@Area, @Issue, @Desc, @Date, @Status, @IsResolved);
                        SELECT last_insert_rowid();";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Area", ticket.Area);
                        cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                        cmd.Parameters.AddWithValue("@Desc", ticket.Description);
                        cmd.Parameters.AddWithValue("@Date", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Status", ticket.Status ?? "Beklemede");
                        cmd.Parameters.AddWithValue("@IsResolved", ticket.IsResolved ? 1 : 0);

                        ticket.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Ticket eklenirken hata oluştu", ex);
            }
        }

        /// <summary>
        /// Tüm ticket'ları listeler.
        /// </summary>
        public static List<Ticket> GetAllTickets()
        {
            var tickets = new List<Ticket>();

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Id, Area, Issue, Description, CreatedAt, IsResolved, Status FROM Tickets ORDER BY CreatedAt DESC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tickets.Add(new Ticket
                            {
                                Id = Convert.ToInt32(reader["Id"]),
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
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Ticketlar yüklenirken hata oluştu", ex);
            }

            return tickets;
        }

        /// <summary>
        /// Ticket ID'ye göre ticket siler.
        /// </summary>
        public static void DeleteTicket(int ticketId)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM Tickets WHERE Id = @Id";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", ticketId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Ticket silinirken hata oluştu", ex);
            }
        }

        /// <summary>
        /// Çözülmüş (IsResolved = 1) ticket'ları listeler.
        /// </summary>
        public static List<Ticket> GetResolvedTickets()
        {
            var tickets = new List<Ticket>();

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Id, Area, Issue, Description, CreatedAt, IsResolved, Status FROM Tickets WHERE IsResolved = 1";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tickets.Add(new Ticket
                            {
                                Id = Convert.ToInt32(reader["Id"]),
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
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Çözülmüş ticketlar yüklenirken hata oluştu", ex);
            }

            return tickets;
        }

        /// <summary>
        /// Tüm çözülmüş ticket'ları veritabanından siler.
        /// </summary>
        public static void DeleteResolvedTickets()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Çözülmüş ticketlar silinirken hata oluştu", ex);
            }
        }

        /// <summary>
        /// Verilen ticket listesini başka bir veritabanına arşivler.
        /// </summary>
        public static void ArchiveTickets(List<Ticket> tickets, string archiveFileName)
        {
            try
            {
                string archiveFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "archive");
                if (!Directory.Exists(archiveFolder))
                    Directory.CreateDirectory(archiveFolder);

                string archivePath = Path.Combine(archiveFolder, archiveFileName);
                SQLiteConnection.CreateFile(archivePath);

                string archiveConnStr = $"Data Source={archivePath};Version=3;";

                using (var conn = new SQLiteConnection(archiveConnStr))
                {
                    conn.Open();

                    string createQuery = @"
                        CREATE TABLE Tickets (
                            Id INTEGER PRIMARY KEY,
                            Area TEXT NOT NULL,
                            Issue TEXT NOT NULL,
                            Description TEXT NOT NULL,
                            CreatedAt DATETIME NOT NULL,
                            Status TEXT NOT NULL,
                            ArchivedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                        );";

                    using (var cmd = new SQLiteCommand(createQuery, conn))
                        cmd.ExecuteNonQuery();

                    foreach (var ticket in tickets)
                    {
                        string insertQuery = @"
                            INSERT INTO Tickets (Id, Area, Issue, Description, CreatedAt, Status) 
                            VALUES (@id, @area, @issue, @desc, @created, @status)";

                        using (var cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", ticket.Id);
                            cmd.Parameters.AddWithValue("@area", ticket.Area);
                            cmd.Parameters.AddWithValue("@issue", ticket.Issue);
                            cmd.Parameters.AddWithValue("@desc", ticket.Description);
                            cmd.Parameters.AddWithValue("@created", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@status", ticket.Status);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Arşivleme sırasında hata oluştu", ex);
            }
        }

        /// <summary>
        /// Ticket'ın durum bilgisini günceller.
        /// </summary>
        public static void UpdateTicketStatus(Ticket ticket)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE Tickets 
                        SET Status = @Status, IsResolved = @IsResolved 
                        WHERE Id = @Id";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", ticket.Status);
                        cmd.Parameters.AddWithValue("@IsResolved", ticket.Status == "çözüldü" ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Id", ticket.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Ticket durumu güncellenirken hata oluştu", ex);
            }
        }
    }
}