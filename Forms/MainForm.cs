// TicketApp/Forms/MainForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TicketApp.Helpers;   // InitializeDatabase, GetAreaSubAreaMap, GetIssueMap vb.
using TicketApp.Models;    // Ticket modeli (Line alanı eklendi)

namespace TicketApp.Forms
{
    public partial class MainForm : Form
    {
        // Alan -> Alt Alan sözlüğü (DB; boşsa AreaCatalog fallback)
        // Ref: DatabaseHelper.GetAreaSubAreaMap + AreaCatalog.GetAreaSubAreaMap
        private Dictionary<string, List<string>> areaSubAreaMap = new Dictionary<string, List<string>>();

        // Alt Alan -> Hat (Line) sözlüğü (YENİ KATMAN)
        // Ref: DatabaseHelper.GetSubAreaLineMap (bir sonraki adımda DB tarafını ekleyeceğiz)
        private Dictionary<string, List<string>> subAreaLineMap = new Dictionary<string, List<string>>();

        // Alan -> Sorun türleri (GENEL de içerebilir)
        private Dictionary<string, List<string>> issueMap = new Dictionary<string, List<string>>();

        // Günlük arşiv temizliği için zamanlayıcı (mevcut davranış)
        private System.Windows.Forms.Timer dailyCleanupTimer;

        public MainForm()
        {
            InitializeComponent();
            InitializeApp();   // verileri yükle
            InitializeTimer(); // günlük arşivleme
        }

        /// <summary>
        /// Form açılışında veri kaynaklarını hazırlar ve combobox eventlerini bağlar.
        /// </summary>
        private void InitializeApp()
        {
            try
            {
                // 1) Veritabanını hazırla/başlat (tablo kontrol/oluşturma/seed)
                DatabaseHelper.InitializeDatabase(); // :contentReference[oaicite:5]{index=5}

                // 2) Alan-Alt Alan haritasını yükle
                areaSubAreaMap = DatabaseHelper.GetAreaSubAreaMap();
                if (areaSubAreaMap == null || areaSubAreaMap.Count == 0)
                    areaSubAreaMap = AreaCatalog.GetAreaSubAreaMap(); // fallback (seed) :contentReference[oaicite:6]{index=6}

                // 3) Alt Alan -> Hat haritası (yeni)
                // Bu metod DB tarafında Lines tablosu ile gelecek; null/boş ise boş sözlükle devam ederiz.
                try
                {
                    subAreaLineMap = DatabaseHelper.GetSubAreaLineMap(); // (sonraki committe eklenecek)
                }
                catch
                {
                    subAreaLineMap = new Dictionary<string, List<string>>();
                }

                // 4) Sorun haritası
                issueMap = DatabaseHelper.GetIssueMap(); // DB’de varsa çek
                if (issueMap == null || issueMap.Count == 0)
                    issueMap = TicketApp.Helpers.IssueCatalog.GetIssueMap(); // seed fallback

                // 5) Alanları yükle
                comboBoxArea.Items.Clear();
                comboBoxArea.Items.AddRange(areaSubAreaMap.Keys.ToArray());

                // 6) Başlangıçta zinciri kilitle (Alan seçilince açılacak)
                comboBoxSubArea.Enabled = false;
                comboBoxLine.Enabled = false;
                comboBoxIssue.Enabled = false;

                // 7) Event zinciri (Alan -> Alt Alan -> Hat -> Sorun)
                comboBoxArea.SelectedIndexChanged += ComboBoxArea_SelectedIndexChanged;
                comboBoxSubArea.SelectedIndexChanged += ComboBoxSubArea_SelectedIndexChanged;
                comboBoxLine.SelectedIndexChanged += ComboBoxLine_SelectedIndexChanged;

                // 8) Açıklama karakter sayacı
                textBoxDescription.TextChanged += TextBoxDescription_TextChanged;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Başlatma sırasında hata oluştu. Detaylar log dosyasına kaydedildi.", "Hata");
            }
        }

        /// <summary>
        /// Günlük arşivleme/silme işini tetikleyen timer (mevcut davranış korunur).
        /// </summary>
        private void InitializeTimer()
        {
            dailyCleanupTimer = new System.Windows.Forms.Timer
            {
                Interval = 24 * 60 * 60 * 1000 // 24 saat
            };
            dailyCleanupTimer.Tick += DailyCleanupTimer_Tick;
            dailyCleanupTimer.Start();
        }

        private void DailyCleanupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var resolved = DatabaseHelper.GetResolvedTickets();
                if (resolved.Count > 0)
                {
                    var archiveFile = $"resolved_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    DatabaseHelper.ArchiveTickets(resolved, archiveFile);
                    DatabaseHelper.DeleteResolvedTickets();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        // ============================
        //  EVENT ZİNCİRİ (4 AŞAMA)
        // ============================

        /// <summary>Alan seçilince: Alt Alanları doldur, diğerlerini sıfırla.</summary>
        private void ComboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Tüm alt seviye seçimleri temizle/disable
                comboBoxSubArea.Items.Clear(); comboBoxSubArea.Enabled = false; comboBoxSubArea.SelectedIndex = -1;
                comboBoxLine.Items.Clear(); comboBoxLine.Enabled = false; comboBoxLine.SelectedIndex = -1;
                comboBoxIssue.Items.Clear(); comboBoxIssue.Enabled = false; comboBoxIssue.SelectedIndex = -1;

                if (comboBoxArea.SelectedItem == null) return;

                var area = comboBoxArea.SelectedItem.ToString();

                if (areaSubAreaMap.TryGetValue(area, out var subAreas) && subAreas?.Count > 0)
                {
                    comboBoxSubArea.Items.AddRange(subAreas.ToArray());
                    comboBoxSubArea.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Alt alan listesi yüklenemedi.", "Hata");
            }
        }

        /// <summary>Alt Alan seçilince: Hat (Line) listesini doldur, Sorun sıfırlanır.</summary>
        private void ComboBoxSubArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxLine.Items.Clear(); comboBoxLine.Enabled = false; comboBoxLine.SelectedIndex = -1;
                comboBoxIssue.Items.Clear(); comboBoxIssue.Enabled = false; comboBoxIssue.SelectedIndex = -1;

                if (comboBoxSubArea.SelectedItem == null) return;

                var subArea = comboBoxSubArea.SelectedItem.ToString();

                // DB’den hatları al (yoksa boş kalır)
                if (subAreaLineMap != null && subAreaLineMap.TryGetValue(subArea, out var lines) && lines?.Count > 0)
                {
                    comboBoxLine.Items.AddRange(lines.ToArray());
                    comboBoxLine.Enabled = true;
                }
                else
                {
                    // Henüz Lines desteği yoksa kullanıcıyı bilgilendir.
                    // DB desteğini bir sonraki adımda ekleyeceğiz (Lines tablosu).
                    MessageBox.Show("Seçilen alt alana bağlı 'Hat' tanımlı değil. Ayarlar > Alan Yönetimi üzerinden Hat ekleyiniz.", "Bilgi");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Hat listesi yüklenemedi.", "Hata");
            }
        }

        /// <summary>Hat (Line) seçilince: Sorun listesini doldur.</summary>
        private void ComboBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxIssue.Items.Clear(); comboBoxIssue.Enabled = false; comboBoxIssue.SelectedIndex = -1;

                if (comboBoxArea.SelectedItem == null || comboBoxLine.SelectedItem == null) return;

                var area = comboBoxArea.SelectedItem.ToString();

                // Sorunlar alan bazlı tutuluyor (IssueCatalog/DB) — mevcut yapı korunur.
                if (issueMap != null && issueMap.TryGetValue(area, out var issues) && issues?.Count > 0)
                {
                    comboBoxIssue.Items.AddRange(issues.ToArray());
                    comboBoxIssue.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Sorun listesi yüklenemedi.", "Hata");
            }
        }

        // ============================
        //  YARDIMCI EVENTLER
        // ============================

        /// <summary>Açıklama için karakter limiti (300) göstergesi.</summary>
        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            int len = (textBoxDescription.Text ?? string.Empty).Length;
            if (len > 300)
            {
                // 300 üstünü kes (kullanıcı deneyimi için otomatik sınırlama)
                textBoxDescription.Text = textBoxDescription.Text.Substring(0, 300);
                textBoxDescription.SelectionStart = textBoxDescription.Text.Length;
                len = 300;
            }
            labelCharCount.Text = $"{len} / 300";
        }

        // ============================
        //  KAYIT OLUŞTURMA
        // ============================

        /// <summary>Yeni talebin kaydedilmesi (gerekli alan doğrulamaları).</summary>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Zorunlu alanlar: Area, SubArea, Line, Issue, Description, FirstName, LastName, PhoneNumber
                if (comboBoxArea.SelectedItem == null) { MessageBox.Show("Lütfen Alan seçiniz."); return; }
                if (comboBoxSubArea.SelectedItem == null) { MessageBox.Show("Lütfen Alt Alan seçiniz."); return; }
                if (comboBoxLine.SelectedItem == null) { MessageBox.Show("Lütfen Hat seçiniz."); return; }
                if (comboBoxIssue.SelectedItem == null) { MessageBox.Show("Lütfen Sorun seçiniz."); return; }

                var ticket = new Ticket
                {
                    Area = comboBoxArea.SelectedItem.ToString(),
                    SubArea = comboBoxSubArea.SelectedItem.ToString(),
                    Line = comboBoxLine.SelectedItem.ToString(),                // YENİ
                    Issue = comboBoxIssue.SelectedItem.ToString(),
                    Description = (textBoxDescription.Text ?? string.Empty).Trim(),
                    FirstName = (textBoxFirstName.Text ?? string.Empty).Trim(),
                    LastName = (textBoxLastName.Text ?? string.Empty).Trim(),
                    PhoneNumber = (textBoxPhoneNumber.Text ?? string.Empty).Trim(),
                    CreatedAt = DateTime.Now,
                    Status = "beklemede",
                    IsResolved = false
                };

                // Not: DatabaseHelper.InsertTicket şu an Line alanını kaydetmiyor olabilir.
                // Bir sonraki adımda Insert/Select map’lerine Line kolonu eklenecek ve Lines tablosu oluşturulacak.
                DatabaseHelper.InsertTicket(ticket);

                MessageBox.Show("Talep başarıyla oluşturuldu.");
                ClearForm();
                RefreshHistory();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Talep oluşturulurken hata oluştu.", "Hata");
            }
        }

        private void ClearForm()
        {
            comboBoxArea.SelectedIndex = -1;
            comboBoxSubArea.Items.Clear(); comboBoxSubArea.Enabled = false;
            comboBoxLine.Items.Clear(); comboBoxLine.Enabled = false;
            comboBoxIssue.Items.Clear(); comboBoxIssue.Enabled = false;

            textBoxDescription.Clear();
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxPhoneNumber.Clear();
            labelCharCount.Text = "0 / 300";
        }

        private void RefreshHistory()
        {
            try
            {
                var all = DatabaseHelper.GetAllTickets();
                listBoxTickets.Items.Clear();
                foreach (var t in all)
                {
                    // Gösterimde FullArea artık Line’ı da içerir (model güncel)
                    listBoxTickets.Items.Add($"#{t.Id} | {t.FullArea} | {t.Issue} | {t.Status} | {t.CreatedAt:dd.MM.yyyy HH:mm}");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        // Form yüklendiğinde buton eventlerini bağlamak için (designer’a alternatif)
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.btnSubmit.Click += BtnSubmit_Click;
            this.btnRefresh.Click += (s, a) => RefreshHistory();
            RefreshHistory();
        }
    }
}
