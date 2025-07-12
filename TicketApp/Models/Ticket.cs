using System;

namespace TicketApp.Models
{
    public class Ticket
    {
        public string Area { get; set; }
        public string Issue { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
