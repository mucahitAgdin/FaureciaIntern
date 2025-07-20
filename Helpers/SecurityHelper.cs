// Helpers/SecurityHelper.cs
using System;
using System.Security.Cryptography;
using System.Text;

namespace TicketApp.Helpers
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Şifreyi SHA256 ile hashler
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Şifre doğrulaması yapar
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}