// Helpers/SecureDatabaseHelper.cs
using System;
using System.Data.SQLite;
using TicketApp.Models;
using TicketApp.Helpers;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Güvenli veritabanı işlemleri
    /// </summary>
    public static class SecureDatabaseHelper
    {
        /// <summary>
        /// Güvenli ticket ekleme
        /// </summary>
        public static bool InsertTicketSecure(Ticket ticket)
        {
            try
            {
                // Input validation
                if (!ValidateTicketInput(ticket))
                    return false;

                // Input sanitization
                ticket.Description = InputValidator.SanitizeInput(ticket.Description);
                ticket.FirstName = InputValidator.SanitizeInput(ticket.FirstName);
                ticket.LastName = InputValidator.SanitizeInput(ticket.LastName);

                // Normal DatabaseHelper metodunu kullan
                DatabaseHelper.InsertTicket(ticket);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log("Güvenli ticket ekleme hatası", ex);
                return false;
            }
        }

        /// <summary>
        /// Ticket input validation
        /// </summary>
        private static bool ValidateTicketInput(Ticket ticket)
        {
            // SQL Injection kontrolü
            if (InputValidator.ContainsSqlInjection(ticket.Description) ||
                InputValidator.ContainsSqlInjection(ticket.FirstName) ||
                InputValidator.ContainsSqlInjection(ticket.LastName))
            {
                Logger.Log("SQL Injection tespit edildi!");
                return false;
            }

            // XSS kontrolü
            if (InputValidator.ContainsXss(ticket.Description))
            {
                Logger.Log("XSS saldırısı tespit edildi!");
                return false;
            }

            // İsim kontrolü
            if (!InputValidator.IsValidTurkishName(ticket.FirstName) ||
                !InputValidator.IsValidTurkishName(ticket.LastName))
            {
                return false;
            }

            // Telefon kontrolü
            if (!InputValidator.IsValidTurkishPhoneNumber(ticket.PhoneNumber))
            {
                return false;
            }

            return true;
        }
    }
}