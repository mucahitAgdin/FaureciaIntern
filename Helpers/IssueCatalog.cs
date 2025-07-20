// Helpers/IssueCatalog.cs

using System.Collections.Generic;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Bölge ve sorun tiplerini haritalayan yardımcı sınıf.
    /// </summary>
    public static class IssueCatalog
    {
        /// <summary>
        /// Sorun haritalama sözlüğünü döndürür.
        /// </summary>
        public static Dictionary<string, List<string>> GetIssueMap()
        {
            return new Dictionary<string, List<string>>()
            {
                { "GENEL", new List<string> {
                    "Bilgisayar açılmıyor",
                    "Mouse bozuldu",
                    "Klavye çalışmıyor",
                    "Yazıcıya kağıt sıkıştı",
                    "Yazıcıya internet gitmiyor",
                    "Yazıcı çalışmıyor"
                }},
                { "UAP-1", new List<string> { "SAP terminal donuyor", "Barkod yazıcı hatası" }},
                { "UAP-2", new List<string> { "IP erişilemiyor" }},
                { "FES", new List<string> { "PLC bağlantısı kesildi", "Etiket yazıcı çevrimdışı" }}
            };
        }
    }
}
