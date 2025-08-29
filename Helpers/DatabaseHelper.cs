// Helpers/DatabaseHelper.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using TicketApp.Models;

namespace TicketApp.Helpers
{
    /// <summary>
    /// SQLite veritabanı yardımcı sınıfı.
    /// - Tabloları oluşturur/günceller
    /// - Ticket CRUD işlemleri
    /// - Ayarlar (Alan↔Alt Alan, Alt Alan↔Hat, Sorun listeleri)
    /// </summary>
    public static class DatabaseHelper
    {
        // --- Dosya ve bağlantı bilgileri ---
        private static readonly string dbFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TicketApp");
        private static readonly string dbPath = Path.Combine(dbFolder, "tickets.db");
        private static readonly string connectionString = $"Data Source={dbPath};Version=3;";

        // --- Tablo sabitleri ---
        private const string AREAS_TABLE = "Areas";
        private const string SUBAREAS_TABLE = "SubAreas";
        private const string ISSUES_TABLE = "Issues";
        private const string SUBAREA_LINES_TABLE = "SubAreaLines"; // Alt Alan -> Hat haritası
        private const string TICKETS_TABLE = "Tickets";

        // --- Genel kurulum ---
        public static void InitializeDatabase()
        {
            Directory.CreateDirectory(dbFolder);
            bool createNew = !File.Exists(dbPath);

            if (createNew)
                SQLiteConnection.CreateFile(dbPath);

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                CreateTables(conn);            // Tabloları oluştur
                UpdateExistingTables(conn);    // Eksik kolonları ekle (migration)
                if (createNew) LoadDefaultDataIfEmpty(conn); // Varsayılan veriler
            }
        }

        /// <summary>Gerekli tüm tabloları oluşturur.</summary>
        private static void CreateTables(SQLiteConnection conn)
        {
            // Areas
            string createAreasTable = $@"
                CREATE TABLE IF NOT EXISTS {AREAS_TABLE}(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AreaName TEXT NOT NULL UNIQUE
                );";

            // SubAreas (AreaId yabancı anahtar)
            string createSubAreasTable = $@"
                CREATE TABLE IF NOT EXISTS {SUBAREAS_TABLE}(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AreaId INTEGER NOT NULL,
                    SubAreaName TEXT NOT NULL,
                    UNIQUE(AreaId, SubAreaName)
                );";

            // Issues (Alan bazlı + GENEL)
            string createIssuesTable = $@"
                CREATE TABLE IF NOT EXISTS {ISSUES_TABLE}(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AreaName TEXT NOT NULL,
                    IssueName TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UNIQUE(AreaName, IssueName)
                );";

            // Alt Alan -> Hat eşlemesi
            string createSubAreaLinesTable = $@"
                CREATE TABLE IF NOT EXISTS {SUBAREA_LINES_TABLE}(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SubAreaName TEXT NOT NULL,
                    LineName TEXT NOT NULL,
                    UNIQUE(SubAreaName, LineName)
                );";

            // Tickets ana tablo (Admin/Main ile birebir uyumlu)  :contentReference[oaicite:6]{index=6}
            string createTicketsTable = $@"
                CREATE TABLE IF NOT EXISTS {TICKETS_TABLE}(
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
                    Status TEXT DEFAULT 'beklemede'
                        CHECK(Status IN ('beklemede','işlemde','çözüldü','reddedildi')),
                    AssignedTo TEXT,
                    RejectionReason TEXT,
                    UpdatedAt DATETIME,
                    ResolvedAt DATETIME
                );";

            // Performans index'leri
            string createIndexes = @"
                CREATE INDEX IF NOT EXISTS idx_tickets_status ON Tickets(Status);
                CREATE INDEX IF NOT EXISTS idx_tickets_area ON Tickets(Area);
                CREATE INDEX IF NOT EXISTS idx_tickets_created ON Tickets(CreatedAt);
                CREATE INDEX IF NOT EXISTS idx_tickets_assigned ON Tickets(AssignedTo);
            ";

            ExecuteNonQuery(conn, createAreasTable);
            ExecuteNonQuery(conn, createSubAreasTable);
            ExecuteNonQuery(conn, createIssuesTable);
            ExecuteNonQuery(conn, createSubAreaLinesTable);
            ExecuteNonQuery(conn, createTicketsTable);
            ExecuteNonQuery(conn, createIndexes);
        }

        /// <summary>Var olan tablolara eksik kolonları ekler (migration).</summary>
        private static void UpdateExistingTables(SQLiteConnection conn)
        {
            try
            {
                var existingColumns = GetTableColumns(conn, TICKETS_TABLE);

                var columnsToAdd = new Dictionary<string, string>
                {
                    ["UpdatedAt"] = $"ALTER TABLE {TICKETS_TABLE} ADD COLUMN UpdatedAt DATETIME;",
                    ["ResolvedAt"] = $"ALTER TABLE {TICKETS_TABLE} ADD COLUMN ResolvedAt DATETIME;"
                };

                foreach (var column in columnsToAdd)
                {
                    if (!existingColumns.Contains(column.Key, StringComparer.OrdinalIgnoreCase))
                        ExecuteNonQuery(conn, column.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Tablo güncelleme hatası: {ex.Message}");
            }
        }

        /// <summary>İlk kurulumda kataloglardan varsayılan verileri yükler.</summary>
        private static void LoadDefaultDataIfEmpty(SQLiteConnection conn)
        {
            // Areas boşsa katalogları bas
            int areaCount = Convert.ToInt32(ExecuteScalar(conn, $"SELECT COUNT(*) FROM {AREAS_TABLE}"));
            if (areaCount == 0)
            {
                var areaMap = AreaCatalog.GetAreaSubAreaMap();
                SaveAreaSubAreaMapInternal(conn, areaMap);

                var issueMap = IssueCatalog.GetIssueMap();
                SaveIssueMapInternal(conn, issueMap);
            }
        }

        // =========================================================
        // ================  TICKET OPERASYONLARI  =================
        // =========================================================

        /// <summary>Yeni ticket ekler.</summary>
        public static int InsertTicket(Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));
            ValidateTicket(ticket);

            string insertQuery = $@"
                INSERT INTO {TICKETS_TABLE}
                (Area, SubArea, Issue, Description, FirstName, LastName, PhoneNumber,
                 CreatedAt, IsResolved, Status, AssignedTo, RejectionReason)
                VALUES
                (@Area, @SubArea, @Issue, @Description, @FirstName, @LastName, @PhoneNumber,
                 @CreatedAt, @IsResolved, @Status, @AssignedTo, @RejectionReason);
                SELECT last_insert_rowid();";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Area", ticket.Area);
                    cmd.Parameters.AddWithValue("@SubArea", (object)ticket.SubArea ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                    cmd.Parameters.AddWithValue("@Description", ticket.Description);
                    cmd.Parameters.AddWithValue("@FirstName", ticket.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", ticket.LastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", ticket.PhoneNumber);
                    cmd.Parameters.AddWithValue("@CreatedAt", ticket.CreatedAt);
                    cmd.Parameters.AddWithValue("@IsResolved", ticket.IsResolved);
                    cmd.Parameters.AddWithValue("@Status", ticket.Status ?? "beklemede");
                    cmd.Parameters.AddWithValue("@AssignedTo", (object)ticket.AssignedTo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RejectionReason", (object)ticket.RejectionReason ?? DBNull.Value);

                    ticket.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    return ticket.Id;
                }
            }
        }

        /// <summary>Tüm ticket’ları getirir.</summary>
        public static List<Ticket> GetAllTickets()
        {
            string q = $@"
                SELECT Id, Area, SubArea, Issue, Description,
                       FirstName, LastName, PhoneNumber, CreatedAt,
                       IsResolved, Status, AssignedTo, RejectionReason,
                       UpdatedAt, ResolvedAt
                FROM {TICKETS_TABLE}
                ORDER BY CreatedAt DESC";

            return ExecuteTicketQuery(q);
        }

        /// <summary>Duruma göre ticket’lar.</summary>
        public static List<Ticket> GetTicketsByStatus(string status)
        {
            string q = $@"
                SELECT Id, Area, SubArea, Issue, Description,
                       FirstName, LastName, PhoneNumber, CreatedAt,
                       IsResolved, Status, AssignedTo, RejectionReason,
                       UpdatedAt, ResolvedAt
                FROM {TICKETS_TABLE}
                WHERE Status = @Status
                ORDER BY CreatedAt DESC";

            return ExecuteTicketQuery(q, ("@Status", status));
        }

        /// <summary>Kullanıcıya göre ticket’lar.</summary>
        public static List<Ticket> GetTicketsByUser(string firstName, string lastName, string phoneNumber)
        {
            string q = $@"
                SELECT Id, Area, SubArea, Issue, Description,
                       FirstName, LastName, PhoneNumber, CreatedAt,
                       IsResolved, Status, AssignedTo, RejectionReason,
                       UpdatedAt, ResolvedAt
                FROM {TICKETS_TABLE}
                WHERE FirstName = @FirstName AND LastName = @LastName AND PhoneNumber = @PhoneNumber
                ORDER BY CreatedAt DESC";

            return ExecuteTicketQuery(q,
                ("@FirstName", firstName),
                ("@LastName", lastName),
                ("@PhoneNumber", phoneNumber));
        }

        /// <summary>
        /// Ticket durumunu/atananı güvenli şekilde günceller (AdminForm çağrısı ile birebir).
        /// - Status, AssignedTo, IsResolved, RejectionReason, UpdatedAt, ResolvedAt alanlarını set eder.
        /// </summary>
        public static bool UpdateTicketStatus(Ticket ticket)
        {
            if (ticket == null || ticket.Id <= 0) return false;

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
                    return affected > 0;
                }
            }
        }
        // (AdminForm bu metodu böyle çağırıyor: `DatabaseHelper.UpdateTicketStatus(ticket)` — çağrı uyumu kanıtı) 

        /// <summary>Ticket sil.</summary>
        public static bool DeleteTicket(int ticketId)
        {
            if (ticketId <= 0) return false;
            string q = $"DELETE FROM {TICKETS_TABLE} WHERE Id = @Id";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(q, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", ticketId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // =========================================================
        // ==============  ALAN / ALT ALAN / HAT / SORUN  =========
        // =========================================================

        /// <summary>Alan↔Alt Alan haritasını kaydeder.</summary>
        public static void SaveAreaSubAreaMap(Dictionary<string, List<string>> areaMap)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    SaveAreaSubAreaMapInternal(conn, areaMap);
                    tr.Commit();
                }
            }
        }

        private static void SaveAreaSubAreaMapInternal(SQLiteConnection conn, Dictionary<string, List<string>> areaMap)
        {
            // Tabloları boşalt ve yeniden yaz
            ExecuteNonQuery(conn, $"DELETE FROM {SUBAREAS_TABLE}");
            ExecuteNonQuery(conn, $"DELETE FROM {AREAS_TABLE}");

            // Alanları insert et
            var areaIdCache = new Dictionary<string, long>();
            foreach (var kv in areaMap)
            {
                ExecuteNonQuery(conn, $"INSERT OR IGNORE INTO {AREAS_TABLE}(AreaName) VALUES (@name)",
                    ("@name", kv.Key));

                long areaId = Convert.ToInt64(
                    ExecuteScalar(conn, $"SELECT Id FROM {AREAS_TABLE} WHERE AreaName=@n", ("@n", kv.Key)));
                areaIdCache[kv.Key] = areaId;

                // Alt alanları insert et
                foreach (var sub in kv.Value.Distinct())
                {
                    ExecuteNonQuery(conn,
                        $"INSERT OR IGNORE INTO {SUBAREAS_TABLE}(AreaId, SubAreaName) VALUES (@a,@s)",
                        ("@a", areaId), ("@s", sub));
                }
            }
        }

        /// <summary>Alan↔Alt Alan haritasını okur.</summary>
        public static Dictionary<string, List<string>> GetAreaSubAreaMap()
        {
            var map = new Dictionary<string, List<string>>();

            string q = $@"
                SELECT a.AreaName, s.SubAreaName
                FROM {AREAS_TABLE} a
                LEFT JOIN {SUBAREAS_TABLE} s ON a.Id = s.AreaId
                ORDER BY a.AreaName, s.SubAreaName";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(q, conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var area = r["AreaName"].ToString();
                        var sub = r["SubAreaName"]?.ToString();

                        if (!map.ContainsKey(area)) map[area] = new List<string>();
                        if (!string.IsNullOrEmpty(sub)) map[area].Add(sub);
                    }
                }
            }

            return map;
        }

        /// <summary>Alt Alan ↔ Hat haritasını kaydeder.</summary>
        public static void SaveSubAreaLineMap(Dictionary<string, List<string>> subAreaLineMap)
        {
            if (subAreaLineMap == null) return;

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    // Basit yaklaşım: tamamını sil-yaz
                    ExecuteNonQuery(conn, $"DELETE FROM {SUBAREA_LINES_TABLE}");

                    foreach (var kv in subAreaLineMap)
                    {
                        var sub = kv.Key;
                        foreach (var line in (kv.Value ?? new List<string>()).Distinct())
                        {
                            ExecuteNonQuery(conn,
                                $"INSERT OR IGNORE INTO {SUBAREA_LINES_TABLE}(SubAreaName, LineName) VALUES (@s,@l)",
                                ("@s", sub), ("@l", line));
                        }
                    }

                    tr.Commit();
                }
            }
        }

        /// <summary>Alt Alan ↔ Hat haritasını okur.</summary>
        public static Dictionary<string, List<string>> GetSubAreaLineMap()
        {
            var map = new Dictionary<string, List<string>>();
            string q = $@"SELECT SubAreaName, LineName FROM {SUBAREA_LINES_TABLE} ORDER BY SubAreaName, LineName";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(q, conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string sub = r["SubAreaName"].ToString();
                        string line = r["LineName"].ToString();

                        if (!map.ContainsKey(sub)) map[sub] = new List<string>();
                        map[sub].Add(line);
                    }
                }
            }
            return map;
        }

        /// <summary>Sorun listesini kaydeder.</summary>
        public static void SaveIssueMap(Dictionary<string, List<string>> issueMap)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    ExecuteNonQuery(conn, $"DELETE FROM {ISSUES_TABLE}");
                    foreach (var kv in issueMap)
                    {
                        foreach (var issue in (kv.Value ?? new List<string>()).Distinct())
                        {
                            ExecuteNonQuery(conn,
                                $"INSERT OR IGNORE INTO {ISSUES_TABLE}(AreaName, IssueName) VALUES (@a,@i)",
                                ("@a", kv.Key), ("@i", issue));
                        }
                    }
                    tr.Commit();
                }
            }
        }

        /// <summary>Sorun listesini okur.</summary>
        public static Dictionary<string, List<string>> GetIssueMap()
        {
            var map = new Dictionary<string, List<string>>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(
                    $"SELECT AreaName, IssueName FROM {ISSUES_TABLE} ORDER BY AreaName, IssueName", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var area = r["AreaName"].ToString();
                        var issue = r["IssueName"].ToString();
                        if (!map.ContainsKey(area)) map[area] = new List<string>();
                        map[area].Add(issue);
                    }
                }
            }
            return map;
        }

        // === Silme kısıtları için yardımcı kontroller ===
        public static bool HasTicketsForArea(string area)
            => Convert.ToInt32(ExecuteScalar(new SQLiteConnection(connectionString),
                $"SELECT COUNT(*) FROM {TICKETS_TABLE} WHERE Area=@a", ("@a", area))) > 0;

        public static bool HasTicketsForSubArea(string area, string subArea)
            => Convert.ToInt32(ExecuteScalar(new SQLiteConnection(connectionString),
                $"SELECT COUNT(*) FROM {TICKETS_TABLE} WHERE Area=@a AND SubArea=@s",
                ("@a", area), ("@s", subArea))) > 0;

        public static bool HasTicketsForIssue(string issue)
            => Convert.ToInt32(ExecuteScalar(new SQLiteConnection(connectionString),
                $"SELECT COUNT(*) FROM {TICKETS_TABLE} WHERE Issue=@i", ("@i", issue))) > 0;

        // =========================================================
        // ====================  ARŞİV / TEMİZLİK  =================
        // =========================================================

        /// <summary>Durumu 'çözüldü' olan ticket’ları siler.</summary>
        public static void DeleteResolvedTickets()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(
                    $"DELETE FROM {TICKETS_TABLE} WHERE Status=@s", conn))
                {
                    cmd.Parameters.AddWithValue("@s", "çözüldü");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =========================================================
        // =====================  ORTAK YARDIMCI  ==================
        // =========================================================

        /// <summary>Ticket doğrulama.</summary>
        private static void ValidateTicket(Ticket ticket)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(ticket.Area)) errors.Add("Alan boş olamaz");
            if (string.IsNullOrWhiteSpace(ticket.Issue)) errors.Add("Sorun tipi boş olamaz");
            if (string.IsNullOrWhiteSpace(ticket.Description)) errors.Add("Açıklama boş olamaz");
            if ((ticket.Description?.Length ?? 0) > 300) errors.Add("Açıklama 300+ karakter");
            if (string.IsNullOrWhiteSpace(ticket.FirstName)) errors.Add("Ad boş olamaz");
            if (string.IsNullOrWhiteSpace(ticket.LastName)) errors.Add("Soyad boş olamaz");
            if (string.IsNullOrWhiteSpace(ticket.PhoneNumber)) errors.Add("Telefon boş olamaz");

            if (errors.Count > 0)
                throw new ArgumentException("Ticket verileri geçersiz: " + string.Join(", ", errors));
        }

        /// <summary>SELECT sorgu çalıştırır ve Ticket listesine map’ler.</summary>
        private static List<Ticket> ExecuteTicketQuery(string query, params (string name, object value)[] parameters)
        {
            var result = new List<Ticket>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    foreach (var p in parameters) cmd.Parameters.AddWithValue(p.name, p.value);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            result.Add(new Ticket
                            {
                                Id = Convert.ToInt32(r["Id"]),
                                Area = r["Area"].ToString(),
                                SubArea = r["SubArea"] == DBNull.Value ? "" : r["SubArea"].ToString(),
                                Issue = r["Issue"].ToString(),
                                Description = r["Description"].ToString(),
                                FirstName = r["FirstName"].ToString(),
                                LastName = r["LastName"].ToString(),
                                PhoneNumber = r["PhoneNumber"].ToString(),
                                CreatedAt = Convert.ToDateTime(r["CreatedAt"]),
                                IsResolved = Convert.ToBoolean(r["IsResolved"]),
                                Status = r["Status"]?.ToString() ?? "beklemede",
                                AssignedTo = r["AssignedTo"] == DBNull.Value ? null : r["AssignedTo"].ToString(),
                                RejectionReason = r["RejectionReason"] == DBNull.Value ? null : r["RejectionReason"].ToString(),
                                UpdatedAt = r["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["UpdatedAt"]),
                                ResolvedAt = r["ResolvedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["ResolvedAt"])
                            });
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>Tek değer döndüren sorgu çalıştırır.</summary>
        private static object ExecuteScalar(SQLiteConnection externalConn, string query, params (string name, object value)[] parameters)
        {
            // Bağlantı dışarıdan verilmediyse kendimiz açar kaparız
            bool own = externalConn.State != System.Data.ConnectionState.Open;
            if (own) externalConn.Open();

            try
            {
                using (var cmd = new SQLiteCommand(query, externalConn))
                {
                    foreach (var p in parameters) cmd.Parameters.AddWithValue(p.name, p.value);
                    return cmd.ExecuteScalar();
                }
            }
            finally
            {
                if (own) externalConn.Close();
            }
        }

        /// <summary>INSERT/UPDATE/DELETE için yardımcı.</summary>
        private static int ExecuteNonQuery(SQLiteConnection conn, string query, params (string name, object value)[] parameters)
        {
            using (var cmd = new SQLiteCommand(query, conn))
            {
                foreach (var p in parameters) cmd.Parameters.AddWithValue(p.name, p.value);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Bir tablonun kolon adlarını döndürür.</summary>
        private static List<string> GetTableColumns(SQLiteConnection conn, string table)
        {
            var cols = new List<string>();
            using (var cmd = new SQLiteCommand($"PRAGMA table_info({table});", conn))
            using (var r = cmd.ExecuteReader())
                while (r.Read()) cols.Add(r["name"].ToString());
            return cols;
        }
    }
}
