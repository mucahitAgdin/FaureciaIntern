// Helpers/DatabaseHelper.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security;
using TicketApp.Models;
using System.Configuration;

namespace TicketApp.Helpers
{
    /// <summary>
    /// SQLite veritabanı işlemlerini yöneten güvenli yardımcı sınıf.
    /// Tüm sorgular parametreli olarak yazılmıştır (SQL Injection koruması).
    /// </summary>
    public static class DatabaseHelper
    {
        #region Fields & Properties



        // Tablo isimleri (typo önlemek için sabit)
        private const string TICKETS_TABLE = "Tickets";
        private const string AREAS_TABLE = "Areas";
        private const string SUBAREAS_TABLE = "SubAreas";
        private const string ISSUES_TABLE = "Issues";

        // Ortak klasör yolu - TÜM BİLGİSAYARLARDA AYNI OLMALI
        private static readonly string SHARED_FOLDER =
        ConfigurationManager.AppSettings["SharedFolderPath"].TrimEnd('\\');
        
        private static readonly string DB_FILENAME = ConfigurationManager.AppSettings["DatabaseFileName"];

        // Veritabanı yolu
        private static readonly string dbPath = @"C:\TicketAppShared\tickets.db";


        // Connection string - Multi-user için özel ayarlar
        private static readonly string connectionString =
    $"Data Source={dbPath};Version=3;Journal Mode=WAL;Cache Size=10000;Temp Store=Memory;Synchronous=NORMAL;Busy Timeout=10000;Default Timeout=30;";


        #endregion

        #region Database Initialization

        /// <summary>
        /// Veritabanı ve tabloları oluşturur, varsayılan verileri yükler
        /// </summary>
        public static void InitializeDatabase()
        {
            try
            {
                string folder = Path.GetDirectoryName(dbPath);

                if (string.IsNullOrWhiteSpace(folder))
                    throw new Exception($"Veritabanı yolu geçersiz: {dbPath}");

                // 📁 Klasör yoksa oluştur
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    Logger.Log($"Veritabanı klasörü oluşturuldu: {folder}");
                }

                // 🔐 Erişim testi
                if (!File.Exists(dbPath))
                {
                    try
                    {
                        SQLiteConnection.CreateFile(dbPath);
                        Logger.Log("Veritabanı dosyası oluşturuldu.");
                    }
                    catch (Exception fileEx)
                    {
                        throw new IOException($"Veritabanı dosyası oluşturulamadı: {dbPath}", fileEx);
                    }
                }

                // 🧪 Dosya gerçekten erişilebilir mi? (ön test)
                try
                {
                    using (var fs = File.Open(dbPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        Logger.Log("Veritabanı dosyasına erişim başarılı.");
                    }
                }
                catch (Exception fsEx)
                {
                    throw new IOException($"Veritabanı dosyasına erişilemiyor: {dbPath}", fsEx);
                }

                // 🚀 Veritabanı bağlantısını başlat
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            CreateTables(conn);
                            UpdateExistingTables(conn);
                            LoadDefaultDataIfEmpty(conn);

                            transaction.Commit();
                            Logger.Log("Veritabanı başarıyla başlatıldı.");
                        }
                        catch (Exception innerEx)
                        {
                            transaction.Rollback();
                            Logger.Log($"Transaction geri alındı: {innerEx.Message}");
                            throw;
                        }
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
        /// Tüm tabloları oluşturur
        /// </summary>
        private static void CreateTables(SQLiteConnection conn)
        {
            // Areas tablosu - Ana alanları tutar (UAP-1, UAP-2, FES vb.)
            string createAreasTable = $@"
                CREATE TABLE IF NOT EXISTS {AREAS_TABLE} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AreaName TEXT NOT NULL UNIQUE,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            // SubAreas tablosu - Alt alanları tutar
            string createSubAreasTable = $@"
                CREATE TABLE IF NOT EXISTS {SUBAREAS_TABLE} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AreaId INTEGER NOT NULL,
                    SubAreaName TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (AreaId) REFERENCES {AREAS_TABLE}(Id) ON DELETE CASCADE,
                    UNIQUE(AreaId, SubAreaName)
                );";

            // Issues tablosu - Sorun tiplerini tutar
            string createIssuesTable = $@"
                CREATE TABLE IF NOT EXISTS {ISSUES_TABLE} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AreaName TEXT NOT NULL,
                    IssueName TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UNIQUE(AreaName, IssueName)
                );";

            // Tickets tablosu - Ana ticket verilerini tutar
            string createTicketsTable = $@"
                CREATE TABLE IF NOT EXISTS {TICKETS_TABLE} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Area TEXT NOT NULL,
                    SubArea TEXT,
                    Issue TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    PhoneNumber TEXT NOT NULL,
                    CreatedAt DATETIME NOT NULL,
                    IsResolved INTEGER DEFAULT 0,
                    Status TEXT DEFAULT 'beklemede' CHECK(Status IN ('beklemede', 'işlemde', 'çözüldü', 'reddedildi')),
                    AssignedTo TEXT,
                    RejectionReason TEXT,
                    UpdatedAt DATETIME,
                    ResolvedAt DATETIME
                );";

            // Index'ler - Performans için
            string createIndexes = @"
                CREATE INDEX IF NOT EXISTS idx_tickets_status ON Tickets(Status);
                CREATE INDEX IF NOT EXISTS idx_tickets_area ON Tickets(Area);
                CREATE INDEX IF NOT EXISTS idx_tickets_created ON Tickets(CreatedAt);
                CREATE INDEX IF NOT EXISTS idx_tickets_assigned ON Tickets(AssignedTo);
            ";

            // Tabloları oluştur
            ExecuteNonQuery(conn, createAreasTable);
            ExecuteNonQuery(conn, createSubAreasTable);
            ExecuteNonQuery(conn, createIssuesTable);
            ExecuteNonQuery(conn, createTicketsTable);
            ExecuteNonQuery(conn, createIndexes);
        }

        /// <summary>
        /// Mevcut tablolara eksik kolonları ekler (migration)
        /// </summary>
        private static void UpdateExistingTables(SQLiteConnection conn)
        {
            try
            {
                // Tickets tablosundaki mevcut kolonları kontrol et
                var existingColumns = GetTableColumns(conn, TICKETS_TABLE);

                // Eksik kolonları ekle
                var columnsToAdd = new Dictionary<string, string>
                {
                    ["UpdatedAt"] = $"ALTER TABLE {TICKETS_TABLE} ADD COLUMN UpdatedAt DATETIME;",
                    ["ResolvedAt"] = $"ALTER TABLE {TICKETS_TABLE} ADD COLUMN ResolvedAt DATETIME;"
                };

                foreach (var column in columnsToAdd)
                {
                    if (!existingColumns.Contains(column.Key, StringComparer.OrdinalIgnoreCase))
                    {
                        ExecuteNonQuery(conn, column.Value);
                        Logger.Log($"'{column.Key}' kolonu eklendi.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Tablo güncelleme hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Varsayılan verileri yükler (ilk kurulum için)
        /// </summary>
        private static void LoadDefaultDataIfEmpty(SQLiteConnection conn)
        {
            // Areas tablosu boş mu kontrol et
            string checkQuery = $"SELECT COUNT(*) FROM {AREAS_TABLE}";
            int count = Convert.ToInt32(ExecuteScalar(conn, checkQuery));

            if (count == 0)
            {
                Logger.Log("Varsayılan veriler yükleniyor...");

                // Alan ve alt alanları yükle
                var areaMap = AreaCatalog.GetAreaSubAreaMap();
                SaveAreaSubAreaMapInternal(conn, areaMap);

                // Sorunları yükle
                var issueMap = IssueCatalog.GetIssueMap();
                SaveIssueMapInternal(conn, issueMap);

                Logger.Log("Varsayılan veriler başarıyla yüklendi.");
            }
        }

        #endregion

        #region Ticket Operations

        /// <summary>
        /// Yeni ticket ekler (SQL Injection korumalı)
        /// </summary>
        public static int InsertTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            // Veri doğrulama
            ValidateTicket(ticket);

            string insertQuery = $@"
                INSERT INTO {TICKETS_TABLE} (
                    Area, SubArea, Issue, Description, 
                    FirstName, LastName, PhoneNumber, 
                    CreatedAt, Status, IsResolved, 
                    AssignedTo, RejectionReason
                )
                VALUES (
                    @Area, @SubArea, @Issue, @Description, 
                    @FirstName, @LastName, @PhoneNumber, 
                    @CreatedAt, @Status, @IsResolved, 
                    @AssignedTo, @RejectionReason
                );
                SELECT last_insert_rowid();";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    // Parametreleri güvenli şekilde ekle
                    cmd.Parameters.AddWithValue("@Area", ticket.Area);
                    cmd.Parameters.AddWithValue("@SubArea", ticket.SubArea ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                    cmd.Parameters.AddWithValue("@Description", ticket.Description);
                    cmd.Parameters.AddWithValue("@FirstName", ticket.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", ticket.LastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", ticket.PhoneNumber);
                    cmd.Parameters.AddWithValue("@CreatedAt", ticket.CreatedAt);
                    cmd.Parameters.AddWithValue("@Status", ticket.Status ?? "beklemede");
                    cmd.Parameters.AddWithValue("@IsResolved", ticket.IsResolved);
                    cmd.Parameters.AddWithValue("@AssignedTo", (object)ticket.AssignedTo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RejectionReason", (object)ticket.RejectionReason ?? DBNull.Value);

                    ticket.Id = Convert.ToInt32(cmd.ExecuteScalar());

                    Logger.Log($"Yeni ticket eklendi: ID={ticket.Id}, Alan={ticket.Area}, Sorun={ticket.Issue}");
                    return ticket.Id;
                }
            }
        }

        /// <summary>
        /// Tüm ticketları getirir (güvenli)
        /// </summary>
        public static List<Ticket> GetAllTickets()
        {
            string query = $@"
                SELECT Id, Area, SubArea, Issue, Description, 
                       FirstName, LastName, PhoneNumber, CreatedAt, 
                       IsResolved, Status, AssignedTo, RejectionReason,
                       UpdatedAt, ResolvedAt
                FROM {TICKETS_TABLE} 
                ORDER BY CreatedAt DESC";

            return ExecuteTicketQuery(query);
        }

        /// <summary>
        /// Belirli duruma göre ticketları getirir
        /// </summary>
        public static List<Ticket> GetTicketsByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Durum boş olamaz", nameof(status));

            string query = $@"
                SELECT Id, Area, SubArea, Issue, Description, 
                       FirstName, LastName, PhoneNumber, CreatedAt, 
                       IsResolved, Status, AssignedTo, RejectionReason,
                       UpdatedAt, ResolvedAt
                FROM {TICKETS_TABLE} 
                WHERE Status = @Status
                ORDER BY CreatedAt DESC";

            return ExecuteTicketQuery(query, ("@Status", status));
        }

        /// <summary>
        /// Kullanıcıya ait ticketları getirir
        /// </summary>
        public static List<Ticket> GetTicketsByUser(string firstName, string lastName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Kullanıcı bilgileri eksik");
            }

            string query = $@"
                SELECT Id, Area, SubArea, Issue, Description, 
                       FirstName, LastName, PhoneNumber, CreatedAt, 
                       IsResolved, Status, AssignedTo, RejectionReason,
                       UpdatedAt, ResolvedAt
                FROM {TICKETS_TABLE} 
                WHERE FirstName = @FirstName 
                  AND LastName = @LastName 
                  AND PhoneNumber = @PhoneNumber
                ORDER BY CreatedAt DESC";

            return ExecuteTicketQuery(query,
                ("@FirstName", firstName),
                ("@LastName", lastName),
                ("@PhoneNumber", phoneNumber));
        }

        /// <summary>
        /// Ticket durumunu günceller (SQL Injection korumalı)
        /// </summary>
        public static bool UpdateTicketStatus(Ticket ticket)
        {
            if (ticket == null || ticket.Id <= 0)
                return false;

            string updateQuery = $@"
                UPDATE {TICKETS_TABLE} 
                SET Status = @Status, 
                    AssignedTo = @AssignedTo,
                    IsResolved = @IsResolved,
                    RejectionReason = @RejectionReason,
                    UpdatedAt = @UpdatedAt,
                    ResolvedAt = @ResolvedAt
                WHERE Id = @Id";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", ticket.Status);
                    cmd.Parameters.AddWithValue("@AssignedTo", (object)ticket.AssignedTo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsResolved", ticket.IsResolved);
                    cmd.Parameters.AddWithValue("@RejectionReason", (object)ticket.RejectionReason ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ResolvedAt",
                        ticket.Status == "çözüldü" ? (object)DateTime.Now : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", ticket.Id);

                    int affected = cmd.ExecuteNonQuery();

                    if (affected > 0)
                    {
                        Logger.Log($"Ticket güncellendi: ID={ticket.Id}, Yeni Durum={ticket.Status}");
                    }

                    return affected > 0;
                }
            }
        }

        /// <summary>
        /// Ticket siler (güvenli)
        /// </summary>
        public static bool DeleteTicket(int ticketId)
        {
            if (ticketId <= 0)
                return false;

            string deleteQuery = $"DELETE FROM {TICKETS_TABLE} WHERE Id = @Id";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", ticketId);
                    int affected = cmd.ExecuteNonQuery();

                    if (affected > 0)
                    {
                        Logger.Log($"Ticket silindi: ID={ticketId}");
                    }

                    return affected > 0;
                }
            }
        }

        #endregion

        #region Area & SubArea Operations

        /// <summary>
        /// Alan-alt alan haritasını kaydeder
        /// </summary>
        public static void SaveAreaSubAreaMap(Dictionary<string, List<string>> areaMap)
        {
            if (areaMap == null)
                throw new ArgumentNullException(nameof(areaMap));

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        SaveAreaSubAreaMapInternal(conn, areaMap);
                        transaction.Commit();
                        Logger.Log("Alan-alt alan haritası güncellendi.");
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Alan-alt alan haritasını getirir
        /// </summary>
        public static Dictionary<string, List<string>> GetAreaSubAreaMap()
        {
            var areaMap = new Dictionary<string, List<string>>();

            string query = $@"
                SELECT a.AreaName, s.SubAreaName
                FROM {AREAS_TABLE} a
                LEFT JOIN {SUBAREAS_TABLE} s ON a.Id = s.AreaId
                ORDER BY a.AreaName, s.SubAreaName";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string areaName = reader["AreaName"].ToString();
                        string subAreaName = reader["SubAreaName"]?.ToString();

                        if (!areaMap.ContainsKey(areaName))
                        {
                            areaMap[areaName] = new List<string>();
                        }

                        if (!string.IsNullOrEmpty(subAreaName))
                        {
                            areaMap[areaName].Add(subAreaName);
                        }
                    }
                }
            }

            return areaMap;
        }

        /// <summary>
        /// Belirli bir alan için ticket var mı kontrol eder
        /// </summary>
        public static bool HasTicketsForArea(string areaName)
        {
            if (string.IsNullOrWhiteSpace(areaName))
                return false;

            string query = $"SELECT COUNT(*) FROM {TICKETS_TABLE} WHERE Area = @AreaName";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AreaName", areaName);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Belirli bir alt alan için ticket var mı kontrol eder
        /// </summary>
        public static bool HasTicketsForSubArea(string areaName, string subAreaName)
        {
            if (string.IsNullOrWhiteSpace(areaName) || string.IsNullOrWhiteSpace(subAreaName))
                return false;

            string query = $@"
                SELECT COUNT(*) FROM {TICKETS_TABLE} 
                WHERE Area = @AreaName AND SubArea = @SubAreaName";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AreaName", areaName);
                    cmd.Parameters.AddWithValue("@SubAreaName", subAreaName);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        #endregion

        #region Issue Operations

        /// <summary>
        /// Sorun haritasını kaydeder
        /// </summary>
        public static void SaveIssueMap(Dictionary<string, List<string>> issueMap)
        {
            if (issueMap == null)
                throw new ArgumentNullException(nameof(issueMap));

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        SaveIssueMapInternal(conn, issueMap);
                        transaction.Commit();
                        Logger.Log("Sorun haritası güncellendi.");
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Sorun haritasını getirir
        /// </summary>
        public static Dictionary<string, List<string>> GetIssueMap()
        {
            var issueMap = new Dictionary<string, List<string>>();

            string query = $@"
                SELECT AreaName, IssueName 
                FROM {ISSUES_TABLE} 
                ORDER BY AreaName, IssueName";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string areaName = reader["AreaName"].ToString();
                        string issueName = reader["IssueName"].ToString();

                        if (!issueMap.ContainsKey(areaName))
                        {
                            issueMap[areaName] = new List<string>();
                        }

                        issueMap[areaName].Add(issueName);
                    }
                }
            }

            return issueMap;
        }

        /// <summary>
        /// Belirli bir sorun için ticket var mı kontrol eder
        /// </summary>
        public static bool HasTicketsForIssue(string issueName)
        {
            if (string.IsNullOrWhiteSpace(issueName))
                return false;

            string query = $"SELECT COUNT(*) FROM {TICKETS_TABLE} WHERE Issue = @IssueName";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IssueName", issueName);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        #endregion

        #region Archive Operations

        /// <summary>
        /// Çözülmüş ticketları getirir
        /// </summary>
        public static List<Ticket> GetResolvedTickets()
        {
            return GetTicketsByStatus("çözüldü");
        }

        /// <summary>
        /// Ticketları arşive taşır
        /// </summary>
        public static void ArchiveTickets(List<Ticket> tickets, string archiveFileName)
        {
            if (tickets == null || tickets.Count == 0)
                return;

            if (string.IsNullOrWhiteSpace(archiveFileName))
                throw new ArgumentException("Arşiv dosya adı boş olamaz", nameof(archiveFileName));

            // Arşiv klasörünü oluştur
            string archiveFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "archive");
            if (!Directory.Exists(archiveFolder))
            {
                Directory.CreateDirectory(archiveFolder);
            }

            string archivePath = Path.Combine(archiveFolder, archiveFileName);

            // Güvenlik: Dosya yolu dışına çıkmasını engelle
            if (!archivePath.StartsWith(archiveFolder))
            {
                throw new SecurityException("Geçersiz arşiv yolu");
            }

            SQLiteConnection.CreateFile(archivePath);
            string archiveConnStr = $"Data Source={archivePath};Version=3;";

            using (var conn = new SQLiteConnection(archiveConnStr))
            {
                conn.Open();

                // Arşiv tablosunu oluştur
                string createArchiveTable = $@"
                    CREATE TABLE ArchivedTickets (
                        Id INTEGER PRIMARY KEY,
                        Area TEXT NOT NULL,
                        SubArea TEXT,
                        Issue TEXT NOT NULL,
                        Description TEXT NOT NULL,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        PhoneNumber TEXT NOT NULL,
                        CreatedAt DATETIME NOT NULL,
                        ResolvedAt DATETIME,
                        Status TEXT NOT NULL,
                        AssignedTo TEXT,
                        RejectionReason TEXT,
                        ArchivedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                using (var cmd = new SQLiteCommand(createArchiveTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Ticketları arşive kopyala
                foreach (var ticket in tickets)
                {
                    string insertQuery = @"
                        INSERT INTO ArchivedTickets (
                            Id, Area, SubArea, Issue, Description, 
                            FirstName, LastName, PhoneNumber, 
                            CreatedAt, ResolvedAt, Status, 
                            AssignedTo, RejectionReason
                        ) VALUES (
                            @Id, @Area, @SubArea, @Issue, @Description, 
                            @FirstName, @LastName, @PhoneNumber, 
                            @CreatedAt, @ResolvedAt, @Status, 
                            @AssignedTo, @RejectionReason
                        )";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", ticket.Id);
                        cmd.Parameters.AddWithValue("@Area", ticket.Area);
                        cmd.Parameters.AddWithValue("@SubArea", ticket.SubArea ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                        cmd.Parameters.AddWithValue("@Description", ticket.Description);
                        cmd.Parameters.AddWithValue("@FirstName", ticket.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", ticket.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", ticket.PhoneNumber);
                        cmd.Parameters.AddWithValue("@CreatedAt", ticket.CreatedAt);
                        cmd.Parameters.AddWithValue("@ResolvedAt", (object)ticket.ResolvedAt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", ticket.Status);
                        cmd.Parameters.AddWithValue("@AssignedTo", (object)ticket.AssignedTo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@RejectionReason", (object)ticket.RejectionReason ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            Logger.Log($"{tickets.Count} ticket arşivlendi: {archiveFileName}");
        }

        /// <summary>
        /// Çözülmüş ticketları siler
        /// </summary>
        public static void DeleteResolvedTickets()
        {
            string deleteQuery = $"DELETE FROM {TICKETS_TABLE} WHERE Status = @Status";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", "çözüldü");
                    int affected = cmd.ExecuteNonQuery();
                    Logger.Log($"{affected} çözülmüş ticket silindi.");
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Ticket verilerini doğrular
        /// </summary>
        private static void ValidateTicket(Ticket ticket)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(ticket.Area))
                errors.Add("Alan boş olamaz");

            if (string.IsNullOrWhiteSpace(ticket.Issue))
                errors.Add("Sorun tipi boş olamaz");

            if (string.IsNullOrWhiteSpace(ticket.Description))
                errors.Add("Açıklama boş olamaz");

            if (ticket.Description?.Length > 300)
                errors.Add("Açıklama 300 karakterden uzun olamaz");

            if (string.IsNullOrWhiteSpace(ticket.FirstName))
                errors.Add("Ad boş olamaz");

            if (string.IsNullOrWhiteSpace(ticket.LastName))
                errors.Add("Soyad boş olamaz");

            if (string.IsNullOrWhiteSpace(ticket.PhoneNumber))
                errors.Add("Telefon numarası boş olamaz");

            if (errors.Count > 0)
            {
                throw new ArgumentException("Ticket verileri geçersiz: " + string.Join(", ", errors));
            }
        }

        /// <summary>
        /// Parametreli sorgu çalıştırır (SELECT)
        /// </summary>
        private static List<Ticket> ExecuteTicketQuery(string query, params (string name, object value)[] parameters)
        {
            var tickets = new List<Ticket>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    // Parametreleri ekle
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.name, param.value);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tickets.Add(MapReaderToTicket(reader));
                        }
                    }
                }
            }

            return tickets;
        }

        /// <summary>
        /// DataReader'dan Ticket nesnesine dönüştürür
        /// </summary>
        private static Ticket MapReaderToTicket(SQLiteDataReader reader)
        {
            return new Ticket
            {
                Id = Convert.ToInt32(reader["Id"]),
                Area = reader["Area"].ToString(),
                SubArea = reader["SubArea"]?.ToString() ?? string.Empty,
                Issue = reader["Issue"].ToString(),
                Description = reader["Description"].ToString(),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                PhoneNumber = reader["PhoneNumber"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                Status = reader["Status"]?.ToString() ?? "beklemede",
                AssignedTo = reader["AssignedTo"] == DBNull.Value ? null : reader["AssignedTo"].ToString(),
                RejectionReason = reader["RejectionReason"] == DBNull.Value ? null : reader["RejectionReason"].ToString()
            };
        }

        /// <summary>
        /// Tek bir değer döndüren sorgu çalıştırır
        /// </summary>
        private static object ExecuteScalar(SQLiteConnection conn, string query, params (string name, object value)[] parameters)
        {
            using (var cmd = new SQLiteCommand(query, conn))
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.name, param.value);
                }
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// INSERT, UPDATE, DELETE sorguları çalıştırır
        /// </summary>
        private static int ExecuteNonQuery(SQLiteConnection conn, string query, params (string name, object value)[] parameters)
        {
            using (var cmd = new SQLiteCommand(query, conn))
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.name, param.value);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Tablonun kolon isimlerini getirir
        /// </summary>
        private static List<string> GetTableColumns(SQLiteConnection conn, string tableName)
        {
            var columns = new List<string>();

            // SQL Injection koruması için tablo adını kontrol et
            if (!IsValidTableName(tableName))
                throw new ArgumentException("Geçersiz tablo adı");

            string query = $"PRAGMA table_info({tableName})";

            using (var cmd = new SQLiteCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    columns.Add(reader["name"].ToString());
                }
            }

            return columns;
        }

        /// <summary>
        /// Tablo adının geçerli olup olmadığını kontrol eder
        /// </summary>
        private static bool IsValidTableName(string tableName)
        {
            // Sadece harf, rakam ve alt çizgi kabul et
            return !string.IsNullOrWhiteSpace(tableName) &&
                   System.Text.RegularExpressions.Regex.IsMatch(tableName, @"^[a-zA-Z0-9_]+$");
        }

        /// <summary>
        /// Alan-alt alan haritasını internal olarak kaydeder (transaction içinde kullanım için)
        /// </summary>
        private static void SaveAreaSubAreaMapInternal(SQLiteConnection conn, Dictionary<string, List<string>> areaMap)
        {
            // Önce mevcut verileri temizle
            ExecuteNonQuery(conn, $"DELETE FROM {SUBAREAS_TABLE}");
            ExecuteNonQuery(conn, $"DELETE FROM {AREAS_TABLE}");

            // Yeni verileri ekle
            foreach (var area in areaMap)
            {
                // Ana alanı ekle
                string insertAreaQuery = $"INSERT INTO {AREAS_TABLE} (AreaName) VALUES (@AreaName)";
                ExecuteNonQuery(conn, insertAreaQuery, ("@AreaName", area.Key));

                // Alan ID'sini al
                string getAreaIdQuery = $"SELECT Id FROM {AREAS_TABLE} WHERE AreaName = @AreaName";
                int areaId = Convert.ToInt32(ExecuteScalar(conn, getAreaIdQuery, ("@AreaName", area.Key)));

                // Alt alanları ekle
                foreach (var subArea in area.Value)
                {
                    string insertSubAreaQuery = $@"
                INSERT INTO {SUBAREAS_TABLE} (AreaId, SubAreaName) 
                VALUES (@AreaId, @SubAreaName)";

                    ExecuteNonQuery(conn, insertSubAreaQuery,
                        ("@AreaId", areaId),
                        ("@SubAreaName", subArea));
                }
            }
        }

        /// <summary>
        /// Sorun haritasını internal olarak kaydeder (transaction içinde kullanım için)
        /// </summary>
        private static void SaveIssueMapInternal(SQLiteConnection conn, Dictionary<string, List<string>> issueMap)
        {
            // Önce mevcut verileri temizle
            ExecuteNonQuery(conn, $"DELETE FROM {ISSUES_TABLE}");

            // Yeni verileri ekle
            foreach (var area in issueMap)
            {
                foreach (var issue in area.Value)
                {
                    string insertQuery = $@"
                INSERT INTO {ISSUES_TABLE} (AreaName, IssueName) 
                VALUES (@AreaName, @IssueName)";

                    ExecuteNonQuery(conn, insertQuery,
                        ("@AreaName", area.Key),
                        ("@IssueName", issue));
                }
            }
        }

        #endregion

        #region Security Exception Class

        /// <summary>
        /// Güvenlik ihlali durumlarında fırlatılan özel exception
        /// </summary>
        public class SecurityException : Exception
        {
            public SecurityException(string message) : base(message) { }
            public SecurityException(string message, Exception innerException) : base(message, innerException) { }
        }

        #endregion
    }
}