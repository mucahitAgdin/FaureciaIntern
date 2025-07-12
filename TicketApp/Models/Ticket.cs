using System;

namespace TicketApp.Models
{
    // This class defines the structure of a ticket record
    public class Ticket
    {
        public int Id { get; set; }                 // Auto-increment ID
        public string Area { get; set; }            // UAP-1, FES, etc.
        public string Issue { get; set; }           // Problem description
        public string Description { get; set; }     // Optional user notes
        public DateTime CreatedAt { get; set; }     // Time of submission
    }
}
