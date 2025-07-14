using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicketApp.Models;
using TicketApp.Helpers;

namespace TicketApp.Forms
{
    public partial class AdminForm : Form
    {
        private List<Ticket> ticketList;

        private string _username; // Kullanıcı adını saklayacak alan
        public AdminForm(string username)
        {
            InitializeComponent();
            _username = username;

        }

        /// <summary>
        /// Form yüklendiğinde ticket listesi gösterilir
        /// </summary>
        private void AdminForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Hoş geldiniz, {_username}";
            LoadTickets();
        }

        /// <summary>
        /// Tüm ticket'ları veritabanından alır ve tabloya yükler
        /// </summary>
        private void LoadTickets()
        {
            try
            {
                ticketList = DatabaseHelper.GetAllTickets();
                dgvTickets.Rows.Clear();

                foreach (var ticket in ticketList)
                {
                    dgvTickets.Rows.Add(ticket.Id, ticket.Area, ticket.Issue, ticket.Status, ticket.CreatedAt);
                }

                lblTicketCount.Text = $"Toplam Kayıtlı Ticket: {ticketList.Count}";
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Ticket'lar yüklenemedi.", "Hata");
            }
        }

        /// <summary>
        /// Ayarlar butonuna basıldığında çalışır
        /// </summary>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        /// <summary>
        /// Çıkış butonuna basıldığında form kapanır
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close(); // Gerekirse LoginForm'a geri dönüş sağlanabilir
        }

        /// <summary>
        /// DataGridView üzerinden satır seçilince açıklama kutusunu doldurur
        /// </summary>
        private void dgvTickets_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTickets.SelectedRows.Count > 0)
            {
                int index = dgvTickets.SelectedRows[0].Index;
                if (index >= 0 && index < ticketList.Count)
                {
                    txtDescription.Text = ticketList[index].Description;
                }
            }
        }

        /// <summary>
        /// Seçilen ticket'ı "çözüldü" olarak işaretler
        /// </summary>
        private void btnMarkCompleted_Click(object sender, EventArgs e)
        {
            if (dgvTickets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir ticket seçin.");
                return;
            }

            int index = dgvTickets.SelectedRows[0].Index;
            var selectedTicket = ticketList[index];
            selectedTicket.Status = "çözüldü";
            DatabaseHelper.UpdateTicketStatus(selectedTicket);

            LoadTickets();
        }

        /// <summary>
        /// Yenile butonuna basıldığında ticket'lar tekrar yüklenir
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
