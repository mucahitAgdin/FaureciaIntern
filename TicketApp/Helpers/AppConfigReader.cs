//Helpers/AppConfigReader.cs

using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace TicketApp.Helpers
{
    /// <summary>
    /// App.config dosyasındaki ayarları okumak için yardımcı sınıf
    /// </summary>
    public static class AppConfigReader
    {
        /// <summary>
        /// Anahtara karşılık gelen değeri App.config dosyasından döner.
        /// </summary>
        public static string Get(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
