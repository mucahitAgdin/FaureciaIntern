// Helpers/ErrorHandler.cs
using System;
using System.Windows.Forms;

namespace TicketApp.Helpers
{
    /// <summary>
    /// Merkezi hata yönetimi sistemi
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Hata türleri
        /// </summary>
        public enum ErrorType
        {
            Database,
            Validation,
            Network,
            FileSystem,
            Configuration,
            Unknown
        }

        /// <summary>
        /// Kullanıcıya hata mesajı gösterir ve log'a kaydeder
        /// </summary>
        public static void HandleError(Exception ex, string userMessage = null, ErrorType errorType = ErrorType.Unknown)
        {
            // Log'a kaydet
            Logger.Log(ex);

            // Kullanıcıya gösterilecek mesajı belirle
            string displayMessage = GetUserFriendlyMessage(ex, userMessage, errorType);

            // Hata türüne göre ikon belirle
            MessageBoxIcon icon = GetIconForErrorType(errorType);

            // Mesajı göster
            MessageBox.Show(displayMessage, "Hata Oluştu", MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Kullanıcı dostu hata mesajı oluşturur
        /// </summary>
        private static string GetUserFriendlyMessage(Exception ex, string userMessage, ErrorType errorType)
        {
            if (!string.IsNullOrEmpty(userMessage))
                return userMessage;

            switch (errorType)
            {
                case ErrorType.Database:
                    return "Veritabanı bağlantısında sorun oluştu. Lütfen daha sonra tekrar deneyin.";
                case ErrorType.Validation:
                    return "Girilen bilgilerde hata var. Lütfen kontrol edin.";
                case ErrorType.Network:
                    return "Ağ bağlantısında sorun var. İnternet bağlantınızı kontrol edin.";
                case ErrorType.FileSystem:
                    return "Dosya işleminde hata oluştu. Dosya izinlerini kontrol edin.";
                case ErrorType.Configuration:
                    return "Uygulama ayarlarında sorun var. Yönetici ile iletişime geçin.";
                default:
                    return "Beklenmeyen bir hata oluştu. Detaylar log dosyasına kaydedildi.";
            }
        }

        /// <summary>
        /// Hata türüne göre ikon döner
        /// </summary>
        private static MessageBoxIcon GetIconForErrorType(ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.Validation:
                    return MessageBoxIcon.Warning;
                case ErrorType.Configuration:
                    return MessageBoxIcon.Information;
                default:
                    return MessageBoxIcon.Error;
            }
        }

        /// <summary>
        /// Kritik hata durumunda uygulamayı güvenli şekilde kapatır
        /// </summary>
        public static void HandleCriticalError(Exception ex, string message = null)
        {
            Logger.Log(ex);

            string criticalMessage = message ??
                "Kritik bir hata oluştu ve uygulama kapatılacak. " +
                "Detaylar log dosyasına kaydedildi.";

            MessageBox.Show(criticalMessage, "Kritik Hata",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);

            // Uygulamayı güvenli şekilde kapat
            Application.Exit();
        }

        /// <summary>
        /// Validasyon hatası gösterir
        /// </summary>
        public static bool ShowValidationError(string message, Control focusControl = null)
        {
            MessageBox.Show(message, "Geçersiz Giriş",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (focusControl != null)
                focusControl.Focus();

            return false; // Validasyon başarısız
        }

        /// <summary>
        /// Onay sorusu gösterir
        /// </summary>
        public static bool ShowConfirmation(string message, string title = "Onay")
        {
            var result = MessageBox.Show(message, title,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Başarı mesajı gösterir
        /// </summary>
        public static void ShowSuccess(string message, string title = "Başarılı")
        {
            MessageBox.Show(message, title,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}