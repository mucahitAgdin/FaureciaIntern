/*// Helpers/NetworkDatabaseHelper.cs
using System;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Ağ üzerinden veritabanı erişimi için yardımcı sınıf
    /// </summary>
    public static class NetworkDatabaseHelper
    {
        private static readonly object _lockObject = new object();
        private static string _currentDbPath = null;
        private static bool _isNetworkAvailable = false;

        /// <summary>
        /// Aktif veritabanı yolu
        /// </summary>
        public static string DatabasePath
        {
            get
            {
                if (_currentDbPath == null)
                {
                    _currentDbPath = GetOptimalDatabasePath();
                }
                return _currentDbPath;
            }
        }

        /// <summary>
        /// Connection string
        /// </summary>
        public static string ConnectionString => BuildConnectionString();

        /// <summary>
        /// Ağ bağlantısı var mı?
        /// </summary>
        public static bool IsNetworkMode => _isNetworkAvailable;

        /// <summary>
        /// En uygun veritabanı yolunu belirler
        /// </summary>
        private static string GetOptimalDatabasePath()
        {
            try
            {
                bool useNetwork = bool.Parse(ConfigurationManager.AppSettings["UseNetworkDatabase"] ?? "false");

                if (useNetwork)
                {
                    string networkPath = ConfigurationManager.AppSettings["NetworkDatabasePath"];

                    if (!string.IsNullOrEmpty(networkPath) && TestNetworkPath(networkPath))
                    {
                        _isNetworkAvailable = true;
                        Logger.Log($"Ağ veritabanı kullanılıyor: {networkPath}");
                        return networkPath;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Ağ yolu kontrol hatası: {ex.Message}");
            }

            // Lokal veritabanını kullan
            _isNetworkAvailable = false;
            string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["LocalDatabasePath"] ?? "tickets.db");

            Logger.Log($"Lokal veritabanı kullanılıyor: {localPath}");
            return localPath;
        }

        /// <summary>
        /// Ağ yolunu test eder
        /// </summary>
        private static bool TestNetworkPath(string networkPath)
        {
            try
            {
                // Önce sunucuya ping at
                string serverIP = ExtractServerIP(networkPath);
                if (!string.IsNullOrEmpty(serverIP) && !PingServer(serverIP))
                {
                    return false;
                }

                // Dosya erişimini test et
                if (File.Exists(networkPath))
                {
                    // Okuma testi
                    using (var fs = File.Open(networkPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sunucuya ping atar
        /// </summary>
        private static bool PingServer(string ipAddress)
        {
            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = ping.Send(ipAddress, 1000); // 1 saniye timeout
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ağ yolundan IP adresini çıkarır
        /// </summary>
        private static string ExtractServerIP(string networkPath)
        {
            try
            {
                // \\192.168.1.100\share\file.db formatından IP'yi al
                if (networkPath.StartsWith(@"\\"))
                {
                    string[] parts = networkPath.Substring(2).Split('\\');
                    if (parts.Length > 0)
                    {
                        return parts[0];
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Connection string oluşturur
        /// </summary>
        private static string BuildConnectionString()
        {
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = DatabasePath,
                Version = 3,
                JournalMode = SQLiteJournalModeEnum.Wal, // Write-Ahead Logging
                DefaultTimeout = 30,
                BusyTimeout = 5000, // 5 saniye busy timeout
                Pooling = true,
                CacheSize = 10000
            };

            return builder.ToString();
        }

        /// <summary>
        /// Bağlantıyı yeniden değerlendirir
        /// </summary>
        public static void RefreshConnection()
        {
            lock (_lockObject)
            {
                _currentDbPath = null;
                _currentDbPath = GetOptimalDatabasePath();
            }
        }

        /// <summary>
        /// Güvenli veritabanı işlemi yapar (retry mekanizmalı)
        /// </summary>
        public static T ExecuteWithRetry<T>(Func<SQLiteConnection, T> operation, int maxRetries = 3)
        {
            int attempt = 0;
            Exception lastException = null;

            while (attempt < maxRetries)
            {
                attempt++;

                try
                {
                    using (var conn = new SQLiteConnection(ConnectionString))
                    {
                        conn.Open();
                        return operation(conn);
                    }
                }
                catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Busy ||
                                                 ex.ResultCode == SQLiteErrorCode.Locked)
                {
                    lastException = ex;
                    Logger.Log($"Veritabanı meşgul, deneme {attempt}/{maxRetries}");
                    Thread.Sleep(1000 * attempt); // Artan bekleme süresi
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    // Ağ hatası ise lokal'e geç
                    if (_isNetworkAvailable && attempt == 1)
                    {
                        Logger.Log("Ağ hatası, lokal veritabanına geçiliyor...");
                        RefreshConnection();
                        continue;
                    }

                    throw;
                }
            }

            throw new Exception($"İşlem {maxRetries} denemeden sonra başarısız oldu.", lastException);
        }

        /// <summary>
        /// Veritabanı senkronizasyonu (opsiyonel)
        /// </summary>
        public static void SyncDatabases()
        {
            try
            {
                if (!_isNetworkAvailable) return;

                string networkPath = ConfigurationManager.AppSettings["NetworkDatabasePath"];
                string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tickets.db");

                if (File.Exists(networkPath) && File.Exists(localPath))
                {
                    // Hangi veritabanı daha güncel?
                    var networkInfo = new FileInfo(networkPath);
                    var localInfo = new FileInfo(localPath);

                    if (networkInfo.LastWriteTime > localInfo.LastWriteTime)
                    {
                        Logger.Log("Ağ veritabanı daha güncel, lokal güncelleniyor...");
                        File.Copy(networkPath, localPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Senkronizasyon hatası: {ex.Message}");
            }
        }
    }
}*/