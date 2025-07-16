// Helpers/AreaCatalog.cs
using System;
using System.Collections.Generic;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Alan ve alt-alan bilgilerini tutan sınıf
    /// </summary>
    public static class AreaCatalog
    {
        /// <summary>
        /// Her alan için alt-alanları tutan harita
        /// </summary>
        public static Dictionary<string, List<string>> GetAreaSubAreaMap()
        {
            return new Dictionary<string, List<string>>
            {
                ["UAP-1"] = new List<string>
                {
                    "GAP-12 Üretim Bandı",
                    "GAP-13 Üretim Bandı",
                    "GAP-14 Üretim Bandı",
                    "Kalite Kontrol Bölümü",
                    "Ambar Bölümü",
                    "Vardiya Ofisi",
                    "Bakım Atölyesi"
                },
                ["UAP-2"] = new List<string>
                {
                    "GAP-21 Üretim Bandı",
                    "GAP-22 Üretim Bandı",
                    "GAP-23 Üretim Bandı",
                    "Paketleme Bölümü",
                    "Sevkiyat Bölümü",
                    "Vardiya Ofisi",
                    "Teknik Büro"
                },
                ["UAP-3"] = new List<string>
                {
                    "GAP-31 Üretim Bandı",
                    "GAP-32 Üretim Bandı",
                    "GAP-33 Üretim Bandı",
                    "Hammadde Deposu",
                    "Laboratuvar",
                    "Vardiya Ofisi",
                    "Elektrik Panosu"
                },
                ["UAP-4"] = new List<string>
                {
                    "GAP-41 Üretim Bandı",
                    "GAP-42 Üretim Bandı",
                    "GAP-43 Üretim Bandı",
                    "Mamul Deposu",
                    "Teknik Servis",
                    "Vardiya Ofisi",
                    "Kompresör Odası"
                },
                ["FES"] = new List<string>
                {
                    "Yönetim Ofisleri",
                    "Muhasebe Bölümü",
                    "İnsan Kaynakları",
                    "Satış Bölümü",
                    "Pazarlama Bölümü",
                    "IT Bölümü",
                    "Güvenlik",
                    "Kantin",
                    "Toplantı Salonları"
                }
            };
        }
    }
}