using System;
using System.IO;

namespace TicketApp.Helpers
{
    public static class Logger
    {
        private static string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");

        public static void Log(Exception ex)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logPath, true))
                {
                    sw.WriteLine($"[{DateTime.Now}] {ex.Message}");
                    sw.WriteLine(ex.StackTrace);
                    sw.WriteLine("--------------------------------------------------");
                }
            }
            catch
            {
                // Logging failed silently
            }
        }
    }
}
