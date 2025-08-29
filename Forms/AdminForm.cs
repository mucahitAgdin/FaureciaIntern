using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using TicketApp.Models;
using TicketApp.Helpers;
using TicketApp.Services;

namespace TicketApp.Forms
{
    public partial class AdminForm : Form
    {
        #region Fields

        private List<Ticket> ticketList;
        private string _username;
        private ContextMenuStrip statusContextMenu;
        private DataGridView currentGridView;
        private List<string> supportTeam = new List<string> { "Burak Bey", "Kerem Bey", "Enver Bey", "Yavuz Bey" };

        // Servisler
        private RealtimeTicketService _realtimeService;
        private NotificationManager _notificationManager;

        #endregion

        #region Constructor

        public AdminForm(string username)
        {
            InitializeComponent();
            _username = username;
            InitializeContextMenu();
            InitializeServices();
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Servisleri başlatır
        /// </summary>
        private void InitializeServices()
        {
            // Bildirim yöneticisini oluştur
            _notificationManager = new NotificationManager(this);

            // Gerçek zamanlı servisi oluştur
            _realtimeService = new RealtimeTicketService();

            // Event'lere abone ol
            _realtimeService.NewTicketReceived += OnNewTicketReceived;
            _realtimeService.TicketUpdated += OnTicketUpdated;
            _realtimeService.DatabaseChanged += OnDatabaseChanged;
        }

        /// <summary>
        /// Durum değiştirme için context menu oluşturur
        /// </summary>
        private void InitializeContextMenu()
        {
            statusContextMenu = new ContextMenuStrip
            {
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#212121"),
                Font = new Font("Segoe UI", 9F)
            };

            // "İşleme Al" ana menüsü (mavi)
            var islemeAlSubMenu = new ToolStripMenuItem("İşleme Al →")
            {
                BackColor = ColorTranslator.FromHtml("#0066FF"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            foreach (var person in supportTeam)
            {
                var personItem = new ToolStripMenuItem(person)
                {
                    BackColor = Color.White,
                    ForeColor = ColorTranslator.FromHtml("#212121"),
                    Font = new Font("Segoe UI", 9F)
                };
                personItem.Click += (s, e) => AssignAndChangeStatus(person);
                islemeAlSubMenu.DropDownItems.Add(personItem);
            }

            // "Çözüldü" menüsü (yeşil)
            var cozulduItem = new ToolStripMenuItem("Çözüldü")
            {
                BackColor = ColorTranslator.FromHtml("#28A745"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            cozulduItem.Click += (s, e) => ChangeTicketStatus("çözüldü");

            // "Beklemede" menüsü (sarı)
            var beklemedeyeAlItem = new ToolStripMenuItem("Beklemede")
            {
                BackColor = ColorTranslator.FromHtml("#FFC107"),
                ForeColor = ColorTranslator.FromHtml("#212121"),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            beklemedeyeAlItem.Click += (s, e) => ChangeTicketStatus("beklemede");

            // Menü öğelerini ekle
            statusContextMenu.Items.AddRange(new ToolStripItem[] {
        islemeAlSubMenu,
        new ToolStripSeparator(),
        cozulduItem,
        beklemedeyeAlItem
    });
        }

        #endregion

        #region Form Events

        /// <summary>
        /// Form yüklendiğinde çalışır
        /// </summary>
        private void AdminForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Hoş geldiniz, {_username}";
            LoadTickets();

            // Servisi başlat
            try
            {
                _realtimeService.Start();
                UpdateStatusBar("Gerçek zamanlı takip aktif ✓");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                UpdateStatusBar("Gerçek zamanlı takip başlatılamadı ✗");
            }

            // Tooltip'ler ekle
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnRefresh, "Ticket listesini yenile");
            toolTip.SetToolTip(btnSettings, "Uygulama ayarları");
            toolTip.SetToolTip(btnLogout, "Çıkış yap");
        }

        /// <summary>
        /// Form kapanırken
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Servisleri temizle
            _realtimeService?.Dispose();
            _notificationManager?.Dispose();

            base.OnFormClosing(e);
        }

        #endregion

        #region Realtime Service Events

        /// <summary>
        /// Yeni ticket geldiğinde
        /// </summary>
        private void OnNewTicketReceived(object sender, TicketEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler<TicketEventArgs>(OnNewTicketReceived), sender, e);
                return;
            }

            // Bildirimi göster
            _notificationManager.ShowNewTicketNotification(e.Ticket);

            // Listeyi yenile
            LoadTickets();

            // Log
            Logger.Log($"Yeni ticket: #{e.Ticket.Id} - {e.Ticket.Issue}");
        }

        /// <summary>
        /// Ticket güncellendiğinde
        /// </summary>
        private void OnTicketUpdated(object sender, TicketEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler<TicketEventArgs>(OnTicketUpdated), sender, e);
                return;
            }

            // Bildirimi göster
            _notificationManager.ShowTicketUpdateNotification(e.Ticket, "güncellendi");

            // Listeyi yenile
            LoadTickets();
        }

        /// <summary>
        /// Veritabanı değiştiğinde
        /// </summary>
        private void OnDatabaseChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnDatabaseChanged), sender, e);
                return;
            }

            UpdateStatusBar($"Son güncelleme: {DateTime.Now:HH:mm:ss}");
        }

        #endregion

        #region Ticket Operations

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

                    // Çözüldü yapılıyorsa IsResolved'ı güncelle
                    if (newStatus == "çözüldü")
                    {
                        ticket.IsResolved = true;
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
        /// Ticket'ları kategorilere ayırarak yükler
        /// </summary>
        private void LoadTickets()
        {
            try
            {
                // Progress bar'ı göster
                if (progressBar != null)
                {
                    progressBar.Visible = true;
                    statusLabel.Text = "Ticketlar yükleniyor...";
                }

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

                // Progress bar'ı gizle
                if (progressBar != null)
                {
                    progressBar.Visible = false;
                    statusLabel.Text = $"Toplam {ticketList.Count} ticket yüklendi";
                    lastUpdateLabel.Text = $"Son güncelleme: {DateTime.Now:HH:mm:ss}";
                }
            }
            catch (Exception ex)
            {
                if (progressBar != null)
                    progressBar.Visible = false;

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

        #endregion

        #region Grid Events

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
                        // Açıklama metnini güncelle
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

        #endregion

        #region Button Events

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
        /// Yenile butonu
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTickets();
            MessageBox.Show("Ticket listesi yenilendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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

        #endregion

        #region Helper Methods

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

        /// <summary>
        /// Durum çubuğunu günceller
        /// </summary>
        private void UpdateStatusBar(string message)
        {
            if (statusLabel != null)
            {
                statusLabel.Text = message;
            }

            if (lastUpdateLabel != null)
            {
                lastUpdateLabel.Text = $"Son güncelleme: {DateTime.Now:HH:mm:ss}";
            }

            Logger.Log($"Status: {message}");
        }

        #endregion
    }
}