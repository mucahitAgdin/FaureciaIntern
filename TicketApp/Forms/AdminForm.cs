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

            var bekleyenItem = new ToolStripMenuItem("Beklemede");
            bekleyenItem.Click += (s, e) => ChangeTicketStatus("beklemede");
            bekleyenItem.BackColor = Color.LightYellow;

            var islemedeItem = new ToolStripMenuItem("İşlemde");
            islemedeItem.Click += (s, e) => ChangeTicketStatus("işlemde");
            islemedeItem.BackColor = Color.LightBlue;

            var cozulduItem = new ToolStripMenuItem("Çözüldü");
            cozulduItem.Click += (s, e) => ChangeTicketStatus("çözüldü");
            cozulduItem.BackColor = Color.LightGreen;

            statusContextMenu.Items.AddRange(new ToolStripItem[] { bekleyenItem, islemedeItem, cozulduItem });
        }

        /// <summary>
        /// Seçilen ticket'ın durumunu değiştirir
        /// </summary>
        private void ChangeTicketStatus(string newStatus)
        {
            if (currentGridView == null || currentGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir ticket seçin.");
                return;
            }

            try
            {
                int ticketId = (int)currentGridView.SelectedRows[0].Cells[0].Value;
                var ticket = ticketList.FirstOrDefault(t => t.Id == ticketId);

                if (ticket != null)
                {
                    ticket.Status = newStatus;
                    DatabaseHelper.UpdateTicketStatus(ticket);
                    LoadTickets();

                    // Başarı mesajı
                    string statusText = newStatus == "beklemede" ? "Beklemede" :
                                       newStatus == "işlemde" ? "İşlemde" : "Çözüldü";
                    MessageBox.Show($"Ticket durumu '{statusText}' olarak güncellendi.", "Başarılı",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Ticket durumu güncellenirken hata oluştu.", "Hata",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Form yüklendiğinde çalışır
        /// </summary>
        private void AdminForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Hoş geldiniz, {_username}";
            LoadTickets();
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
                    dgvBekleyen.Rows.Add(ticket.Id, ticket.Area, ticket.Issue, ticket.CreatedAt.ToString("dd/MM/yyyy"));
                }

                // İşlemdeki ticket'ları yükle
                foreach (var ticket in islemedeTickets)
                {
                    dgvIslemde.Rows.Add(ticket.Id, ticket.Area, ticket.Issue, ticket.CreatedAt.ToString("dd/MM/yyyy"));
                }

                // Çözülen ticket'ları yükle
                foreach (var ticket in cozulenTickets)
                {
                    dgvCozulen.Rows.Add(ticket.Id, ticket.Area, ticket.Issue, ticket.CreatedAt.ToString("dd/MM/yyyy"));
                }

                // Sayıları güncelle
                lblBekleyenCount.Text = $"Bekleyen: {bekleyenTickets.Count}";
                lblIslemdeCount.Text = $"İşlemde: {islemedeTickets.Count}";
                lblCozulenCount.Text = $"Çözülen: {cozulenTickets.Count}";
                lblTotalCount.Text = $"Toplam: {ticketList.Count}";
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Ticket'lar yüklenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        txtDescription.Text = ticket.Description;
                        // Seçilen ticket bilgilerini göster
                        lblSelectedTicket.Text = $"Seçilen Ticket: #{ticket.Id} - {ticket.Issue}";
                    }
                }
                catch
                {
                    txtDescription.Text = "";
                    lblSelectedTicket.Text = "Seçilen Ticket: Yok";
                }
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
                    statusContextMenu.Show(grid, e.Location);
                }
            }
        }

        /// <summary>
        /// Ayarlar butonu
        /// </summary>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        /// <summary>
        /// Çıkış butonu
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Yenile butonu
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }

        /// <summary>
        /// Hızlı işlem butonları
        /// </summary>
        private void btnMarkInProgress_Click(object sender, EventArgs e)
        {
            if (dgvBekleyen.SelectedRows.Count > 0)
            {
                currentGridView = dgvBekleyen;
                ChangeTicketStatus("işlemde");
            }
            else
            {
                MessageBox.Show("Lütfen bekleyen ticket'lardan birini seçin.", "Bilgi");
            }
        }

        private void btnMarkCompleted_Click(object sender, EventArgs e)
        {
            if (dgvIslemde.SelectedRows.Count > 0)
            {
                currentGridView = dgvIslemde;
                ChangeTicketStatus("çözüldü");
            }
            else
            {
                MessageBox.Show("Lütfen işlemdeki ticket'lardan birini seçin.", "Bilgi");
            }
        }
    }
}