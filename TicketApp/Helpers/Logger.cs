using System;
using System.IO;

namespace TicketApp.Helpers
{
    public static class Logger
    {
        public static void Log(Exception ex)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            File.AppendAllText(logPath, $"{DateTime.Now}: {ex.Message}{Environment.NewLine}");
        }
    }
}
