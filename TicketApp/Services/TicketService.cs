// Services/TicketService.cs
using System;
using System.Collections.Generic;
using TicketApp.Models;
using TicketApp.Helpers;

namespace TicketApp.Services
{
    public class TicketService
    {
        public void CreateTicket(Ticket ticket)
        {
            DatabaseHelper.InsertTicket(ticket);
        }

        public List<Ticket> GetTickets()
        {
            return DatabaseHelper.GetAllTickets();
        }

        public List<Ticket> GetResolved()
        {
            return DatabaseHelper.GetResolvedTickets();
        }

        public void ArchiveResolved()
        {
            var tickets = GetResolved();
            if (tickets.Count > 0)
            {
                string archiveFile = $"resolved_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                DatabaseHelper.ArchiveTickets(tickets, archiveFile);
                DatabaseHelper.DeleteResolvedTickets();
            }
        }
    }
}
