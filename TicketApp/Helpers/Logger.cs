using System;
using System.IO;

namespace TicketApp.Helpers
{
    public static class Logger
    {
        private static string logPath = "error.log";

        // Writes exceptions to a log file
        public static void Log(Exception ex)
        {
            try
            {
                string message = $"[{DateTime.Now}] {ex.Message}\n{ex.StackTrace}\n";
                File.AppendAllText(logPath, message);
            }
            catch
            {
                // If logging fails, don't throw again.
            }
        }
    }
}
