//Models/Ticket.cs

using System;

namespace TicketApp.Models
{
    /// <summary>
    /// Ticket sınıfı, kullanıcıdan alınan destek taleplerini temsil eder.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Veritabanındaki otomatik artan kimlik numarası
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Hangi bölümden (UAP-1, FES vs.) talep geldiği
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Hangi sorunla ilgili talep açıldığı (örneğin: "Mouse bozuldu")
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// Kullanıcının isteğe dair yazdığı açıklama (300 karaktere kadar)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Talebin oluşturulma zamanı (kayıt tarihi)
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Talep çözüldü mü? (false = çözülmedi, true = çözüldü)
        /// </summary>
        public bool IsResolved { get; set; } = false;

        public string Status { get; set; } // "beklemede", "işlemde", "çözüldü"


    }
}
