// TicketApp/Program.cs
// Uygulamanın giriş noktası
using System;
using System.Windows.Forms;
using TicketApp.Forms; // Ana form burada tanımlı

namespace TicketApp
{
    internal static class Program
    {
        [STAThread] // WinForms uygulamaları tek iş parçacıklı çalışır (Single-Threaded Apartment)
        static void Main()
        {
            // Uygulama için modern görsel stiller etkinleştirilir
            Application.EnableVisualStyles();
            // Varsayılan yazı tiplerinin uyumluluğu sağlanır
            Application.SetCompatibleTextRenderingDefault(false);
            // Uygulamanın ana formu başlatılır
            Application.Run(new Form1()); 
        }
    }
}