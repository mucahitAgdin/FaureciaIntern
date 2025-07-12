using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicketApp.Models;
using TicketApp.Helpers;

namespace TicketApp
{
    public partial class Form1 : Form
    {
        // Issues are categorized per area (UAP-1, FES, etc.)
        private Dictionary<string, List<string>> issueMap = new Dictionary<string, List<string>>();

        public Form1()
        {
            InitializeComponent();
            InitializeApp();
        }

        // Populate dropdowns and issue mappings
        private void InitializeApp()
        {
            try
            {
                DatabaseHelper.InitializeDatabase();

                comboBoxArea.Items.AddRange(new string[] { "UAP-1", "UAP-2", "UAP-3", "UAP-4", "FES" });
                comboBoxArea.SelectedIndexChanged += ComboBoxArea_SelectedIndexChanged;

                issueMap["GENEL"] = new List<string> {
                    "Bilgisayar açılmıyor",
                    "Mouse bozuldu",
                    "Klavye çalışmıyor",
                    "Yazıcıya kağıt sıkıştı",
                    "Yazıcıya internet gitmiyor",
                    "Yazıcı çalışmıyor"
                };

                issueMap["UAP-1"] = new List<string> { "SAP terminal donuyor", "Barkod yazıcı hatası" };
                issueMap["UAP-2"] = new List<string> { "IP erişilemiyor" };
                issueMap["FES"] = new List<string> { "PLC bağlantısı kesildi", "Etiket yazıcı çevrimdışı" };
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Başlatma hatası. Log dosyasına bakınız.", "Hata");
            }
        }

        // When area is changed, update issues accordingly
        private void ComboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selected = comboBoxArea.SelectedItem.ToString();
                comboBoxIssue.Items.Clear();

                if (issueMap.ContainsKey(selected))
                    comboBoxIssue.Items.AddRange(issueMap[selected].ToArray());

                comboBoxIssue.Items.AddRange(issueMap["GENEL"].ToArray());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Sorun listesi yüklenemedi.", "Hata");
            }
        }

        // When submit button clicked, create ticket
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcı alanı seçmiş mi?
                if (comboBoxArea.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir alan (UAP/FES) seçin.", "Eksik Alan");
                    return;
                }

                // Kullanıcı sorunu seçmiş mi?
                if (comboBoxIssue.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir sorun tipi seçin.", "Eksik Sorun");
                    return;
                }

                string desc = textBoxDescription.Text.Trim();

                // Açıklama boş mu?
                if (string.IsNullOrWhiteSpace(desc))
                {
                    MessageBox.Show("Lütfen açıklama alanını doldurun.", "Eksik Açıklama");
                    return;
                }

                // Açıklama çok uzun mu?
                if (desc.Length > 300)
                {
                    MessageBox.Show("Açıklama en fazla 300 karakter olabilir.", "Aşırı Uzun Açıklama");
                    return;
                }

                // Her şey doğruysa kayıt yap
                var ticket = new Ticket
                {
                    Area = comboBoxArea.SelectedItem.ToString(),
                    Issue = comboBoxIssue.SelectedItem.ToString(),
                    Description = desc,
                    CreatedAt = DateTime.Now
                };

                DatabaseHelper.InsertTicket(ticket);

                listBoxTickets.Items.Insert(0, $"[{ticket.CreatedAt}] {ticket.Area} - {ticket.Issue}");
                comboBoxIssue.SelectedIndex = -1;
                textBoxDescription.Clear();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Talep gönderilirken hata oluştu.", "Hata");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var tickets = DatabaseHelper.GetAllTickets();
                foreach (var t in tickets)
                {
                    listBoxTickets.Items.Add($"[{t.CreatedAt}] {t.Area} - {t.Issue}");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Kayıtlar yüklenemedi.", "Hata");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTickets.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz talebi seçin.", "Uyarı");
                    return;
                }

                string selectedText = listBoxTickets.SelectedItem.ToString();

                // [2025-07-12 15:31:48] UAP-1 - Mouse bozuldu
                // Tarihi çekiyoruz
                int start = selectedText.IndexOf('[') + 1;
                int end = selectedText.IndexOf(']');
                string dateStr = selectedText.Substring(start, end - start);

                if (DateTime.TryParse(dateStr, out DateTime createdAt))
                {
                    DialogResult result = MessageBox.Show("Seçili talep silinsin mi?", "Onay", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        DatabaseHelper.DeleteTicket(createdAt);
                        listBoxTickets.Items.Remove(listBoxTickets.SelectedItem);
                        MessageBox.Show("Talep başarıyla silindi.");
                    }
                }
                else
                {
                    MessageBox.Show("Tarih ayrıştırılamadı, silme işlemi başarısız oldu.", "Hata");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Silme sırasında hata oluştu.", "Hata");
            }
        }


        private void comboBoxArea_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
