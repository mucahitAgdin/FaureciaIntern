// Program.cs
using System;
using System.Windows.Forms;
using TicketApp.Forms;
using TicketApp.Helpers;

namespace TicketApp
{
    /// <summary>
    /// Uygulamanın ana başlangıç noktası
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktası
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Veritabanını başlat
                DatabaseHelper.InitializeDatabase();

                // Günlük temizlik yöneticisini başlat
                var cleanupManager = new DailyCleanupManager();
                cleanupManager.Start();

                // Ana formu başlat
                Application.Run(new MainForm());
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Uygulama başlatılırken kritik hata oluştu.", "Kritik Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}