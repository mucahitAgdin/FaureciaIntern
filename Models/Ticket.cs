// Models/Ticket.cs
using System;

namespace TicketApp.Models
{
    /// <summary>
    /// Destek talebi model sınıfı.
    /// Yeni alan: Line (Hat). Zincir: Area > SubArea > Line > Issue > Description
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }

        public string Area { get; set; }         // Örn: UAP-1, UAP-2 ...  (Alan)
        public string SubArea { get; set; }      // Örn: GAP-10 (Alt Alan)

        // YENİ: Hat (Line). Alt Alan seçimine bağlıdır.
        // DB tarafında Lines tablosu ile saklanacak. UI'da zorunlu alan olarak kullanacağız.
        public string Line { get; set; }         // Örn: Hat-1, Hat-2 ...

        public string Issue { get; set; }        // Sorun tipi
        public string Description { get; set; }  // Detaylı açıklama

        public string FirstName { get; set; }    // Kullanıcı adı
        public string LastName { get; set; }     // Kullanıcı soyadı
        public string PhoneNumber { get; set; }  // İletişim numarası

        public DateTime CreatedAt { get; set; }  // Oluşturulma tarihi
        public bool IsResolved { get; set; }     // Çözülmüş mü?

        // Durum akışı: beklemede | işlemde | çözüldü | reddedildi
        public string Status { get; set; }

        public string AssignedTo { get; set; }     // Atanan kişi
        public string RejectionReason { get; set; } // Red sebebi

        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        /// <summary>Tam kullanıcı adı</summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Tam alan bilgisi:
        /// - SubArea yoksa: Area
        /// - SubArea var, Line yoksa: Area - SubArea
        /// - Hepsi varsa: Area - SubArea - Line
        /// </summary>
        public string FullArea
        {
            get
            {
                if (string.IsNullOrEmpty(SubArea))
                    return Area ?? string.Empty;

                if (string.IsNullOrEmpty(Line))
                    return $"{Area} - {SubArea}";

                return $"{Area} - {SubArea} - {Line}";
            }
        }
    }
}
