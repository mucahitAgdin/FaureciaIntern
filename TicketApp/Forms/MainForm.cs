//TicketApp/Forms/MainForm.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicketApp.Models;       // Ticket modelini kullanmak için
using TicketApp.Helpers;      // Veritabanı ve log işlemleri için yardımcı sınıf
using System.Timers;          // Timer sınıfı için

namespace TicketApp.Forms
{
    public partial class MainForm : Form
    {
        // Alanlara (bölgelere) göre sorun türlerini tutan sözlük yapısı
        private Dictionary<string, List<string>> issueMap = new Dictionary<string, List<string>>();

        // Her 24 saatte bir arşivleme ve silme işlemi yapacak olan zamanlayıcı
        private System.Windows.Forms.Timer dailyCleanupTimer;

        // Yapıcı metod: Form ilk oluşturulduğunda çalışır
        public MainForm()
        {
            InitializeComponent();      // Form bileşenlerini (butonlar, textboxlar vs) başlat
            InitializeApp();           // Uygulama ilk açıldığında yapılacak hazırlıkları başlat
            InitializeTimer();         // Günlük arşivleme zamanlayıcısını başlat
        }

        /// <summary>
        /// Uygulama açıldığında çalışacak ilk ayarlamalar (veritabanı hazırlığı, combobox'lar vs)
        /// </summary>
        private void InitializeApp()
        {
            try
            {
                // Veritabanı yoksa oluştur, varsa aç
                DatabaseHelper.InitializeDatabase();

                // Alan (bölge) seçeneklerini combobox'a ekle
                comboBoxArea.Items.AddRange(new string[] { "UAP-1", "UAP-2", "UAP-3", "UAP-4", "FES" });

                // Alan seçildiğinde ilgili sorunları yüklemek için event'e bağlan
                comboBoxArea.SelectedIndexChanged += ComboBoxArea_SelectedIndexChanged;

                // GENEL sorunlar (tüm alanlar için ortak sorunlar)
                issueMap["GENEL"] = new List<string>
                {
                    "Bilgisayar açılmıyor",
                    "Mouse bozuldu",
                    "Klavye çalışmıyor",
                    "Yazıcıya kağıt sıkıştı",
                    "Yazıcıya internet gitmiyor",
                    "Yazıcı çalışmıyor"
                };

                // Özel alanlara ait özel sorunlar
                issueMap["UAP-1"] = new List<string> { "SAP terminal donuyor", "Barkod yazıcı hatası" };
                issueMap["UAP-2"] = new List<string> { "IP erişilemiyor" };
                issueMap["FES"] = new List<string> { "PLC bağlantısı kesildi", "Etiket yazıcı çevrimdışı" };
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Başlatma sırasında hata oluştu. Detaylar log dosyasına kaydedildi.", "Hata");
            }
        }

        /// <summary>
        /// Günlük olarak çalışacak zamanlayıcıyı başlatır.
        /// Bu zamanlayıcı çözülmüş talepleri arşivler ve veritabanından siler.
        /// </summary>
        private void InitializeTimer()
        {
            dailyCleanupTimer = new System.Windows.Forms.Timer();
            dailyCleanupTimer.Interval = 86400000; // 24 saat = 24 * 60 * 60 * 1000 ms
            dailyCleanupTimer.Tick += DailyCleanupTimer_Tick;
            dailyCleanupTimer.Start();
        }

        /// <summary>
        /// Kullanıcı bir alan seçtiğinde, o alana özel sorunlar combobox'a yüklenir.
        /// Ayrıca her zaman genel sorunlar da eklenir.
        /// </summary>
        private void ComboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selected = comboBoxArea.SelectedItem.ToString();
                comboBoxIssue.Items.Clear();

                // Seçilen alana ait sorunlar varsa ekle
                if (issueMap.ContainsKey(selected))
                    comboBoxIssue.Items.AddRange(issueMap[selected].ToArray());

                // Genel sorunları da her zaman ekle
                comboBoxIssue.Items.AddRange(issueMap["GENEL"].ToArray());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Sorun listesi yüklenemedi. Lütfen logları kontrol edin.", "Hata");
            }
        }

        /// <summary>
        /// Form ilk yüklendiğinde geçmiş ticket kayıtlarını listeler.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadTicketHistory();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Kayıtlar yüklenemedi.", "Hata");
            }
        }

        /// <summary>
        /// Ticket geçmişini yükler
        /// </summary>
        private void LoadTicketHistory()
        {
            try
            {
                listBoxTickets.Items.Clear();
                var tickets = DatabaseHelper.GetAllTickets();

                // En yeni ticket'ları üstte göstermek için ters sırala
                tickets.Reverse();

                foreach (var ticket in tickets)
                {
                    string statusText = "";
                    switch (ticket.Status?.ToLower())
                    {
                        case "beklemede":
                            statusText = " [Beklemede]";
                            break;
                        case "işlemde":
                            statusText = " [İşlemde]";
                            break;
                        case "çözüldü":
                            statusText = " [Çözüldü]";
                            break;
                        default:
                            statusText = " [Beklemede]";
                            break;
                    }

                    listBoxTickets.Items.Add($"[{ticket.CreatedAt:dd/MM/yyyy HH:mm}] {ticket.Area} - {ticket.Issue}{statusText}");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Ticket geçmişi yüklenemedi.", "Hata");
            }
        }

        /// <summary>
        /// Talep gönder butonuna basıldığında kullanıcıdan gelen bilgileri kontrol eder,
        /// eksiklik yoksa yeni bir destek talebi olarak veritabanına kaydeder.
        /// </summary>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxArea.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir alan (UAP/FES) seçin.", "Eksik Alan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxArea.Focus();
                    return;
                }

                if (comboBoxIssue.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir sorun tipi seçin.", "Eksik Sorun", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxIssue.Focus();
                    return;
                }

                string description = textBoxDescription.Text.Trim();

                if (string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show("Lütfen açıklama girin.", "Eksik Açıklama", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxDescription.Focus();
                    return;
                }

                if (description.Length > 300)
                {
                    MessageBox.Show("Açıklama 300 karakteri aşmamalı.", "Uzun Açıklama", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxDescription.Focus();
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Bu talebi göndermek istediğinize emin misiniz?",
                    "Talep Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                    return;

                // Butonu geçici olarak devre dışı bırak
                btnSubmit.Enabled = false;
                btnSubmit.Text = "Gönderiliyor...";

                var ticket = new Ticket
                {
                    Area = comboBoxArea.SelectedItem.ToString(),
                    Issue = comboBoxIssue.SelectedItem.ToString(),
                    Description = description,
                    CreatedAt = DateTime.Now,
                    IsResolved = false, // Yeni talep gönderildiğinde çözülmemiş olarak işaretlenir
                    Status = "beklemede" // ÖNEMLİ: Yeni ticket'lar beklemede durumunda başlar
                };

                DatabaseHelper.InsertTicket(ticket);

                // Formu temizle
                comboBoxArea.SelectedIndex = -1;
                comboBoxIssue.SelectedIndex = -1;
                comboBoxIssue.Items.Clear();
                textBoxDescription.Clear();

                // Ticket geçmişini yenile
                LoadTicketHistory();

                // Başarı mesajı
                MessageBox.Show(
                    "Talep başarıyla gönderildi!\nIT ekibi en kısa sürede talebinizle ilgilenecektir.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Butonu tekrar aktif et
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Talebi Gönder";
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Talep gönderilirken hata oluştu. Lütfen log dosyasını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Butonu tekrar aktif et
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Talebi Gönder";
            }
        }

        /// <summary>
        /// Yenile butonuna basıldığında ticket geçmişini yeniden yükler
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadTicketHistory();
        }

        /// <summary>
        /// Gün sonunda çözülmüş taleplerin arşivlenmesi ve silinmesi işlemi
        /// </summary>
        private void DailyCleanupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var resolvedTickets = DatabaseHelper.GetResolvedTickets();

                if (resolvedTickets.Count > 0)
                {
                    var archiveFile = $"resolved_{DateTime.Now:yyyyMMdd_HHmmss}.db";

                    // Arşive yaz
                    DatabaseHelper.ArchiveTickets(resolvedTickets, archiveFile);

                    // Veritabanından sil
                    DatabaseHelper.DeleteResolvedTickets();

                    //Logger.Log($"Günlük temizlik: {resolvedTickets.Count} kayıt arşivlendi.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}