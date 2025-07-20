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

        // Alan-alt alan haritası
        private Dictionary<string, List<string>> areaSubAreaMap = new Dictionary<string, List<string>>();

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

                // Alan seçildiğinde ilgili alt alanları ve sorunları yüklemek için event'e bağlan
                comboBoxArea.SelectedIndexChanged += ComboBoxArea_SelectedIndexChanged;
                comboBoxSubArea.SelectedIndexChanged += ComboBoxSubArea_SelectedIndexChanged;

                // Sorun haritasını IssueCatalog üzerinden yükle
                issueMap = IssueCatalog.GetIssueMap();

                // Alan-alt alan haritasını AreaCatalog üzerinden yükle
                areaSubAreaMap = AreaCatalog.GetAreaSubAreaMap();

                // Karakter sayacı için event
                textBoxDescription.TextChanged += TextBoxDescription_TextChanged;

                // Alt alan combobox'ını başlangıçta devre dışı bırak
                comboBoxSubArea.Enabled = false;
                comboBoxIssue.Enabled = false;
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
        /// Kullanıcı bir alan seçtiğinde, o alana özel alt alanlar combobox'a yüklenir.
        /// </summary>
        private void ComboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Null kontrolü ekle
                if (comboBoxArea.SelectedItem == null)
                    return;

                string selectedArea = comboBoxArea.SelectedItem.ToString();

                // Alt alan ComboBox'ı temizle ve aktif et
                comboBoxSubArea.Items.Clear();
                comboBoxSubArea.Enabled = true;
                comboBoxSubArea.SelectedIndex = -1;

                // Sorun ComboBox'ını temizle ve devre dışı bırak
                comboBoxIssue.Items.Clear();
                comboBoxIssue.Enabled = false;
                comboBoxIssue.SelectedIndex = -1;

                // areaSubAreaMap null kontrolü
                if (areaSubAreaMap == null)
                {
                    areaSubAreaMap = AreaCatalog.GetAreaSubAreaMap();
                }

                // Seçilen alana ait alt alanlar varsa ekle
                if (areaSubAreaMap.ContainsKey(selectedArea) && areaSubAreaMap[selectedArea] != null)
                {
                    comboBoxSubArea.Items.AddRange(areaSubAreaMap[selectedArea].ToArray());
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Alt alan listesi yüklenemedi. Lütfen logları kontrol edin.", "Hata");
            }
        }

        /// <summary>
        /// Alt alan seçildiğinde sorun listesini yükler
        /// </summary>
        private void ComboBoxSubArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Null kontrolü ekle
                if (comboBoxArea.SelectedItem == null || comboBoxSubArea.SelectedItem == null)
                    return;

                string selectedArea = comboBoxArea.SelectedItem.ToString();

                // Sorun ComboBox'ını temizle ve aktif et
                comboBoxIssue.Items.Clear();
                comboBoxIssue.Enabled = true;
                comboBoxIssue.SelectedIndex = -1;

                // issueMap null kontrolü
                if (issueMap == null)
                {
                    issueMap = IssueCatalog.GetIssueMap();
                }

                // Seçilen alana ait sorunlar varsa ekle
                if (issueMap.ContainsKey(selectedArea) && issueMap[selectedArea] != null)
                {
                    comboBoxIssue.Items.AddRange(issueMap[selectedArea].ToArray());
                }

                // Genel sorunları da her zaman ekle
                if (issueMap.ContainsKey("GENEL") && issueMap["GENEL"] != null)
                {
                    comboBoxIssue.Items.AddRange(issueMap["GENEL"].ToArray());
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Sorun listesi yüklenemedi. Lütfen logları kontrol edin.", "Hata");
            }
        }

        /// <summary>
        /// Açıklama alanında karakter sayacını günceller
        /// </summary>
        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxDescription != null && labelCharCount != null)
                {
                    int charCount = textBoxDescription.Text.Length;
                    labelCharCount.Text = $"{charCount} / 300";

                    // Karakter sınırına yaklaşırken renk değiştir
                    if (charCount > 250)
                        labelCharCount.ForeColor = System.Drawing.Color.Red;
                    else if (charCount > 200)
                        labelCharCount.ForeColor = System.Drawing.Color.Orange;
                    else
                        labelCharCount.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
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
                // ListBox kontrolü
                if (listBoxTickets == null)
                    return;

                listBoxTickets.Items.Clear();
                var tickets = DatabaseHelper.GetAllTickets();

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
                        case "reddedildi":
                            statusText = " [Reddedildi]";
                            break;
                        default:
                            statusText = " [Beklemede]";
                            break;
                    }

                    string subAreaText = !string.IsNullOrEmpty(ticket.SubArea) ? $" - {ticket.SubArea}" : "";
                    string assignedText = !string.IsNullOrEmpty(ticket.AssignedTo) ? $" ({ticket.AssignedTo})" : "";

                    listBoxTickets.Items.Add($"[{ticket.CreatedAt:dd/MM/yyyy HH:mm}] {ticket.Area}{subAreaText} - {ticket.Issue}{statusText}{assignedText}");
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

                if (comboBoxSubArea.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir alt alan seçin.", "Eksik Alt Alan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxSubArea.Focus();
                    return;
                }

                if (comboBoxIssue.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir sorun tipi seçin.", "Eksik Sorun", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxIssue.Focus();
                    return;
                }

                // textBoxDescription null kontrolü
                if (textBoxDescription == null)
                {
                    MessageBox.Show("Açıklama alanı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string description = textBoxDescription.Text.Trim();

                if (string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show("Lütfen açıklama girin.", "Eksik Açıklama", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxDescription.Focus();
                    return;
                }

                if (description.Length > 300 || description.Length < 20)
                {
                    MessageBox.Show("Açıklama 300 karakteri aşmamalı ve 20 karakterden kısa olamaz","hatalı açıklama" , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxDescription.Focus();
                    return;
                }

                // Kişi bilgileri kontrolü
                string firstName = textBoxFirstName.Text.Trim();
                string lastName = textBoxLastName.Text.Trim();
                string phoneNumber = textBoxPhoneNumber.Text.Trim();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    MessageBox.Show("Lütfen adınızı girin.", "Eksik Ad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxFirstName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    MessageBox.Show("Lütfen soyadınızı girin.", "Eksik Soyad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxLastName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("Lütfen telefon numaranızı girin.", "Eksik Telefon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPhoneNumber.Focus();
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

                // Buton kontrolü
                if (btnSubmit != null)
                {
                    btnSubmit.Enabled = false;
                    btnSubmit.Text = "Gönderiliyor...";
                }

                var ticket = new Ticket
                {
                    Area = comboBoxArea.SelectedItem.ToString(),
                    SubArea = comboBoxSubArea.SelectedItem.ToString(),
                    Issue = comboBoxIssue.SelectedItem.ToString(),
                    Description = description,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    CreatedAt = DateTime.Now,
                    IsResolved = false, // Yeni talep gönderildiğinde çözülmemiş olarak işaretlenir
                    Status = "beklemede", // ÖNEMLİ: Yeni ticket'lar beklemede durumunda başlar
                    AssignedTo = null
                };

                DatabaseHelper.InsertTicket(ticket);

                // Formu temizle
                comboBoxArea.SelectedIndex = -1;
                comboBoxSubArea.SelectedIndex = -1;
                comboBoxIssue.SelectedIndex = -1;
                comboBoxSubArea.Items.Clear();
                comboBoxIssue.Items.Clear();
                comboBoxSubArea.Enabled = false;
                comboBoxIssue.Enabled = false;
                textBoxDescription.Clear();
                textBoxFirstName.Clear();
                textBoxLastName.Clear();
                textBoxPhoneNumber.Clear();

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
                if (btnSubmit != null)
                {
                    btnSubmit.Enabled = true;
                    btnSubmit.Text = "Talebi Gönder";
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Talep gönderilirken hata oluştu. Lütfen log dosyasını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Butonu tekrar aktif et
                if (btnSubmit != null)
                {
                    btnSubmit.Enabled = true;
                    btnSubmit.Text = "Talebi Gönder";
                }
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

                    // Ticket geçmişini yenile
                    LoadTicketHistory();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private void groupBoxHistory_Enter(object sender, EventArgs e)
        {
            // Boş bırakıldı
        }
    }
}