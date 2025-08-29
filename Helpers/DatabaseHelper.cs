// Helpers/DatabaseHelper.cs
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using TicketApp.Models;

namespace TicketApp.Helpers
{
    /// <summary>
    /// SQLite veritabanı işlemlerini yöneten güvenli yardımcı sınıf.
    /// - Tüm sorgular parametreli yazılır (SQL Injection koruması).
    /// - Uygulama ilk açılışta InitializeDatabase() çağrılır (Program.cs)  // ref: Program.cs
    /// - Varsayılan veri yükleme: AreaCatalog + IssueCatalog               // seed fallback
    /// </summary>
    public static class DatabaseHelper
    {
        #region Fields & Consts

        // --- Tablo isimleri ---
        private const string TICKETS_TABLE = "Tickets";
        private const string AREAS_TABLE = "Areas";
        private const string SUBAREAS_TABLE = "SubAreas";
        private const string LINES_TABLE = "Lines";     // YENİ: Hat tablosu
        private const string ISSUES_TABLE = "Issues";

        // --- Konfig / Yol bilgileri ---
        // Not: App.config'ten okuma için AppConfigReader da kullanılabilir.  // ref: AppConfigReader
        private static readonly string dbPath =
            ConfigurationManager.AppSettings["DatabaseFilePath"] ??
            @"C:\TicketAppShared\tickets.db";

        // Çok kullanıcılı senaryolara uygun connection string (WAL vb.)
        private static readonly string connectionString =
            $"Data Source={dbPath};Version=3;Journal Mode=WAL;Cache Size=10000;Temp Store=Memory;Synchronous=NORMAL;Busy Timeout=10000;Default Timeout=30;";

        #endregion

        #region Initialize & Schema

        /// <summary>
        /// Veritabanını ve tabloları hazırlar; yoksa oluşturur; seed verileri yükler.
        /// Program başlangıcında bir kez çağrılır. (Program.cs)  // ref: Program.cs
        /// </summary>
        public static void InitializeDatabase()
        {
            try
            {
                var folder = Path.GetDirectoryName(dbPath);
                if (string.IsNullOrWhiteSpace(folder))
                    throw new Exception($"Veritabanı yolu geçersiz: {dbPath}");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    Logger.Log($"Veritabanı klasörü oluşturuldu: {folder}");
                }

                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                    Logger.Log("Veritabanı dosyası oluşturuldu.");
                }

                // Erişim kontrolü
                using (var fs = File.Open(dbPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { }

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        try
                        {
                            CreateTables(conn);           // tüm tablolar (Lines dahil)
                            UpdateExistingTables(conn);   // eksik kolonları ekle (Line vb.)
                            LoadDefaultDataIfEmpty(conn); // seed: AreaCatalog + IssueCatalog
                            tx.Commit();
                            Logger.Log("Veritabanı başarıyla başlatıldı.");
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            Logger.Log($"Initialize transaction geri alındı: {ex.Message}");
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
        /// Tüm tabloları oluşturur (idempotent). Yeni: Lines (Hat) tablosu ve Tickets.Line kolonu.
        /// </summary>
        private static void CreateTables(SQLiteConnection conn)
        {
            // Ana alanlar
            string createAreas = $@"
            CREATE TABLE IF NOT EXISTS {AREAS_TABLE}(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AreaName TEXT NOT NULL UNIQUE,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            // Alt alanlar
            string createSubAreas = $@"
            CREATE TABLE IF NOT EXISTS {SUBAREAS_TABLE}(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AreaId INTEGER NOT NULL,
                SubAreaName TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY(AreaId) REFERENCES {AREAS_TABLE}(Id) ON DELETE CASCADE,
                UNIQUE(AreaId, SubAreaName)
            );";

            // YENİ: Hatlar
            string createLines = $@"
            CREATE TABLE IF NOT EXISTS {LINES_TABLE}(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SubAreaId INTEGER NOT NULL,
                LineName TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY(SubAreaId) REFERENCES {SUBAREAS_TABLE}(Id) ON DELETE CASCADE,
                UNIQUE(SubAreaId, LineName)
            );";

            // Sorun tipleri (alan bazlı)
            string createIssues = $@"
            CREATE TABLE IF NOT EXISTS {ISSUES_TABLE}(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AreaName TEXT NOT NULL,
                IssueName TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                UNIQUE(AreaName, IssueName)
            );";

            // Ticketlar
            // Not: Yeni kurulumlarda Line kolonu doğrudan burada var.
            string createTickets = $@"
            CREATE TABLE IF NOT EXISTS {TICKETS_TABLE}(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Area TEXT NOT NULL,
                SubArea TEXT,
                Line TEXT,                      -- YENİ: Hat
                Issue TEXT NOT NULL,
                Description TEXT NOT NULL,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                PhoneNumber TEXT NOT NULL,
                CreatedAt DATETIME NOT NULL,
                IsResolved INTEGER DEFAULT 0,
                Status TEXT DEFAULT 'beklemede' CHECK(Status IN ('beklemede','işlemde','çözüldü','reddedildi')),
                AssignedTo TEXT,
                RejectionReason TEXT,
                UpdatedAt DATETIME,
                ResolvedAt DATETIME
            );";

            // Indexler
            string createIndexes = $@"
            CREATE INDEX IF NOT EXISTS idx_tickets_status   ON {TICKETS_TABLE}(Status);
            CREATE INDEX IF NOT EXISTS idx_tickets_area     ON {TICKETS_TABLE}(Area);
            CREATE INDEX IF NOT EXISTS idx_tickets_created  ON {TICKETS_TABLE}(CreatedAt);
            CREATE INDEX IF NOT EXISTS idx_tickets_assigned ON {TICKETS_TABLE}(AssignedTo);
            CREATE INDEX IF NOT EXISTS idx_lines_subarea    ON {LINES_TABLE}(SubAreaId);
            ";

            ExecuteNonQuery(conn, createAreas);
            ExecuteNonQuery(conn, createSubAreas);
            ExecuteNonQuery(conn, createLines);
            ExecuteNonQuery(conn, createIssues);
            ExecuteNonQuery(conn, createTickets);
            ExecuteNonQuery(conn, createIndexes);
        }

        /// <summary>
        /// Mevcut kurulumlarda eksik kolon/tabloları güvenle ekler (non-breaking migration).
        /// </summary>
        private static void UpdateExistingTables(SQLiteConnection conn)
        {
            try
            {
                // 1) Tickets tabloda eksik kolon var mı?
                var ticketCols = GetTableColumns(conn, TICKETS_TABLE)
                                 ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                var addColumns = new Dictionary<string, string>
                {
                    // Eski sürümlerde bulunmayabilir:
                    ["UpdatedAt"] = $"ALTER TABLE {TICKETS_TABLE} ADD COLUMN UpdatedAt DATETIME;",
                    ["ResolvedAt"] = $"ALTER TABLE {TICKETS_TABLE} ADD COLUMN ResolvedAt DATETIME;",
                    ["Line"] = $"ALTER TABLE {TICKETS_TABLE} ADD COLUMN Line TEXT;" // YENİ
                };

                foreach (var kv in addColumns)
                {
                    if (!ticketCols.Contains(kv.Key))
                    {
                        ExecuteNonQuery(conn, kv.Value);
                        Logger.Log($"'{kv.Key}' kolonu {TICKETS_TABLE} tablosuna eklendi.");
                    }
                }

                // 2) Lines tablosu yoksa oluştur (CreateTables zaten idempotent ama garanti için)
                ExecuteNonQuery(conn, $@"
                    CREATE TABLE IF NOT EXISTS {LINES_TABLE}(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SubAreaId INTEGER NOT NULL,
                        LineName TEXT NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY(SubAreaId) REFERENCES {SUBAREAS_TABLE}(Id) ON DELETE CASCADE,
                        UNIQUE(SubAreaId, LineName)
                    );");
            }
            catch (Exception ex)
            {
                Logger.Log($"Tablo güncelleme hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// İlk kurulumda alan/alt alan ve sorun tiplerini seed eder.
        /// </summary>
        private static void LoadDefaultDataIfEmpty(SQLiteConnection conn)
        {
            var count = Convert.ToInt32(ExecuteScalar(conn, $"SELECT COUNT(*) FROM {AREAS_TABLE}"));
            if (count > 0) return;

            Logger.Log("Varsayılan veriler yükleniyor...");

            // Alan & Alt Alan seed
            var areaMap = AreaCatalog.GetAreaSubAreaMap();     // ref: AreaCatalog
            SaveAreaSubAreaMapInternal(conn, areaMap);

            // (İsteğe bağlı) Hat seed — şu an boş bırakıyoruz; SettingsForm üzerinden eklenecek.

            // Sorun tipi seed
            var issueMap = IssueCatalog.GetIssueMap();         // ref: IssueCatalog
            SaveIssueMapInternal(conn, issueMap);

            Logger.Log("Varsayılan veriler yüklendi.");
        }

        #endregion

        #region Ticket CRUD

        /// <summary>
        /// Yeni ticket ekler. (Alan, Alt Alan, Hat (Line), Sorun, Kişi bilgileri…)
        /// </summary>
        public static int InsertTicket(Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));
            ValidateTicket(ticket);

            string insert = $@"
            INSERT INTO {TICKETS_TABLE}(
                Area, SubArea, Line, Issue, Description,
                FirstName, LastName, PhoneNumber,
                CreatedAt, Status, IsResolved,
                AssignedTo, RejectionReason
            ) VALUES (
                @Area, @SubArea, @Line, @Issue, @Description,
                @FirstName, @LastName, @PhoneNumber,
                @CreatedAt, @Status, @IsResolved,
                @AssignedTo, @RejectionReason
            );
            SELECT last_insert_rowid();";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(insert, conn))
                {
                    cmd.Parameters.AddWithValue("@Area", ticket.Area);
                    cmd.Parameters.AddWithValue("@SubArea", (object)ticket.SubArea ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Line", (object)ticket.Line ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                    cmd.Parameters.AddWithValue("@Description", ticket.Description);
                    cmd.Parameters.AddWithValue("@FirstName", ticket.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", ticket.LastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", ticket.PhoneNumber);
                    cmd.Parameters.AddWithValue("@CreatedAt", ticket.CreatedAt);
                    cmd.Parameters.AddWithValue("@Status", ticket.Status ?? "beklemede");
                    cmd.Parameters.AddWithValue("@IsResolved", ticket.IsResolved ? 1 : 0);
                    cmd.Parameters.AddWithValue("@AssignedTo", (object)ticket.AssignedTo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RejectionReason", (object)ticket.RejectionReason ?? DBNull.Value);

                    ticket.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    Logger.Log($"Yeni ticket eklendi: #{ticket.Id}");
                    return ticket.Id;
                }
            }
        }

        /// <summary>Tüm ticket'ları (en yeni üstte) döndürür.</summary>
        public static List<Ticket> GetAllTickets()
        {
            var list = new List<Ticket>();
            string sql = $@"SELECT * FROM {TICKETS_TABLE} ORDER BY CreatedAt DESC, Id DESC;";

            using (var conn = new SQLiteConnection(connectionString))
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read()) list.Add(MapReaderToTicket(r));
                }
            }
            return list;
        }

        /// <summary>Çözülen ticket'ları getirir.</summary>
        public static List<Ticket> GetResolvedTickets()
        {
            var list = new List<Ticket>();
            string sql = $@"SELECT * FROM {TICKETS_TABLE} WHERE IsResolved=1 OR Status='çözüldü' ORDER BY ResolvedAt DESC, UpdatedAt DESC;";

            using (var conn = new SQLiteConnection(connectionString))
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read()) list.Add(MapReaderToTicket(r));
                }
            }
            return list;
        }

        /// <summary>Çözülen ticket'ları arşiv dosyasına yazar (yeni SQLite dosyası) ve kaydeder.</summary>
        public static void ArchiveTickets(List<Ticket> tickets, string archiveFileName)
        {
            if (tickets == null || tickets.Count == 0) return;

            string archivePath = Path.Combine(Path.GetDirectoryName(dbPath) ?? "", archiveFileName);
            if (!File.Exists(archivePath)) SQLiteConnection.CreateFile(archivePath);

            string cs = $"Data Source={archivePath};Version=3;Journal Mode=WAL;Synchronous=NORMAL;";
            using (var conn = new SQLiteConnection(cs))
            {
                conn.Open();

                // Arşivde tabloyu oluştur
                string create = @"
                CREATE TABLE IF NOT EXISTS TicketsArchive(
                    Id INTEGER,
                    Area TEXT, SubArea TEXT, Line TEXT,
                    Issue TEXT, Description TEXT,
                    FirstName TEXT, LastName TEXT, PhoneNumber TEXT,
                    CreatedAt DATETIME, IsResolved INTEGER, Status TEXT,
                    AssignedTo TEXT, RejectionReason TEXT, UpdatedAt DATETIME, ResolvedAt DATETIME
                );";
                ExecuteNonQuery(conn, create);

                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        string ins = @"
                        INSERT INTO TicketsArchive
                        (Id, Area, SubArea, Line, Issue, Description, FirstName, LastName, PhoneNumber, CreatedAt, IsResolved, Status, AssignedTo, RejectionReason, UpdatedAt, ResolvedAt)
                        VALUES
                        (@Id,@Area,@SubArea,@Line,@Issue,@Description,@FirstName,@LastName,@PhoneNumber,@CreatedAt,@IsResolved,@Status,@AssignedTo,@RejectionReason,@UpdatedAt,@ResolvedAt);";

                        foreach (var t in tickets)
                        {
                            using (var cmd = new SQLiteCommand(ins, conn))
                            {
                                cmd.Parameters.AddWithValue("@Id", t.Id);
                                cmd.Parameters.AddWithValue("@Area", t.Area);
                                cmd.Parameters.AddWithValue("@SubArea", (object)t.SubArea ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Line", (object)t.Line ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Issue", t.Issue);
                                cmd.Parameters.AddWithValue("@Description", t.Description);
                                cmd.Parameters.AddWithValue("@FirstName", t.FirstName);
                                cmd.Parameters.AddWithValue("@LastName", t.LastName);
                                cmd.Parameters.AddWithValue("@PhoneNumber", t.PhoneNumber);
                                cmd.Parameters.AddWithValue("@CreatedAt", t.CreatedAt);
                                cmd.Parameters.AddWithValue("@IsResolved", t.IsResolved ? 1 : 0);
                                cmd.Parameters.AddWithValue("@Status", t.Status);
                                cmd.Parameters.AddWithValue("@AssignedTo", (object)t.AssignedTo ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@RejectionReason", (object)t.RejectionReason ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdatedAt", (object)t.UpdatedAt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ResolvedAt", (object)t.ResolvedAt ?? DBNull.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Logger.Log(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>Çözülen ticket'ları ana veritabanından siler.</summary>
        public static void DeleteResolvedTickets()
        {
            ExecuteNonQuery(connectionString, $@"DELETE FROM {TICKETS_TABLE} WHERE IsResolved=1 OR Status='çözüldü';");
        }

        /// <summary>DataReader satırını Ticket nesnesine map eder. (Line dahil)</summary>
        private static Ticket MapReaderToTicket(SQLiteDataReader r)
        {
            return new Ticket
            {
                Id = Convert.ToInt32(r["Id"]),
                Area = Convert.ToString(r["Area"]),
                SubArea = r["SubArea"] == DBNull.Value ? null : Convert.ToString(r["SubArea"]),
                Line = r["Line"] == DBNull.Value ? null : Convert.ToString(r["Line"]), // YENİ
                Issue = Convert.ToString(r["Issue"]),
                Description = Convert.ToString(r["Description"]),
                FirstName = Convert.ToString(r["FirstName"]),
                LastName = Convert.ToString(r["LastName"]),
                PhoneNumber = Convert.ToString(r["PhoneNumber"]),
                CreatedAt = Convert.ToDateTime(r["CreatedAt"]),
                IsResolved = Convert.ToInt32(r["IsResolved"]) == 1,
                Status = Convert.ToString(r["Status"]),
                AssignedTo = r["AssignedTo"] == DBNull.Value ? null : Convert.ToString(r["AssignedTo"]),
                RejectionReason = r["RejectionReason"] == DBNull.Value ? null : Convert.ToString(r["RejectionReason"]),
                UpdatedAt = r["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["UpdatedAt"]),
                ResolvedAt = r["ResolvedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["ResolvedAt"])
            };
        }

        /// <summary>Temel doğrulama: zorunlu alanlar ve uzunluklar.</summary>
        private static void ValidateTicket(Ticket t)
        {
            if (string.IsNullOrWhiteSpace(t.Area)) throw new ArgumentException("Alan boş olamaz.");
            if (string.IsNullOrWhiteSpace(t.SubArea)) throw new ArgumentException("Alt Alan boş olamaz.");
            if (string.IsNullOrWhiteSpace(t.Line)) throw new ArgumentException("Hat (Line) boş olamaz."); // YENİ
            if (string.IsNullOrWhiteSpace(t.Issue)) throw new ArgumentException("Sorun tipi boş olamaz.");
            if (string.IsNullOrWhiteSpace(t.Description)) throw new ArgumentException("Açıklama boş olamaz.");
            if (string.IsNullOrWhiteSpace(t.FirstName)) throw new ArgumentException("Ad boş olamaz.");
            if (string.IsNullOrWhiteSpace(t.LastName)) throw new ArgumentException("Soyad boş olamaz.");
            if (string.IsNullOrWhiteSpace(t.PhoneNumber)) throw new ArgumentException("Telefon boş olamaz.");
            if (t.Description.Length > 300) throw new ArgumentException("Açıklama en fazla 300 karakter olmalıdır.");
        }

        #endregion

        #region Area/SubArea (Mevcut)

        /// <summary>DB'den Alan→Alt Alan haritasını döndürür.</summary>
        public static Dictionary<string, List<string>> GetAreaSubAreaMap()
        {
            var map = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            string sqlAreas = $@"SELECT Id, AreaName FROM {AREAS_TABLE} ORDER BY AreaName;";
            string sqlSubs = $@"SELECT s.Id, a.AreaName, s.SubAreaName
                                 FROM {SUBAREAS_TABLE} s
                                 JOIN {AREAS_TABLE} a ON a.Id = s.AreaId
                                 ORDER BY a.AreaName, s.SubAreaName;";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Alanlar
                var areaIdToName = new Dictionary<long, string>();
                using (var cmd = new SQLiteCommand(sqlAreas, conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var name = Convert.ToString(r["AreaName"]);
                        map[name] = new List<string>();
                    }
                }

                // Alt alanlar
                using (var cmd = new SQLiteCommand(sqlSubs, conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string areaName = Convert.ToString(r["AreaName"]);
                        string subName = Convert.ToString(r["SubAreaName"]);
                        if (!map.ContainsKey(areaName))
                            map[areaName] = new List<string>();
                        map[areaName].Add(subName);
                    }
                }
            }

            return map;
        }

        /// <summary>Area/SubArea haritasını tek transaction’da kaydeder (seed veya SettingsForm kaydet).</summary>
        public static void SaveAreaSubAreaMap(Dictionary<string, List<string>> map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        SaveAreaSubAreaMapInternal(conn, map);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Logger.Log(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>İç kaydetme (transaction açıkken çağrılır).</summary>
        private static void SaveAreaSubAreaMapInternal(SQLiteConnection conn, Dictionary<string, List<string>> map)
        {
            // Temizle
            ExecuteNonQuery(conn, $"DELETE FROM {SUBAREAS_TABLE};");
            ExecuteNonQuery(conn, $"DELETE FROM {AREAS_TABLE};");

            // Alanları ekle
            string insertArea = $@"INSERT INTO {AREAS_TABLE}(AreaName) VALUES(@AreaName); SELECT last_insert_rowid();";
            string insertSub = $@"INSERT INTO {SUBAREAS_TABLE}(AreaId, SubAreaName) VALUES(@AreaId,@SubAreaName);";

            foreach (var area in map.Keys)
            {
                long areaId;
                using (var cmd = new SQLiteCommand(insertArea, conn))
                {
                    cmd.Parameters.AddWithValue("@AreaName", area);
                    areaId = (long)(long)Convert.ToInt64(cmd.ExecuteScalar());
                }

                foreach (var sub in map[area] ?? Enumerable.Empty<string>())
                {
                    using (var cmd = new SQLiteCommand(insertSub, conn))
                    {
                        cmd.Parameters.AddWithValue("@AreaId", areaId);
                        cmd.Parameters.AddWithValue("@SubAreaName", sub);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion

        #region Lines (Hat) — YENİ

        /// <summary>Alt Alan → Hat listesini döndürür.</summary>
        public static Dictionary<string, List<string>> GetSubAreaLineMap()
        {
            var map = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            string sql = $@"
            SELECT sa.SubAreaName, l.LineName
            FROM {LINES_TABLE} l
            JOIN {SUBAREAS_TABLE} sa ON sa.Id = l.SubAreaId
            ORDER BY sa.SubAreaName, l.LineName;";

            using (var conn = new SQLiteConnection(connectionString))
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string sub = Convert.ToString(r["SubAreaName"]);
                        string line = Convert.ToString(r["LineName"]);
                        if (!map.ContainsKey(sub)) map[sub] = new List<string>();
                        map[sub].Add(line);
                    }
                }
            }

            return map;
        }

        /// <summary>Tüm Lines haritasını (Alt Alan → Hatlar) tek transaction’da kaydeder.</summary>
        public static void SaveSubAreaLineMap(Dictionary<string, List<string>> subAreaLineMap)
        {
            if (subAreaLineMap == null) throw new ArgumentNullException(nameof(subAreaLineMap));

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        // Tüm hatları sıfırla ve yeniden yaz.
                        ExecuteNonQuery(conn, $"DELETE FROM {LINES_TABLE};");

                        string findSub = $@"SELECT Id FROM {SUBAREAS_TABLE} WHERE SubAreaName=@Sub;";
                        string insert = $@"INSERT INTO {LINES_TABLE}(SubAreaId, LineName) VALUES(@SubAreaId,@LineName);";

                        foreach (var kv in subAreaLineMap)
                        {
                            long? subId = null;
                            using (var find = new SQLiteCommand(findSub, conn))
                            {
                                find.Parameters.AddWithValue("@Sub", kv.Key);
                                var obj = find.ExecuteScalar();
                                if (obj != null && obj != DBNull.Value)
                                    subId = Convert.ToInt64(obj);
                            }

                            if (subId == null) continue; // Alt Alan yoksa atla

                            foreach (var line in kv.Value?.Distinct(StringComparer.OrdinalIgnoreCase) ?? Enumerable.Empty<string>())
                            {
                                using (var cmd = new SQLiteCommand(insert, conn))
                                {
                                    cmd.Parameters.AddWithValue("@SubAreaId", subId.Value);
                                    cmd.Parameters.AddWithValue("@LineName", line);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Logger.Log(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>Tek bir hat ekler (Alt Alan adına göre).</summary>
        public static void AddLine(string subAreaName, string lineName)
        {
            if (string.IsNullOrWhiteSpace(subAreaName)) throw new ArgumentException("Alt Alan boş olamaz.", nameof(subAreaName));
            if (string.IsNullOrWhiteSpace(lineName)) throw new ArgumentException("Hat adı boş olamaz.", nameof(lineName));

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // SubAreaId bul
                long? subId = null;
                using (var find = new SQLiteCommand($@"SELECT Id FROM {SUBAREAS_TABLE} WHERE SubAreaName=@s;", conn))
                {
                    find.Parameters.AddWithValue("@s", subAreaName);
                    var obj = find.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value) subId = Convert.ToInt64(obj);
                }
                if (subId == null) throw new InvalidOperationException("Alt Alan bulunamadı.");

                // Ekle
                using (var cmd = new SQLiteCommand($@"INSERT OR IGNORE INTO {LINES_TABLE}(SubAreaId, LineName) VALUES(@id,@name);", conn))
                {
                    cmd.Parameters.AddWithValue("@id", subId.Value);
                    cmd.Parameters.AddWithValue("@name", lineName);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Hattı siler (o hatta ait ticket yoksa). True: silindi, False: engellendi.</summary>
        public static bool DeleteLine(string subAreaName, string lineName)
        {
            if (HasTicketsForLine(lineName)) return false; // bağlı kayıtlar var

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = $@"
                DELETE FROM {LINES_TABLE}
                WHERE LineName=@line
                  AND SubAreaId IN (SELECT Id FROM {SUBAREAS_TABLE} WHERE SubAreaName=@sub);";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@line", lineName);
                    cmd.Parameters.AddWithValue("@sub", subAreaName);
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        /// <summary>Belirli bir hattı kullanan ticket var mı?</summary>
        public static bool HasTicketsForLine(string lineName)
        {
            if (string.IsNullOrWhiteSpace(lineName)) return false;
            using (var conn = new SQLiteConnection(connectionString))
            using (var cmd = new SQLiteCommand($@"SELECT COUNT(*) FROM {TICKETS_TABLE} WHERE Line=@line;", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@line", lineName);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        #endregion

        #region Issues (Mevcut)

        /// <summary>Alan → Sorun listesi haritasını döndürür.</summary>
        public static Dictionary<string, List<string>> GetIssueMap()
        {
            var map = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // DB’de tanım yoksa (ilk açılış) fallback IssueCatalog kullanılabilir.
                string sql = $@"SELECT AreaName, IssueName FROM {ISSUES_TABLE} ORDER BY AreaName, IssueName;";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string area = Convert.ToString(r["AreaName"]);
                        string issue = Convert.ToString(r["IssueName"]);
                        if (!map.ContainsKey(area)) map[area] = new List<string>();
                        map[area].Add(issue);
                    }
                }
            }

            if (map.Count == 0)
                return IssueCatalog.GetIssueMap(); // seed fallback

            return map;
        }

        /// <summary>Issues (Area→Issue) haritasını kaydeder (tam yenileme).</summary>
        public static void SaveIssueMap(Dictionary<string, List<string>> map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        SaveIssueMapInternal(conn, map);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Logger.Log(ex);
                        throw;
                    }
                }
            }
        }

        private static void SaveIssueMapInternal(SQLiteConnection conn, Dictionary<string, List<string>> map)
        {
            ExecuteNonQuery(conn, $"DELETE FROM {ISSUES_TABLE};");

            string insert = $@"INSERT INTO {ISSUES_TABLE}(AreaName, IssueName) VALUES(@a,@i);";

            foreach (var area in map.Keys)
            {
                foreach (var issue in map[area]?.Distinct(StringComparer.OrdinalIgnoreCase) ?? Enumerable.Empty<string>())
                {
                    using (var cmd = new SQLiteCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@a", area);
                        cmd.Parameters.AddWithValue("@i", issue);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion

        #region Low-level Helpers

        private static void ExecuteNonQuery(SQLiteConnection conn, string sql)
        {
            using (var cmd = new SQLiteCommand(sql, conn))
                cmd.ExecuteNonQuery();
        }

        private static void ExecuteNonQuery(string cs, string sql)
        {
            using (var conn = new SQLiteConnection(cs))
            {
                conn.Open();
                ExecuteNonQuery(conn, sql);
            }
        }

        private static object ExecuteScalar(SQLiteConnection conn, string sql)
        {
            using (var cmd = new SQLiteCommand(sql, conn))
                return cmd.ExecuteScalar();
        }

        /// <summary>Tablo kolon adlarını set olarak döndürür.</summary>
        private static HashSet<string> GetTableColumns(SQLiteConnection conn, string tableName)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", conn))
            using (var r = cmd.ExecuteReader())
                while (r.Read())
                    set.Add(Convert.ToString(r["name"]));
            return set;
        }

        #endregion
    }
}
