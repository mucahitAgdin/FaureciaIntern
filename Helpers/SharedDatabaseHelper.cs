using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TicketApp.Helpers;

namespace SharedDatabaseHelper.Helper
{
    /// <summary>
    /// SQLite bağlantılarını yöneten yardımcı sınıftır.
    /// Yeniden deneme ve hata yönetimi içerir.
    /// </summary>
    public static class SharedDatabaseHelper
    {
        private static readonly string connectionString =
            new SQLiteConnectionStringBuilder
            {
                DataSource = "tickets.db",
                Version = 3,
                JournalMode = SQLiteJournalModeEnum.Wal,
                SyncMode = SynchronizationModes.Off,
                CacheSize = 10000,
                PageSize = 4096,
                FailIfMissing = false
            }.ToString();

        private static readonly object lockObject = new object();

        /// <summary>
        /// SQLite bağlantısını test eder.
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        /// SQLite komutlarını güvenli şekilde çalıştırır, yeniden deneme yapar.
        /// </summary>
        public static void ExecuteWithRetry(Action<SQLiteConnection> action, int maxRetries = 3)
        {
            int retryCount = 0;
            bool success = false;

            while (!success && retryCount < maxRetries)
            {
                try
                {
                    lock (lockObject)
                    {
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            using (var command = new SQLiteCommand("PRAGMA busy_timeout = 3000;", connection))
                            {
                                command.ExecuteNonQuery();
                            }

                            action(connection);
                            success = true;
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Logger.Log(ex);
                    retryCount++;
                    Thread.Sleep(100 * retryCount); // Artan gecikme
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    break;
                }
            }

            if (!success)
            {
                ShowConnectionError();
            }
        }

        /// <summary>
        /// Veritabanına bağlanılamadığında kullanıcıya hata mesajı gösterir.
        /// </summary>
        private static void ShowConnectionError()
        {
            MessageBox.Show("Veritabanına bağlanılamadı. Lütfen daha sonra tekrar deneyiniz.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Sistemde aktif kullanıcıların listesini getirir.
        /// </summary>
        public static List<string> GetActiveUsers()
        {
            List<string> users = new List<string>();

            ExecuteWithRetry(connection =>
            {
                using (var command = new SQLiteCommand("SELECT DISTINCT AssignedTo FROM Tickets WHERE AssignedTo IS NOT NULL AND AssignedTo <> ''", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(reader.GetString(0));
                    }
                }
            });

            return users;
        }

        /// <summary>
        /// Eğer gerekiyorsa veritabanını ilk açılışta oluşturur. (Varsa sil)
        /// </summary>
        public static void InitializeDatabaseIfMissing()
        {
            if (!File.Exists("tickets.db"))
            {
                SQLiteConnection.CreateFile("tickets.db");
                Logger.Log("Yeni veritabanı dosyası oluşturuldu.");
                // İlk kurulum yapılabilir.
            }
        }

        /// <summary>
        /// Ortak veritabanını başlatır ve bağlantıyı test eder
        /// </summary>
        public static bool Initialize()
        {
            InitializeDatabaseIfMissing();  // Dosya yoksa oluştur
            return TestConnection();        // Bağlantıyı test et
        }

    }
}
