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

                    // Önce tabloyu kontrol et
                    string checkTableQuery = @"
                        SELECT name FROM sqlite_master 
                        WHERE type='table' AND name='Tickets';";

                    bool tableExists = false;
                    using (var cmd = new SQLiteCommand(checkTableQuery, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        tableExists = result != null;
                    }

                    if (!tableExists)
                    {
                        // Yeni tablo oluştur
                        string createQuery = @"
                        CREATE TABLE Tickets (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Area TEXT NOT NULL,
                            SubArea TEXT NOT NULL,
                            Issue TEXT NOT NULL,
                            Description TEXT NOT NULL,
                            FirstName TEXT NOT NULL,
                            LastName TEXT NOT NULL,
                            PhoneNumber TEXT NOT NULL,
                            CreatedAt DATETIME NOT NULL,
                            IsResolved INTEGER DEFAULT 0,
                            Status TEXT DEFAULT 'beklemede',
                            AssignedTo TEXT,
                            RejectionReason TEXT
                        );";

                        using (var cmd = new SQLiteCommand(createQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Mevcut tabloya yeni kolonları ekle
                        AddMissingColumns(conn);
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
        /// Mevcut tabloya eksik kolonları ekler
        /// </summary>
        private static void AddMissingColumns(SQLiteConnection conn)
        {
            try
            {
                // Mevcut kolonları kontrol et
                string checkColumnQuery = @"PRAGMA table_info(Tickets);";
                var existingColumns = new List<string>();

                using (var cmd = new SQLiteCommand(checkColumnQuery, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingColumns.Add(reader["name"].ToString());
                    }
                }

                // Eksik kolonları ekle
                var requiredColumns = new Dictionary<string, string>
                {
                    ["SubArea"] = "ALTER TABLE Tickets ADD COLUMN SubArea TEXT DEFAULT '';",
                    ["FirstName"] = "ALTER TABLE Tickets ADD COLUMN FirstName TEXT DEFAULT '';",
                    ["LastName"] = "ALTER TABLE Tickets ADD COLUMN LastName TEXT DEFAULT '';",
                    ["PhoneNumber"] = "ALTER TABLE Tickets ADD COLUMN PhoneNumber TEXT DEFAULT '';",
                    ["AssignedTo"] = "ALTER TABLE Tickets ADD COLUMN AssignedTo TEXT;",
                    ["RejectionReason"] = "ALTER TABLE Tickets ADD COLUMN RejectionReason TEXT;"
                };

                foreach (var column in requiredColumns)
                {
                    if (!existingColumns.Contains(column.Key))
                    {
                        using (var cmd = new SQLiteCommand(column.Value, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
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
                        INSERT INTO Tickets (Area, SubArea, Issue, Description, FirstName, LastName, PhoneNumber, CreatedAt, Status, IsResolved, AssignedTo, RejectionReason)
                        VALUES (@Area, @SubArea, @Issue, @Desc, @FirstName, @LastName, @PhoneNumber, @Date, @Status, @IsResolved, @AssignedTo, @RejectionReason);
                        SELECT last_insert_rowid();";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Area", ticket.Area);
                        cmd.Parameters.AddWithValue("@SubArea", ticket.SubArea ?? "");
                        cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                        cmd.Parameters.AddWithValue("@Desc", ticket.Description);
                        cmd.Parameters.AddWithValue("@FirstName", ticket.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", ticket.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", ticket.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Date", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Status", ticket.Status ?? "beklemede");
                        cmd.Parameters.AddWithValue("@IsResolved", ticket.IsResolved ? 1 : 0);
                        cmd.Parameters.AddWithValue("@AssignedTo", ticket.AssignedTo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RejectionReason", ticket.RejectionReason ?? (object)DBNull.Value);

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

                    string query = @"SELECT Id, Area, SubArea, Issue, Description, FirstName, LastName, PhoneNumber, 
                                   CreatedAt, IsResolved, Status, AssignedTo, RejectionReason 
                                   FROM Tickets ORDER BY CreatedAt DESC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tickets.Add(new Ticket
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Area = reader["Area"]?.ToString() ?? "",
                                SubArea = reader["SubArea"]?.ToString() ?? "",
                                Issue = reader["Issue"]?.ToString() ?? "",
                                Description = reader["Description"]?.ToString() ?? "",
                                FirstName = reader["FirstName"]?.ToString() ?? "",
                                LastName = reader["LastName"]?.ToString() ?? "",
                                PhoneNumber = reader["PhoneNumber"]?.ToString() ?? "",
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                                IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                                Status = reader["Status"]?.ToString() ?? "beklemede",
                                AssignedTo = reader["AssignedTo"] == DBNull.Value ? null : reader["AssignedTo"].ToString(),
                                RejectionReason = reader["RejectionReason"] == DBNull.Value ? null : reader["RejectionReason"].ToString()
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
        /// Belirli bir kullanıcıya ait ticket'ları listeler
        /// </summary>
        public static List<Ticket> GetTicketsByUser(string firstName, string lastName, string phoneNumber)
        {
            var tickets = new List<Ticket>();

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT Id, Area, SubArea, Issue, Description, FirstName, LastName, PhoneNumber, 
                                   CreatedAt, IsResolved, Status, AssignedTo, RejectionReason 
                                   FROM Tickets 
                                   WHERE FirstName = @FirstName AND LastName = @LastName AND PhoneNumber = @PhoneNumber
                                   ORDER BY CreatedAt DESC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tickets.Add(new Ticket
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Area = reader["Area"]?.ToString() ?? "",
                                    SubArea = reader["SubArea"]?.ToString() ?? "",
                                    Issue = reader["Issue"]?.ToString() ?? "",
                                    Description = reader["Description"]?.ToString() ?? "",
                                    FirstName = reader["FirstName"]?.ToString() ?? "",
                                    LastName = reader["LastName"]?.ToString() ?? "",
                                    PhoneNumber = reader["PhoneNumber"]?.ToString() ?? "",
                                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                                    IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                                    Status = reader["Status"]?.ToString() ?? "beklemede",
                                    AssignedTo = reader["AssignedTo"] == DBNull.Value ? null : reader["AssignedTo"].ToString(),
                                    RejectionReason = reader["RejectionReason"] == DBNull.Value ? null : reader["RejectionReason"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new Exception("Kullanıcı ticketları yüklenirken hata oluştu", ex);
            }

            return tickets;
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

                    string query = @"SELECT Id, Area, SubArea, Issue, Description, FirstName, LastName, PhoneNumber, 
                                   CreatedAt, IsResolved, Status, AssignedTo, RejectionReason 
                                   FROM Tickets WHERE IsResolved = 1";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tickets.Add(new Ticket
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Area = reader["Area"]?.ToString() ?? "",
                                SubArea = reader["SubArea"]?.ToString() ?? "",
                                Issue = reader["Issue"]?.ToString() ?? "",
                                Description = reader["Description"]?.ToString() ?? "",
                                FirstName = reader["FirstName"]?.ToString() ?? "",
                                LastName = reader["LastName"]?.ToString() ?? "",
                                PhoneNumber = reader["PhoneNumber"]?.ToString() ?? "",
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                                IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                                Status = reader["Status"]?.ToString() ?? "beklemede",
                                AssignedTo = reader["AssignedTo"] == DBNull.Value ? null : reader["AssignedTo"].ToString(),
                                RejectionReason = reader["RejectionReason"] == DBNull.Value ? null : reader["RejectionReason"].ToString()
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
                            SubArea TEXT NOT NULL,
                            Issue TEXT NOT NULL,
                            Description TEXT NOT NULL,
                            FirstName TEXT NOT NULL,
                            LastName TEXT NOT NULL,
                            PhoneNumber TEXT NOT NULL,
                            CreatedAt DATETIME NOT NULL,
                            Status TEXT NOT NULL,
                            AssignedTo TEXT,
                            RejectionReason TEXT,
                            ArchivedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                        );";

                    using (var cmd = new SQLiteCommand(createQuery, conn))
                        cmd.ExecuteNonQuery();

                    foreach (var ticket in tickets)
                    {
                        string insertQuery = @"
                            INSERT INTO Tickets (Id, Area, SubArea, Issue, Description, FirstName, LastName, PhoneNumber, CreatedAt, Status, AssignedTo, RejectionReason) 
                            VALUES (@id, @area, @subarea, @issue, @desc, @fname, @lname, @phone, @created, @status, @assigned, @rejected)";

                        using (var cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", ticket.Id);
                            cmd.Parameters.AddWithValue("@area", ticket.Area);
                            cmd.Parameters.AddWithValue("@subarea", ticket.SubArea ?? "");
                            cmd.Parameters.AddWithValue("@issue", ticket.Issue);
                            cmd.Parameters.AddWithValue("@desc", ticket.Description);
                            cmd.Parameters.AddWithValue("@fname", ticket.FirstName);
                            cmd.Parameters.AddWithValue("@lname", ticket.LastName);
                            cmd.Parameters.AddWithValue("@phone", ticket.PhoneNumber);
                            cmd.Parameters.AddWithValue("@created", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@status", ticket.Status);
                            cmd.Parameters.AddWithValue("@assigned", ticket.AssignedTo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@rejected", ticket.RejectionReason ?? (object)DBNull.Value);
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
        /// Ticket durumunu ve atama bilgisini günceller
        /// </summary>
        /// <param name="ticket">Güncellenecek ticket</param>
        /// <returns>Güncelleme başarılı ise true</returns>
        public static bool UpdateTicketStatus(Ticket ticket)
        {
            try
            {
                string query = @"
                UPDATE Tickets 
                SET Status = @Status, 
                    AssignedTo = @AssignedTo,
                    IsResolved = @IsResolved,
                    RejectionReason = @RejectionReason
                WHERE Id = @Id";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", ticket.Status);
                        command.Parameters.AddWithValue("@AssignedTo", ticket.AssignedTo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsResolved", ticket.IsResolved ? 1 : 0);
                        command.Parameters.AddWithValue("@RejectionReason", ticket.RejectionReason ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Id", ticket.Id);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        /// Ticket'ı belirli bir kişiye atar
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="assignedTo">Atanacak kişi</param>
        /// <returns>Atama başarılı ise true</returns>
        public static bool AssignTicket(int ticketId, string assignedTo)
        {
            try
            {
                string query = @"
                UPDATE Tickets 
                SET AssignedTo = @AssignedTo,
                    Status = 'işlemde'
                WHERE Id = @Id";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AssignedTo", assignedTo);
                        command.Parameters.AddWithValue("@Id", ticketId);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }
    }
}