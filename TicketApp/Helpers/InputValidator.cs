// Helpers/InputValidator.cs
using System;
using System.Text.RegularExpressions;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Input validation için güvenlik sınıfı
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// SQL Injection saldırılarını tespit eder
        /// </summary>
        public static bool ContainsSqlInjection(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string[] sqlKeywords = {
                "SELECT", "INSERT", "UPDATE", "DELETE", "DROP", "CREATE", "ALTER",
                "EXEC", "EXECUTE", "UNION", "SCRIPT", "JAVASCRIPT", "VBSCRIPT",
                "--", "/*", "*/", ";", "'", "\""
            };

            string upperInput = input.ToUpperInvariant();
            foreach (string keyword in sqlKeywords)
            {
                if (upperInput.Contains(keyword))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// XSS saldırılarını tespit eder
        /// </summary>
        public static bool ContainsXss(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string[] xssPatterns = {
                "<script", "</script>", "javascript:", "vbscript:", "onload=",
                "onerror=", "onclick=", "onmouseover=", "<iframe", "<object",
                "<embed", "<applet"
            };

            string lowerInput = input.ToLowerInvariant();
            foreach (string pattern in xssPatterns)
            {
                if (lowerInput.Contains(pattern))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Türk telefon numarası formatını kontrol eder
        /// </summary>
        public static bool IsValidTurkishPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

            // Türk telefon numarası regex: +90 veya 0 ile başlayıp 5XX XXX XX XX formatında
            string pattern = @"^(\+90|0)?[5][0-9]{9}$";
            string cleanedNumber = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            return Regex.IsMatch(cleanedNumber, pattern);
        }

        /// <summary>
        /// Türkçe karakter içeren isim kontrolü
        /// </summary>
        public static bool IsValidTurkishName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (name.Length < 2 || name.Length > 50) return false;

            // Türkçe karakterler dahil sadece harfler ve boşluk
            string pattern = @"^[a-zA-ZçğıöşüÇĞIİÖŞÜ\s]+$";
            return Regex.IsMatch(name, pattern);
        }

        /// <summary>
        /// Email format kontrolü
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Güvenli string sanitization
        /// </summary>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // HTML encode (basit versiyon)
            input = input.Replace("&", "&amp;")
                        .Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("\"", "&quot;")
                        .Replace("'", "&#x27;");

            // Fazla boşlukları temizle
            input = Regex.Replace(input, @"\s+", " ").Trim();

            return input;
        }

        /// <summary>
        /// Dosya yolu güvenlik kontrolü
        /// </summary>
        public static bool IsValidFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return false;

            // Path traversal saldırılarını engelle
            string[] dangerousPatterns = { "..", "~/", "\\", ":", "*", "?", "\"", "<", ">", "|" };

            foreach (string pattern in dangerousPatterns)
            {
                if (filePath.Contains(pattern))
                    return false;
            }

            return true;
        }
    }
}