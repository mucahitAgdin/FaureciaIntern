using System;
using System.Windows.Forms;
using TicketApp.Forms;
using TicketApp.Helpers;

namespace TicketApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {

                // Veritabanını başlat
                DatabaseHelper.InitializeDatabase();

                // Login formunu başlat
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show(
                    "Uygulama başlatılırken kritik hata!\n" +
                    $"Hata: {ex.Message}",
                    "Kritik Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
