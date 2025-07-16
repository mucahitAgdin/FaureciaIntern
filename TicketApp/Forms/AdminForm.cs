using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TicketApp.Models;
using TicketApp.Helpers;

namespace TicketApp.Forms
{
    public partial class AdminForm : Form
    {
        private List<Ticket> ticketList;
        private string _username;
        private ContextMenuStrip statusContextMenu;
        private DataGridView currentGridView; // Hangi grid'de sağ tık yapıldığını takip etmek için
        private List<string> supportTeam = new List<string> { "Burak Bey", "Kerem Bey", "Enver Bey", "Yavuz Bey" };

        public AdminForm(string username)
        {
            InitializeComponent();
            _username = username;
            InitializeContextMenu();
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
                        // Açıklama metnini güncelle
                        txtDescription.Text = ticket.Description;

                        // Seçilen ticket bilgilerini detaylı şekilde göster
                        string assignedInfo = string.IsNullOrEmpty(ticket.AssignedTo) ?
                            "Atanmamış" : ticket.AssignedTo;

                        lblSelectedTicket.Text = $"Seçilen Ticket: #{ticket.Id} - {ticket.Issue} " +
                                               $"(Durum: {ticket.Status}, Atanan: {assignedInfo})";
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
    }
}