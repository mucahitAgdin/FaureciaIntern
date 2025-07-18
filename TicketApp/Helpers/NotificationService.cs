// Services/NotificationService.cs - YENİ DOSYA
using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using TicketApp.Helpers;
using TicketApp.Models;

namespace TicketApp.Services
{
    /// <summary>
    /// Bildirim gönderme servisi (E-posta ve sistem bildirimleri)
    /// </summary>
    public class NotificationService
    {
        private readonly bool _emailEnabled;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _adminEmail;

        public NotificationService()
        {
            // App.config'den ayarları oku
            _emailEnabled = AppConfigReader.GetBool("EmailNotificationsEnabled", false);
            _smtpServer = AppConfigReader.Get("SmtpServer");
            _smtpPort = AppConfigReader.GetInt("SmtpPort", 587);
            _smtpUsername = AppConfigReader.Get("SmtpUsername");
            _smtpPassword = AppConfigReader.Get("SmtpPassword");
            _fromEmail = AppConfigReader.Get("NotificationFromEmail", "noreply@example.com");
            _adminEmail = AppConfigReader.Get("AdminNotificationEmail");
        }

        /// <summary>
        /// Yeni ticket oluşturulduğunda bildirim gönder
        /// </summary>
        public void NotifyNewTicket(Ticket ticket)
        {
            try
            {
                // Sistem bildirimi
                ShowSystemNotification("Yeni Destek Talebi",
                    $"#{ticket.TicketNumber} - {ticket.Issue}\nOluşturan: {ticket.FullName}");

                // E-posta bildirimi
                if (_emailEnabled && !string.IsNullOrEmpty(_adminEmail))
                {
                    var subject = $"Yeni Destek Talebi: #{ticket.TicketNumber}";
                    var body = GenerateNewTicketEmailBody(ticket);
                    SendEmail(_adminEmail, subject, body);
                }

                Logger.Log($"Yeni ticket bildirimi gönderildi: #{ticket.TicketNumber}");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Ticket durumu değiştiğinde bildirim gönder
        /// </summary>
        public void NotifyStatusChange(Ticket ticket, string oldStatus, string newStatus)
        {
            try
            {
                // Kullanıcıya e-posta gönder (e-posta adresi varsa)
                if (_emailEnabled && !string.IsNullOrEmpty(ticket.Email))
                {
                    var subject = $"Destek Talebiniz Güncellendi: #{ticket.TicketNumber}";
                    var body = GenerateStatusChangeEmailBody(ticket, oldStatus, newStatus);
                    SendEmail(ticket.Email, subject, body);
                }

                // Admin'e sistem bildirimi
                ShowSystemNotification("Ticket Durumu Değişti",
                    $"#{ticket.TicketNumber}\n{oldStatus} → {newStatus}");

                Logger.Log($"Ticket durum değişikliği bildirimi: #{ticket.TicketNumber} {oldStatus} → {newStatus}");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Ticket atandığında bildirim gönder
        /// </summary>
        public void NotifyAssignment(Ticket ticket, string assignedTo)
        {
            try
            {
                ShowSystemNotification("Ticket Atandı",
                    $"#{ticket.TicketNumber} - {ticket.Issue}\nAtanan: {assignedTo}");

                // Atanan kişiye e-posta gönderilebilir (e-posta adresi biliniyorsa)
                Logger.Log($"Ticket atama bildirimi: #{ticket.TicketNumber} → {assignedTo}");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Ticket çözüldüğünde bildirim gönder
        /// </summary>
        public void NotifyResolution(Ticket ticket)
        {
            try
            {
                // Kullanıcıya e-posta
                if (_emailEnabled && !string.IsNullOrEmpty(ticket.Email))
                {
                    var subject = $"Destek Talebiniz Çözüldü: #{ticket.TicketNumber}";
                    var body = GenerateResolutionEmailBody(ticket);
                    SendEmail(ticket.Email, subject, body);
                }

                ShowSystemNotification("Ticket Çözüldü",
                    $"#{ticket.TicketNumber} - {ticket.Issue}\nÇözüm süresi: {ticket.ResolutionTime?.TotalHours:F1} saat");

                Logger.Log($"Ticket çözüm bildirimi: #{ticket.TicketNumber}");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Sistem bildirimi göster (Windows notification)
        /// </summary>
        private void ShowSystemNotification(string title, string message)
        {
            try
            {
                var notification = new NotifyIcon
                {
                    Icon = System.Drawing.SystemIcons.Information,
                    BalloonTipTitle = title,
                    BalloonTipText = message,
                    Visible = true
                };

                notification.ShowBalloonTip(3000);

                // 5 saniye sonra kaldır
                var timer = new Timer { Interval = 5000 };
                timer.Tick += (s, e) =>
                {
                    notification.Visible = false;
                    notification.Dispose();
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                Logger.Log($"Sistem bildirimi gösterilemedi: {ex.Message}");
            }
        }

        /// <summary>
        /// E-posta gönder
        /// </summary>
        private void SendEmail(string to, string subject, string body)
        {
            if (!_emailEnabled || string.IsNullOrEmpty(_smtpServer))
                return;

            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(_fromEmail, "IT Destek Sistemi");
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"E-posta gönderilemedi: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni ticket e-posta içeriği oluştur
        /// </summary>
        private string GenerateNewTicketEmailBody(Ticket ticket)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f4f4f4; padding: 20px; margin-top: 20px; }}
        .field {{ margin: 10px 0; }}
        .label {{ font-weight: bold; color: #333; }}
        .value {{ color: #666; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Yeni Destek Talebi Oluşturuldu</h2>
        </div>
        <div class='content'>
            <div class='field'>
                <span class='label'>Ticket No:</span>
                <span class='value'>#{ticket.TicketNumber}</span>
            </div>
            <div class='field'>
                <span class='label'>Oluşturan:</span>
                <span class='value'>{ticket.FullName}</span>
            </div>
            <div class='field'>
                <span class='label'>Telefon:</span>
                <span class='value'>{ticket.PhoneNumber}</span>
            </div>
            <div class='field'>
                <span class='label'>Alan:</span>
                <span class='value'>{ticket.FullArea}</span>
            </div>
            <div class='field'>
                <span class='label'>Sorun:</span>
                <span class='value'>{ticket.Issue}</span>
            </div>
            <div class='field'>
                <span class='label'>Açıklama:</span>
                <span class='value'>{ticket.Description}</span>
            </div>
            <div class='field'>
                <span class='label'>Öncelik:</span>
                <span class='value'>{ticket.PriorityText}</span>
            </div>
            <div class='field'>
                <span class='label'>Tarih:</span>
                <span class='value'>{ticket.CreatedAt:dd.MM.yyyy HH:mm}</span>
            </div>
        </div>
        <div class='footer'>
            <p>Bu e-posta IT Destek Sistemi tarafından otomatik olarak gönderilmiştir.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Durum değişikliği e-posta içeriği
        /// </summary>
        private string GenerateStatusChangeEmailBody(Ticket ticket, string oldStatus, string newStatus)
        {
            var statusColor = ticket.StatusColor;
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: {statusColor}; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f4f4f4; padding: 20px; margin-top: 20px; }}
        .status-change {{ background-color: #fff; padding: 15px; border-left: 4px solid {statusColor}; margin: 15px 0; }}
        .field {{ margin: 10px 0; }}
        .label {{ font-weight: bold; color: #333; }}
        .value {{ color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Destek Talebiniz Güncellendi</h2>
        </div>
        <div class='content'>
            <p>Sayın {ticket.FullName},</p>
            <p>#{ticket.TicketNumber} numaralı destek talebinizin durumu güncellendi.</p>
            
            <div class='status-change'>
                <div class='field'>
                    <span class='label'>Önceki Durum:</span>
                    <span class='value'>{oldStatus}</span>
                </div>
                <div class='field'>
                    <span class='label'>Yeni Durum:</span>
                    <span class='value'>{newStatus}</span>
                </div>
                {(string.IsNullOrEmpty(ticket.AssignedTo) ? "" : $@"
                <div class='field'>
                    <span class='label'>İlgilenen:</span>
                    <span class='value'>{ticket.AssignedTo}</span>
                </div>")}
            </div>
            
            <p>Talebinizle ilgili güncellemeleri takip edebilirsiniz.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Çözüm e-posta içeriği
        /// </summary>
        private string GenerateResolutionEmailBody(Ticket ticket)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f4f4f4; padding: 20px; margin-top: 20px; }}
        .success-box {{ background-color: #d4edda; border: 1px solid #c3e6cb; color: #155724; padding: 15px; margin: 15px 0; }}
        .field {{ margin: 10px 0; }}
        .label {{ font-weight: bold; color: #333; }}
        .value {{ color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>✅ Destek Talebiniz Çözüldü</h2>
        </div>
        <div class='content'>
            <p>Sayın {ticket.FullName},</p>
            
            <div class='success-box'>
                <p><strong>#{ticket.TicketNumber}</strong> numaralı destek talebiniz başarıyla çözüldü.</p>
            </div>
            
            <div class='field'>
                <span class='label'>Sorun:</span>
                <span class='value'>{ticket.Issue}</span>
            </div>
            <div class='field'>
                <span class='label'>Çözüm Süresi:</span>
                <span class='value'>{ticket.ResolutionTime?.TotalHours:F1} saat</span>
            </div>
            {(!string.IsNullOrEmpty(ticket.Resolution) ? $@"
            <div class='field'>
                <span class='label'>Çözüm:</span>
                <span class='value'>{ticket.Resolution}</span>
            </div>" : "")}
            
            <p>IT Destek ekibimizi tercih ettiğiniz için teşekkür ederiz.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}