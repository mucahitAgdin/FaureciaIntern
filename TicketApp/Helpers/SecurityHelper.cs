// Helpers/SecurityHelper.cs - YENİ DOSYA
using System;
using System.Security.Cryptography;
using System.Text;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Güvenlik işlemleri için yardımcı sınıf
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Şifreyi SHA256 ile hashler
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Şifre boş olamaz");

            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Şifre doğrulaması yapar
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            string inputHash = HashPassword(password);
            return string.Equals(inputHash, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Rastgele güvenli şifre oluşturur
        /// </summary>
        public static string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            var password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }

        /// <summary>
        /// Ticket için güvenli takip numarası oluşturur
        /// </summary>
        public static string GenerateTicketNumber()
        {
            var prefix = AppConfigReader.Get("TicketNumberPrefix", "IT");
            var year = DateTime.Now.Year;
            var timestamp = DateTime.Now.Ticks.ToString().Substring(10, 6);
            var random = new Random().Next(100, 999);

            return $"{prefix}-{year}-{timestamp}{random}";
        }

        /// <summary>
        /// SQL Injection koruması için string temizleme
        /// </summary>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Tehlikeli karakterleri temizle
            return input.Replace("'", "''")
                       .Replace("--", "")
                       .Replace("/*", "")
                       .Replace("*/", "")
                       .Replace("xp_", "")
                       .Replace("sp_", "");
        }

        /// <summary>
        /// XSS koruması için HTML temizleme
        /// </summary>
        public static string SanitizeHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return html.Replace("<", "&lt;")
                      .Replace(">", "&gt;")
                      .Replace("\"", "&quot;")
                      .Replace("'", "&#x27;")
                      .Replace("/", "&#x2F;");
        }

        /// <summary>
        /// Telefon numarasını maskeler (güvenlik için)
        /// </summary>
        public static string MaskPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 4)
                return phoneNumber;

            // Son 4 hane hariç maskele
            var masked = new StringBuilder();
            for (int i = 0; i < phoneNumber.Length - 4; i++)
            {
                masked.Append(char.IsDigit(phoneNumber[i]) ? '*' : phoneNumber[i]);
            }
            masked.Append(phoneNumber.Substring(phoneNumber.Length - 4));

            return masked.ToString();
        }

        /// <summary>
        /// E-posta adresini maskeler
        /// </summary>
        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                return email;

            var parts = email.Split('@');
            var username = parts[0];
            var domain = parts[1];

            if (username.Length <= 2)
                return email;

            var maskedUsername = username[0] + new string('*', username.Length - 2) + username[username.Length - 1];
            return $"{maskedUsername}@{domain}";
        }

        /// <summary>
        /// Güvenli session token oluşturur
        /// </summary>
        public static string GenerateSessionToken()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// IP adresini anonimleştirir (GDPR uyumluluğu için)
        /// </summary>
        public static string AnonymizeIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return ipAddress;

            var parts = ipAddress.Split('.');
            if (parts.Length == 4)
            {
                // Son oktet'i sıfırla
                parts[3] = "0";
                return string.Join(".", parts);
            }

            return ipAddress;
        }
    }
}