using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TicketApp.Helpers;
using TicketApp.Models;
using System.Media;
using System.Text;

namespace TicketApp.Forms
{
    public partial class AdminForm : Form
    {
        private List<Ticket> ticketList;
        private string _username;
        private ContextMenuStrip statusContextMenu;
        private DataGridView currentGridView; // Hangi grid'de sağ tık yapıldığını takip etmek için
        private List<string> supportTeam = new List<string> { "Burak Bey", "Kerem Bey", "Enver Bey", "Yavuz Bey" };
        
        // Gerçek zamanlı güncelleme için
        private FileSystemWatcher dbWatcher;
        private DateTime lastDbUpdate = DateTime.Now;
        private System.Windows.Forms.Timer debounceTimer;
        private NotifyIcon notifyIcon;

        public AdminForm(string username)
        {
            InitializeComponent();
            _username = username;
            InitializeContextMenu();

            //Event bağlantılarını ekle
            this.Load += AdminForm_Load;
        }

        /// <summary>
        /// Durum değiştirme için context menu oluşturur
        /// </summary>
        private void InitializeContextMenu()
        {
            statusContextMenu = new ContextMenuStrip();

            // "İşleme Al" ana menüsü
            var islemeAlSubMenu = new ToolStripMenuItem("İşleme Al →");
            islemeAlSubMenu.BackColor = Color.LightBlue;
            islemeAlSubMenu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Her destek ekibi üyesi için alt menü oluştur
            foreach (var person in supportTeam)
            {
                var personItem = new ToolStripMenuItem(person);
                personItem.BackColor = Color.White;
                personItem.ForeColor = Color.Black;
                personItem.Click += (s, e) => AssignAndChangeStatus(person);
                islemeAlSubMenu.DropDownItems.Add(personItem);
            }

            // "Çözüldü" menüsü
            var cozulduItem = new ToolStripMenuItem("Çözüldü");
            cozulduItem.Click += (s, e) => ChangeTicketStatus("çözüldü");
            cozulduItem.BackColor = Color.LightGreen;
            cozulduItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // "Beklemede" menüsü (isteğe bağlı)
            var beklemedeyeAlItem = new ToolStripMenuItem("Beklemede");
            beklemedeyeAlItem.Click += (s, e) => ChangeTicketStatus("beklemede");
            beklemedeyeAlItem.BackColor = Color.LightYellow;
            beklemedeyeAlItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Menü öğelerini ekle
            statusContextMenu.Items.AddRange(new ToolStripItem[] {
                islemeAlSubMenu,
                new ToolStripSeparator(),
                cozulduItem,
                beklemedeyeAlItem
            });

            // Context menü stilini ayarla
            statusContextMenu.BackColor = Color.White;
            statusContextMenu.ForeColor = Color.Black;
        }

        /// <summary>
        /// Ticket'ı belirli bir kişiye atar ve durumunu işlemde olarak değiştirir
        /// </summary>
        private void AssignAndChangeStatus(string assignedPerson)
        {
            if (currentGridView == null || currentGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir ticket seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int ticketId = (int)currentGridView.SelectedRows[0].Cells[0].Value;
                var ticket = ticketList.FirstOrDefault(t => t.Id == ticketId);

                if (ticket != null)
                {
                    // Önceki durumu kaydet
                    string previousStatus = ticket.Status;
                    string previousAssignee = ticket.AssignedTo ?? "Atanmamış";

                    // Atama yap ve durumu güncelle
                    ticket.AssignedTo = assignedPerson;
                    ticket.Status = "işlemde";

                    // Veritabanını güncelle
                    bool updateSuccess = DatabaseHelper.UpdateTicketStatus(ticket);

                    if (updateSuccess)
                    {
                        // Grid'leri yenile
                        LoadTickets();

                        // Başarı mesajı göster
                        MessageBox.Show(
                            $"✅ Ticket başarıyla atandı!\n\n" +
                            $"🎫 Ticket ID: #{ticket.Id}\n" +
                            $"📝 Konu: {ticket.Issue}\n" +
                            $"👤 Atanan Kişi: {assignedPerson}\n" +
                            $"📊 Durum: İşlemde\n" +
                            $"📧 Otomatik bildirim gönderildi.",
                            "Atama Başarılı",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        // Log kaydı
                        Logger.Log($"Ticket #{ticket.Id} {assignedPerson} tarafından işleme alındı. Önceki durum: {previousStatus}");
                    }
                    else
                    {
                        // Hata durumunda eski değerleri geri yükle
                        ticket.AssignedTo = previousAssignee == "Atanmamış" ? null : previousAssignee;
                        ticket.Status = previousStatus;

                        MessageBox.Show("Veritabanı güncellenirken hata oluştu. Lütfen tekrar deneyin.",
                                      "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ticket bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show($"Atama işlemi sırasında hata oluştu:\n{ex.Message}",
                              "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Seçilen ticket'ın durumunu değiştirir
        /// </summary>
        private void ChangeTicketStatus(string newStatus)
        {
            if (currentGridView == null || currentGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir ticket seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int ticketId = (int)currentGridView.SelectedRows[0].Cells[0].Value;
                var ticket = ticketList.FirstOrDefault(t => t.Id == ticketId);

                if (ticket != null)
                {
                    string previousStatus = ticket.Status;
                    ticket.Status = newStatus;

                    // Eğer beklemede yapılıyorsa atamayı kaldır
                    if (newStatus == "beklemede")
                    {
                        ticket.AssignedTo = null;
                    }

                    bool updateSuccess = DatabaseHelper.UpdateTicketStatus(ticket);

                    if (updateSuccess)
                    {
                        LoadTickets();

                        // Başarı mesajı
                        string statusText = newStatus == "beklemede" ? "Beklemede" :
                                           newStatus == "işlemde" ? "İşlemde" : "Çözüldü";

                        string emoji = newStatus == "beklemede" ? "⏳" :
                                      newStatus == "işlemde" ? "⚙️" : "✅";

                        MessageBox.Show(
                            $"{emoji} Ticket durumu güncellendi!\n\n" +
                            $"🎫 Ticket ID: #{ticket.Id}\n" +
                            $"📝 Konu: {ticket.Issue}\n" +
                            $"📊 Yeni Durum: {statusText}",
                            "Güncelleme Başarılı",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        Logger.Log($"Ticket #{ticket.Id} durumu '{previousStatus}' -> '{newStatus}' olarak güncellendi.");
                    }
                    else
                    {
                        // Hata durumunda eski değeri geri yükle
                        ticket.Status = previousStatus;
                        MessageBox.Show("Veritabanı güncellenirken hata oluştu. Lütfen tekrar deneyin.",
                                      "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ticket bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show($"Ticket durumu güncellenirken hata oluştu:\n{ex.Message}",
                              "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Form yüklendiğinde çalışır
        /// </summary>
        private void AdminForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Hoş geldiniz, {_username}";
            LoadTickets();

            // YENİ SATIR
            InitializeDatabaseWatcher();

            // Tooltip'ler ekle
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnRefresh, "Ticket listesini yenile");
            toolTip.SetToolTip(btnSettings, "Uygulama ayarları");
            toolTip.SetToolTip(btnLogout, "Çıkış yap");
        }

        /// <summary>
        /// Ticket'ları kategorilere ayırarak yükler
        /// </summary>
        private void LoadTickets()
        {
            try
            {
                ticketList = DatabaseHelper.GetAllTickets();

                // Kategorilere ayır
                var bekleyenTickets = ticketList.Where(t => t.Status == "beklemede").ToList();
                var islemedeTickets = ticketList.Where(t => t.Status == "işlemde").ToList();
                var cozulenTickets = ticketList.Where(t => t.Status == "çözüldü").ToList();

                // Grid'leri temizle
                dgvBekleyen.Rows.Clear();
                dgvIslemde.Rows.Clear();
                dgvCozulen.Rows.Clear();

                // Bekleyen ticket'ları yükle
                foreach (var ticket in bekleyenTickets)
                {
                    dgvBekleyen.Rows.Add(
                        ticket.Id,
                        ticket.Area,
                        ticket.Issue,
                        ticket.CreatedAt.ToString("dd/MM/yyyy HH:mm")
                    );
                }

                // İşlemdeki ticket'ları yükle (atanan kişi bilgisi ile)
                foreach (var ticket in islemedeTickets)
                {
                    string displayText = ticket.Issue;
                    if (!string.IsNullOrEmpty(ticket.AssignedTo))
                    {
                        displayText += $" ({ticket.AssignedTo})";
                    }

                    dgvIslemde.Rows.Add(
                        ticket.Id,
                        ticket.Area,
                        displayText,
                        ticket.CreatedAt.ToString("dd/MM/yyyy HH:mm")
                    );
                }

                // Çözülen ticket'ları yükle
                foreach (var ticket in cozulenTickets)
                {
                    dgvCozulen.Rows.Add(
                        ticket.Id,
                        ticket.Area,
                        ticket.Issue,
                        ticket.CreatedAt.ToString("dd/MM/yyyy HH:mm")
                    );
                }

                // Sayıları güncelle
                lblBekleyenCount.Text = $"Bekleyen: {bekleyenTickets.Count}";
                lblIslemdeCount.Text = $"İşlemde: {islemedeTickets.Count}";
                lblCozulenCount.Text = $"Çözülen: {cozulenTickets.Count}";
                lblTotalCount.Text = $"Toplam: {ticketList.Count}";

                // Grid renklerini ayarla
                UpdateGridColors();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show($"Ticket'lar yüklenirken hata oluştu:\n{ex.Message}",
                              "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Grid renklerini günceller
        /// </summary>
        private void UpdateGridColors()
        {
            // Bekleyen ticket'lar için sarı ton
            foreach (DataGridViewRow row in dgvBekleyen.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 252, 240);
            }

            // İşlemdeki ticket'lar için mavi ton
            foreach (DataGridViewRow row in dgvIslemde.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            }

            // Çözülen ticket'lar için yeşil ton
            foreach (DataGridViewRow row in dgvCozulen.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
            }
        }

        /// <summary>
        /// Grid seçimi değiştiğinde açıklama kutusunu günceller
        /// </summary>
        private void GridView_SelectionChanged(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid.SelectedRows.Count > 0)
            {
                try
                {
                    int ticketId = (int)grid.SelectedRows[0].Cells[0].Value;
                    var ticket = ticketList.FirstOrDefault(t => t.Id == ticketId);

                    if (ticket != null)
                    {
                        // ESKİ KOD YERİNE:
                        txtDescription.Text = BuildTicketDetails(ticket);
                        UpdateSelectedTicketLabel(ticket);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    txtDescription.Text = "Ticket bilgileri yüklenirken hata oluştu.";
                    lblSelectedTicket.Text = "Seçilen Ticket: Hata";
                }
            }
            else
            {
                txtDescription.Text = "Bir ticket seçin...";
                lblSelectedTicket.Text = "Seçilen Ticket: Yok";
            }
        }

        /// <summary>
        /// Grid'e sağ tık yapıldığında context menu gösterir
        /// </summary>
        private void GridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var grid = sender as DataGridView;
                var hit = grid.HitTest(e.X, e.Y);

                if (hit.RowIndex >= 0)
                {
                    grid.ClearSelection();
                    grid.Rows[hit.RowIndex].Selected = true;
                    currentGridView = grid;

                    // Context menüyü göster
                    statusContextMenu.Show(grid, e.Location);
                }
            }
        }

        /// <summary>
        /// Ayarlar butonu
        /// </summary>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                var settingsForm = new SettingsForm();
                settingsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Ayarlar formu açılamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Çıkış butonu
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?",
                                       "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Veritabanı değişikliklerini takip eden FileSystemWatcher'ı başlatır
        /// </summary>
        private void InitializeDatabaseWatcher()
        {
            try
            {
                // Veritabanı yolunu al
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tickets.db");
                string dbDirectory = Path.GetDirectoryName(dbPath);
                string dbFileName = Path.GetFileName(dbPath);

                // FileSystemWatcher oluştur
                dbWatcher = new FileSystemWatcher(dbDirectory);
                dbWatcher.Filter = dbFileName;

                // Neleri izleyeceğini belirt
                dbWatcher.NotifyFilter = NotifyFilters.LastWrite
                                       | NotifyFilters.Size
                                       | NotifyFilters.LastAccess;

                // Event handler'ları bağla
                dbWatcher.Changed += OnDatabaseChanged;
                dbWatcher.Created += OnDatabaseChanged;

                // İzlemeyi başlat
                dbWatcher.EnableRaisingEvents = true;

                Logger.Log("Veritabanı izleyici başlatıldı.");

                // Debounce timer'ı oluştur (çoklu tetiklemeyi önlemek için)
                debounceTimer = new System.Windows.Forms.Timer();
                debounceTimer.Interval = 500; // 500ms bekle
                debounceTimer.Tick += DebounceTimer_Tick;
            }
            catch (Exception ex)
            {
                Logger.Log($"FileSystemWatcher başlatma hatası: {ex.Message}");
                MessageBox.Show(
                    "Otomatik güncelleme sistemi başlatılamadı.\n" +
                    "Manuel yenileme yapmanız gerekebilir.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        /// <summary>
        /// Veritabanı dosyası değiştiğinde tetiklenir
        /// </summary>
        private void OnDatabaseChanged(object sender, FileSystemEventArgs e)
        {
            // Kendi yazdığımız değişiklikleri yoksay
            if (DateTime.Now.Subtract(lastDbUpdate).TotalMilliseconds < 1000)
                return;

            // Timer zaten çalışıyorsa durdur ve yeniden başlat (debouncing)
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        /// <summary>
        /// Debounce timer tetiklendiğinde (gerçek güncelleme burada)
        /// </summary>
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();

            // Ana thread'de çalıştır (UI güncellemesi için gerekli)
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => RefreshTicketsWithNotification()));
            }
            else
            {
                RefreshTicketsWithNotification();
            }
        }

        /// <summary>
        /// Ticketları yeniler ve yeni ticket varsa bildirim gösterir
        /// </summary>
        private void RefreshTicketsWithNotification()
        {
            try
            {
                // Mevcut ticket sayısını kaydet
                int oldBekleyenCount = ticketList?.Where(t => t.Status == "beklemede").Count() ?? 0;

                // Ticketları yenile
                LoadTickets();

                // Yeni bekleyen ticket var mı kontrol et
                int newBekleyenCount = ticketList.Where(t => t.Status == "beklemede").Count();

                if (newBekleyenCount > oldBekleyenCount)
                {
                    int newTicketCount = newBekleyenCount - oldBekleyenCount;

                    // Bildirim göster
                    ShowNewTicketNotification(newTicketCount);

                    // Ses çal
                    PlayNotificationSound();

                    // Form başlığını güncelle (yanıp sönme efekti)
                    FlashWindowTitle(newTicketCount);
                }

                // Son güncelleme zamanını kaydet
                lastDbUpdate = DateTime.Now;

                // Durum çubuğunu güncelle (varsa)
                UpdateStatusBar($"Otomatik güncelleme: {DateTime.Now:HH:mm:ss}");
            }
            catch (Exception ex)
            {
                Logger.Log($"Otomatik yenileme hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Windows bildirim balonu gösterir
        /// </summary>
        private void ShowNewTicketNotification(int count)
        {
            try
            {
                // NotifyIcon yoksa oluştur
                if (notifyIcon == null)
                {
                    notifyIcon = new NotifyIcon();
                    notifyIcon.Icon = this.Icon ?? SystemIcons.Information;
                    notifyIcon.Text = "IT Destek Sistemi";
                    notifyIcon.BalloonTipClicked += (s, e) =>
                    {
                        // Bildirme tıklandığında formu öne getir
                        this.WindowState = FormWindowState.Normal;
                        this.BringToFront();
                        this.Activate();
                    };
                }

                notifyIcon.Visible = true;

                // En son gelen ticket bilgisini al
                var latestTicket = ticketList
                    .Where(t => t.Status == "beklemede")
                    .OrderByDescending(t => t.CreatedAt)
                    .FirstOrDefault();

                string title = count == 1 ? "Yeni Ticket!" : $"{count} Yeni Ticket!";
                string text = latestTicket != null ?
                    $"{latestTicket.FullName} - {latestTicket.Issue}\n{latestTicket.Area}/{latestTicket.SubArea}" :
                    "Yeni destek talebi geldi.";

                notifyIcon.BalloonTipTitle = title;
                notifyIcon.BalloonTipText = text;
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.ShowBalloonTip(5000); // 5 saniye göster

                // 10 saniye sonra gizle
                var hideTimer = new System.Windows.Forms.Timer();
                hideTimer.Interval = 10000;
                hideTimer.Tick += (s, e) =>
                {
                    notifyIcon.Visible = false;
                    hideTimer.Stop();
                    hideTimer.Dispose();
                };
                hideTimer.Start();
            }
            catch (Exception ex)
            {
                Logger.Log($"Bildirim gösterme hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Bildirim sesi çalar
        /// </summary>
        private void PlayNotificationSound()
        {
            try
            {
                // Windows varsayılan bildirim sesi
                System.Media.SystemSounds.Asterisk.Play();

                // Veya özel ses dosyası (isteğe bağlı)
                // string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notification.wav");
                // if (File.Exists(soundPath))
                // {
                //     using (var player = new System.Media.SoundPlayer(soundPath))
                //     {
                //         player.Play();
                //     }
                // }
            }
            catch (Exception ex)
            {
                Logger.Log($"Ses çalma hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Form başlığında yanıp sönme efekti
        /// </summary>
        private void FlashWindowTitle(int newTicketCount)
        {
            string originalTitle = this.Text;
            int flashCount = 0;

            var flashTimer = new System.Windows.Forms.Timer();
            flashTimer.Interval = 500;
            flashTimer.Tick += (s, e) =>
            {
                if (flashCount >= 6) // 3 kez yanıp sönsün
                {
                    this.Text = originalTitle;
                    flashTimer.Stop();
                    flashTimer.Dispose();
                    return;
                }

                if (flashCount % 2 == 0)
                {
                    this.Text = $"*** {newTicketCount} YENİ TICKET! ***";
                }
                else
                {
                    this.Text = originalTitle;
                }

                flashCount++;
            };
            flashTimer.Start();
        }

        /// <summary>
        /// Durum çubuğunu günceller
        /// </summary>
        private void UpdateStatusBar(string message)
        {
            // Eğer StatusStrip kullanıyorsanız
            if (statusLabel != null)
            {
                statusLabel.Text = message;
            }
        }



        /// <summary>
        /// Yenile butonu
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTickets();
            MessageBox.Show("Ticket listesi yenilendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region Ticket Detail Methods

        /// <summary>
        /// Ticket detaylarını formatlar
        /// </summary>
        private string BuildTicketDetails(Ticket ticket)
        {
            var sb = new StringBuilder();

            // Kişi bilgileri
            sb.AppendLine("👤 GÖNDERİCİ BİLGİLERİ");
            sb.AppendLine($"Ad Soyad: {ticket.FirstName} {ticket.LastName}");
            sb.AppendLine($"Telefon: {ticket.PhoneNumber}");
            sb.AppendLine();

            // Konum bilgileri
            sb.AppendLine("📍 KONUM BİLGİLERİ");
            sb.AppendLine($"Ana Alan: {ticket.Area}");
            sb.AppendLine($"Alt Alan: {ticket.SubArea}");
            sb.AppendLine();

            // Sorun detayları
            sb.AppendLine("⚠️ SORUN DETAYLARI");
            sb.AppendLine($"Sorun Tipi: {ticket.Issue}");
            sb.AppendLine($"Oluşturma Tarihi: {ticket.CreatedAt:dd.MM.yyyy HH:mm}");
            sb.AppendLine();

            // Durum bilgileri
            sb.AppendLine("📊 DURUM BİLGİLERİ");
            sb.AppendLine($"Mevcut Durum: {GetStatusText(ticket.Status)}");

            if (!string.IsNullOrEmpty(ticket.AssignedTo))
            {
                sb.AppendLine($"Atanan Kişi: {ticket.AssignedTo}");
            }

            sb.AppendLine();
            sb.AppendLine("📝 AÇIKLAMA");
            sb.AppendLine(new string('-', 40));
            sb.AppendLine(ticket.Description);

            return sb.ToString();
        }

        /// <summary>
        /// Seçilen ticket etiketini günceller
        /// </summary>
        private void UpdateSelectedTicketLabel(Ticket ticket)
        {
            string statusIcon = GetStatusIcon(ticket.Status);
            string assignedInfo = string.IsNullOrEmpty(ticket.AssignedTo) ?
                "Atanmamış" : ticket.AssignedTo;

            // lblSelectedTicket'ın var olduğunu kontrol et
            if (lblSelectedTicket != null)
            {
                lblSelectedTicket.Text = $"{statusIcon} Ticket #{ticket.Id} - {ticket.FirstName} {ticket.LastName} - {ticket.Area}/{ticket.SubArea}";
            }
        }

        /// <summary>
        /// Status metnini döndürür
        /// </summary>
        private string GetStatusText(string status)
        {
            switch (status?.ToLower())
            {
                case "beklemede": return "⏳ Beklemede";
                case "işlemde": return "⚙️ İşlemde";
                case "çözüldü": return "✅ Çözüldü";
                case "reddedildi": return "❌ Reddedildi";
                default: return "❓ Bilinmiyor";
            }
        }

        /// <summary>
        /// Status ikonunu döndürür
        /// </summary>
        private string GetStatusIcon(string status)
        {
            switch (status?.ToLower())
            {
                case "beklemede": return "⏳";
                case "işlemde": return "⚙️";
                case "çözüldü": return "✅";
                case "reddedildi": return "❌";
                default: return "❓";
            }
        }

        #endregion

        /// <summary>
        /// Hızlı işlem butonları
        /// </summary>
        private void btnMarkInProgress_Click(object sender, EventArgs e)
        {
            if (dgvBekleyen.SelectedRows.Count > 0)
            {
                currentGridView = dgvBekleyen;

                // Atama yapmadan sadece durum değiştir
                var result = MessageBox.Show(
                    "Ticket'ı işleme almak istiyor musunuz?\n\n" +
                    "• Evet: Sadece durumu işleme al\n" +
                    "• Hayır: Sağ tık menüsünden kişi seçin",
                    "İşlem Seçin",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    ChangeTicketStatus("işlemde");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bekleyen ticket'lardan birini seçin.", "Uyarı",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMarkCompleted_Click(object sender, EventArgs e)
        {
            if (dgvIslemde.SelectedRows.Count > 0)
            {
                currentGridView = dgvIslemde;

                var result = MessageBox.Show(
                    "Ticket'ı tamamlandı olarak işaretlemek istediğinizden emin misiniz?",
                    "Tamamlama Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    ChangeTicketStatus("çözüldü");
                }
            }
            else
            {
                MessageBox.Show("Lütfen işlemdeki ticket'lardan birini seçin.", "Uyarı",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvCozulen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Çözülen ticket'lar için özel işlemler buraya eklenebilir
        }
    }

}
