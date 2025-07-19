// Models/Ticket.cs
using System;

namespace TicketApp.Models
{
    /// <summary>
    /// Destek talebi bilgilerini tutan model sınıfı
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }
        public string Area { get; set; }           // UAP-1, UAP-2, etc.
        public string SubArea { get; set; }        // GAP-12, Üretim Bandı, etc.
        public string Issue { get; set; }          // Sorun tipi
        public string Description { get; set; }    // Detaylı açıklama
        public string FirstName { get; set; }      // Kullanıcı adı
        public string LastName { get; set; }       // Kullanıcı soyadı
        public string PhoneNumber { get; set; }    // İletişim numarası
        public DateTime CreatedAt { get; set; }    // Oluşturulma tarihi
        public bool IsResolved { get; set; }       // Çözülmüş mü?
        public string Status { get; set; }         // beklemede, işlemde, çözüldü, reddedildi
        public string AssignedTo { get; set; }     // Atanan kişi
        public string RejectionReason { get; set; } // Red sebebi (eğer varsa)
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        /// <summary>
        /// Tam kullanıcı adı
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Tam alan bilgisi
        /// </summary>
        public string FullArea => string.IsNullOrEmpty(SubArea) ? Area : $"{Area} - {SubArea}";
    }
}