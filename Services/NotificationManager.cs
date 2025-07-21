// Services/NotificationManager.cs
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using TicketApp.Helpers;
using TicketApp.Models;

namespace TicketApp.Services
{
    /// <summary>
    /// Bildirim yönetimi için servis sınıfı
    /// </summary>
    public class NotificationManager : IDisposable
    {
        #region Fields

        private NotifyIcon _notifyIcon;
        private readonly Form _parentForm;
        private Timer _hideTimer;

        #endregion

        #region Properties

        /// <summary>
        /// Bildirim sesi aktif mi
        /// </summary>
        public bool SoundEnabled { get; set; } = true;

        /// <summary>
        /// Bildirim balonu gösterim süresi (ms)
        /// </summary>
        public int BalloonTipTimeout { get; set; } = 5000;

        /// <summary>
        /// Form başlığı yanıp sönme özelliği aktif mi
        /// </summary>
        public bool FlashWindowEnabled { get; set; } = true;

        #endregion

        #region Constructor

        public NotificationManager(Form parentForm)
        {
            _parentForm = parentForm ?? throw new ArgumentNullException(nameof(parentForm));
            InitializeNotifyIcon();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Yeni ticket bildirimi gösterir
        /// </summary>
        public void ShowNewTicketNotification(Ticket ticket)
        {
            string title = "🎫 Yeni Ticket!";
            string message = $"{ticket.FirstName} {ticket.LastName}\n" +
                           $"📍 {ticket.Area}/{ticket.SubArea}\n" +
                           $"⚠️ {ticket.Issue}";

            ShowNotification(title, message, ToolTipIcon.Info);

            if (FlashWindowEnabled)
            {
                FlashWindow("YENİ TICKET!");
            }
        }

        /// <summary>
        /// Çoklu ticket bildirimi gösterir
        /// </summary>
        public void ShowMultipleTicketsNotification(int count)
        {
            string title = $"🎫 {count} Yeni Ticket!";
            string message = $"{count} adet yeni destek talebi geldi.\n" +
                           "Detaylar için tıklayın.";

            ShowNotification(title, message, ToolTipIcon.Info);

            if (FlashWindowEnabled)
            {
                FlashWindow($"{count} YENİ TICKET!");
            }
        }

        /// <summary>
        /// Ticket güncelleme bildirimi gösterir
        /// </summary>
        public void ShowTicketUpdateNotification(Ticket ticket, string updateType)
        {
            string title = "📝 Ticket Güncellendi";
            string message = $"Ticket #{ticket.Id} {updateType}\n" +
                           $"📍 {ticket.Area} - {ticket.Issue}";

            ShowNotification(title, message, ToolTipIcon.Info);
        }

        /// <summary>
        /// Özel bildirim gösterir
        /// </summary>
        public void ShowCustomNotification(string title, string message, ToolTipIcon icon = ToolTipIcon.Info)
        {
            ShowNotification(title, message, icon);
        }

        /// <summary>
        /// Bildirim sesini çalar
        /// </summary>
        public void PlayNotificationSound()
        {
            if (!SoundEnabled) return;

            try
            {
                SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                Logger.Log($"Bildirim sesi çalma hatası: {ex.Message}");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// NotifyIcon'u başlatır
        /// </summary>
        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = _parentForm.Icon ?? SystemIcons.Information,
                Text = "IT Destek Sistemi",
                Visible = false
            };

            // Bildirime tıklandığında
            _notifyIcon.BalloonTipClicked += (s, e) =>
            {
                _parentForm.WindowState = FormWindowState.Normal;
                _parentForm.BringToFront();
                _parentForm.Activate();
            };

            // Sağ tık menüsü
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Göster", null, (s, e) =>
            {
                _parentForm.WindowState = FormWindowState.Normal;
                _parentForm.Show();
            });
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Çıkış", null, (s, e) =>
            {
                _parentForm.Close();
            });

            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Bildirim gösterir
        /// </summary>
        private void ShowNotification(string title, string message, ToolTipIcon icon)
        {
            try
            {
                _notifyIcon.Visible = true;
                _notifyIcon.BalloonTipTitle = title;
                _notifyIcon.BalloonTipText = message;
                _notifyIcon.BalloonTipIcon = icon;
                _notifyIcon.ShowBalloonTip(BalloonTipTimeout);

                if (SoundEnabled)
                {
                    PlayNotificationSound();
                }

                // Otomatik gizleme timer'ı
                StartHideTimer();
            }
            catch (Exception ex)
            {
                Logger.Log($"Bildirim gösterme hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Form başlığını yanıp söndürür
        /// </summary>
        private void FlashWindow(string flashText)
        {
            if (_parentForm.InvokeRequired)
            {
                _parentForm.BeginInvoke(new Action<string>(FlashWindow), flashText);
                return;
            }

            string originalTitle = _parentForm.Text;
            int flashCount = 0;

            var flashTimer = new Timer { Interval = 500 };
            flashTimer.Tick += (s, e) =>
            {
                if (flashCount >= 6)
                {
                    _parentForm.Text = originalTitle;
                    flashTimer.Stop();
                    flashTimer.Dispose();
                    return;
                }

                _parentForm.Text = flashCount % 2 == 0 ?
                    $"*** {flashText} ***" : originalTitle;

                flashCount++;
            };
            flashTimer.Start();
        }

        /// <summary>
        /// Gizleme timer'ını başlatır
        /// </summary>
        private void StartHideTimer()
        {
            _hideTimer?.Stop();
            _hideTimer?.Dispose();

            _hideTimer = new Timer { Interval = 10000 };
            _hideTimer.Tick += (s, e) =>
            {
                _notifyIcon.Visible = false;
                _hideTimer.Stop();
                _hideTimer.Dispose();
            };
            _hideTimer.Start();
        }

        #endregion

        #region IDisposable Implementation

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _hideTimer?.Dispose();
                _notifyIcon?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}