// Helpers/Logger.cs
using System;
using System.IO;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Uygulama hatalarını ve bilgileri dosyaya kaydeden yardımcı sınıf
    /// </summary>
    public static class Logger
    {
        private static readonly string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static readonly object lockObject = new object();

        static Logger()
        {
            // Logs klasörünü oluştur
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
        }

        /// <summary>
        /// Hata bilgisini log dosyasına kaydeder
        /// </summary>
        public static void Log(Exception ex)
        {
            try
            {
                string logFile = Path.Combine(logPath, $"error_{DateTime.Now:yyyyMMdd}.log");
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}\n" +
                                  $"StackTrace: {ex.StackTrace}\n" +
                                  $"Source: {ex.Source}\n" +
                                  new string('-', 80) + "\n";

                lock (lockObject)
                {
                    File.AppendAllText(logFile, logMessage);
                }
            }
            catch
            {
                // Log yazma hatası durumunda sessiz devam et
            }
        }

        /// <summary>
        /// Bilgi mesajını log dosyasına kaydeder
        /// </summary>
        public static void Log(string message)
        {
            try
            {
                string logFile = Path.Combine(logPath, $"info_{DateTime.Now:yyyyMMdd}.log");
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}\n";

                lock (lockObject)
                {
                    File.AppendAllText(logFile, logMessage);
                }
            }
            catch
            {
                // Log yazma hatası durumunda sessiz devam et
            }
        }
    }
}