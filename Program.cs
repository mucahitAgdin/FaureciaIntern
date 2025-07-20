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
                Logger.Log("Uygulama başlatıldı.");

                // Günlük temizlik yöneticisini başlat
                var cleanupManager = new DailyCleanupManager();
                cleanupManager.Start();
                Logger.Log("Günlük temizlik yöneticisi başlatıldı.");

                // Login formunu başlat
                // Bu form hem admin girişi hem de kullanıcı girişi sağlar
                Application.Run(new LoginForm());

                Logger.Log("Uygulama kapatıldı.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show(
                    "Uygulama başlatılırken kritik hata oluştu!\n\n" +
                    "Hata detayları log dosyasına kaydedildi.\n" +
                    "Lütfen sistem yöneticisi ile iletişime geçin.\n\n" +
                    $"Hata: {ex.Message}",
                    "Kritik Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}