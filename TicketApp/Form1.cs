using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicketApp.Models;  // Ticket modelini kullanmak için
using TicketApp.Helpers; // Yardımcı sınıflar (veritabanı, log vs.)
using System.Timers;     // Timer kullanımı için

namespace TicketApp
{
    public partial class Form1 : Form
    {
        // Belirli alanlara (bölgelere) göre tanımlı sorun listesi
        private Dictionary<string, List<string>> issueMap = new Dictionary<string, List<string>>();

        // Günlük temizlik işlemi için zamanlayıcı (her 24 saatte bir çalışacak)
        private System.Windows.Forms.Timer dailyCleanupTimer;

        public Form1()
        {
            InitializeComponent(); // UI bileşenlerini oluşturur (Form1.Designer.cs çağrısı)
            InitializeApp();       // Uygulamanın verilerini ve olaylarını kurar
            InitializeTimer();     // Zamanlayıcı başlatılır
        }

        /// <summary>
        /// Uygulama açıldığında yapılacak kurulumlar (veritabanı, comboboxlar vs.)
        /// </summary>
        private void InitializeApp()
        {
            try
            {
                // Veritabanı yoksa oluşturulacak, varsa açılacak
                DatabaseHelper.InitializeDatabase();

                // Kullanıcının seçim yapması için alanlar (UAP, FES) ekleniyor
                comboBoxArea.Items.AddRange(new string[] { "UAP-1", "UAP-2", "UAP-3", "UAP-4", "FES" });
                comboBoxArea.SelectedIndexChanged += ComboBoxArea_SelectedIndexChanged;

                // Her alan için özel tanımlı sorunlar
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
                Logger.Log(ex); // Hata loglanır
                MessageBox.Show("Başlatma hatası. Log dosyasına bakınız.", "Hata");
            }
        }

        /// <summary>
        /// Her 24 saatte bir çalışacak temizleme zamanlayıcısını başlatır.
        /// </summary>
        private void InitializeTimer()
        {
            dailyCleanupTimer = new System.Windows.Forms.Timer();
            dailyCleanupTimer.Interval = 86400000; // 24 saat = 24*60*60*1000 ms
            dailyCleanupTimer.Tick += DailyCleanupTimer_Tick;
            dailyCleanupTimer.Start(); // Zamanlayıcı başlar
        }

        /// <summary>
        /// Kullanıcı alan (UAP/FES) seçtiğinde uygun sorunları yükler
        /// </summary>
        private void ComboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selected = comboBoxArea.SelectedItem.ToString();
                comboBoxIssue.Items.Clear(); // Önceki seçenekler temizlenir

                if (issueMap.ContainsKey(selected))
                    comboBoxIssue.Items.AddRange(issueMap[selected].ToArray()); // Özel sorunlar

                comboBoxIssue.Items.AddRange(issueMap["GENEL"].ToArray()); // Herkese açık sorunlar
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Sorun listesi yüklenemedi.", "Hata");
            }
        }

        /// <summary>
        /// Form yüklendiğinde, var olan tüm ticketlar ekrana yazdırılır
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var tickets = DatabaseHelper.GetAllTickets(); // Tüm kayıtlar alınır
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

        /// <summary>
        /// Talep Gönder butonuna basıldığında yeni kayıt oluşturulur
        /// </summary>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Alan ve sorun tipi seçilmiş mi kontrolü
                if (comboBoxArea.SelectedItem == null || comboBoxIssue.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen alan ve sorun tipi seçin.", "Eksik Bilgi");
                    return;
                }

                string desc = textBoxDescription.Text.Trim();

                if (string.IsNullOrWhiteSpace(desc))
                {
                    MessageBox.Show("Lütfen açıklama alanını doldurun.", "Eksik Açıklama");
                    return;
                }

                if (desc.Length > 300)
                {
                    MessageBox.Show("Açıklama en fazla 300 karakter olabilir.", "Fazla Uzun");
                    return;
                }

                // Kullanıcıya onay sor
                DialogResult result = MessageBox.Show("Bu talebi göndermek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;

                // Yeni kayıt oluştur
                var ticket = new Ticket
                {
                    Area = comboBoxArea.SelectedItem.ToString(),
                    Issue = comboBoxIssue.SelectedItem.ToString(),
                    Description = desc,
                    CreatedAt = DateTime.Now
                };

                DatabaseHelper.InsertTicket(ticket); // Veritabanına ekle
                listBoxTickets.Items.Insert(0, $"[{ticket.CreatedAt}] {ticket.Area} - {ticket.Issue}");

                // Form temizlenir
                comboBoxIssue.SelectedIndex = -1;
                textBoxDescription.Clear();

                MessageBox.Show("Talep başarıyla gönderildi.", "Başarılı");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Talep gönderilirken hata oluştu.", "Hata");
            }
        }

        /// <summary>
        /// Günlük otomatik olarak çözümlenmiş ticket'ları arşivler ve siler
        /// </summary>
        private void DailyCleanupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var resolvedTickets = DatabaseHelper.GetResolvedTickets();
                if (resolvedTickets.Count > 0)
                {
                    var archiveFile = $"resolved_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    DatabaseHelper.ArchiveTickets(resolvedTickets, archiveFile);
                    DatabaseHelper.DeleteResolvedTickets();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        // Gereksiz event - kullanılmıyor ama Form1.Designer.cs'de bağlı
        private void comboBoxArea_SelectedIndexChanged_1(object sender, EventArgs e) { }
    }
}
