// Logger.cs
using System;
using System.IO;

namespace TicketApp.Helpers
{
    // Logger sınıfı uygulamadaki hataları log dosyasına yazmak için kullanılır.
    public static class Logger
    {
        // Hata loglarının yazılacağı dosya yolu
        private static readonly string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");

        // Hata mesajını dosyaya yazar
        public static void Log(Exception ex)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logPath, true))
                {
                    sw.WriteLine($"[{DateTime.Now}] {ex.Message}");
                    sw.WriteLine(ex.StackTrace);
                    sw.WriteLine(new string('-', 50));
                }
            }
            catch
            {
                // Log yazma da başarısız olursa sessizce geçilir.
            }
        }
    }
}
