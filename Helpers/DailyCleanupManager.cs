// Helpers/DailyCleanupManager.cs

using System;
using System.Windows.Forms;
using TicketApp.Helpers;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Günlük temizlik (arşivleme) işlemini yöneten sınıf.
    /// </summary>
    public class DailyCleanupManager
    {
        private Timer cleanupTimer;

        public DailyCleanupManager()
        {
            cleanupTimer = new Timer();
            cleanupTimer.Interval = 86400000; // 24 saat
            cleanupTimer.Tick += CleanupTimer_Tick;
        }

        public void Start() => cleanupTimer.Start();

        private void CleanupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var resolved = DatabaseHelper.GetResolvedTickets();

                if (resolved.Count > 0)
                {
                    var archiveFile = $"resolved_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    DatabaseHelper.ArchiveTickets(resolved, archiveFile);
                    DatabaseHelper.DeleteResolvedTickets();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}
