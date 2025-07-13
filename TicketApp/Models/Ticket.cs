using System;

namespace TicketApp.Models
{
    /// <summary>
    /// Kullanıcının oluşturduğu destek talebini temsil eden sınıf.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Veritabanı için otomatik artan kimlik (ID).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sorunun yaşandığı alan (örneğin: UAP-1, FES).
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Sorunun tipi (örneğin: Mouse bozuldu, SAP donuyor).
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// Kullanıcı tarafından açıklama girilebilir.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Talebin oluşturulma zamanı.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Talebin çözülüp çözülmediğini belirten bayrak.
        /// false: çözülmedi, true: çözüldü.
        /// </summary>
        public bool IsResolved { get; set; } = false;
    }
}
