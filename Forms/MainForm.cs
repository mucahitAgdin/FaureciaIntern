// TicketApp/Forms/MainForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TicketApp.Helpers;
using TicketApp.Models;

namespace TicketApp.Forms
{
    public partial class MainForm : Form
    {
        // Alan -> Alt Alan sözlüğü (DB’den yüklenir; boşsa AreaCatalog fallback)
        private Dictionary<string, List<string>> areaSubAreaMap = new Dictionary<string, List<string>>();

        // Alt Alan -> Hat (Line) sözlüğü (yeni)
        private Dictionary<string, List<string>> subAreaLineMap = new Dictionary<string, List<string>>();

        // Alan -> Sorun listesi (GENEL anahtarı her zaman eklenir)
        private Dictionary<string, List<string>> issueMap = new Dictionary<string, List<string>>();

        // Günlük arşivleme zamanlayıcısı (mevcut davranış korunuyor)
        private System.Windows.Forms.Timer dailyCleanupTimer;

        public MainForm()
        {
            InitializeComponent();
            InitializeApp();
            InitializeTimer();
        }

        /// <summary>
        /// Form açılış hazırlıkları: verileri yükle, combobox’ları hazırla, eventleri bağla.
        /// </summary>
        private void InitializeApp()
        {
            try
            {
                // DB hazırla (varsa açar, yoksa oluşturup seed eder)
                DatabaseHelper.InitializeDatabase();

                // Alan–Alt Alan haritası
                areaSubAreaMap = DatabaseHelper.GetAreaSubAreaMap();
                if (areaSubAreaMap == null || areaSubAreaMap.Count == 0)
                    areaSubAreaMap = AreaCatalog.GetAreaSubAreaMap(); // fallback

                // Hat (Line) haritası — alt alana göre hat listesi
                subAreaLineMap = DatabaseHelper.GetSubAreaLineMap(); // yeni

                // Sorun haritası
                issueMap = DatabaseHelper.GetIssueMap();

                // Alanları yükle
                comboBoxArea.Items.Clear();
                comboBoxArea.Items.AddRange(areaSubAreaMap.Keys.ToArray());

                // Başlangıçta zincirin geri kalanını kilitle
                comboBoxSubArea.Enabled = false;
                comboBoxLine.Enabled = false;
                comboBoxIssue.Enabled = false;

                // Event zinciri (Alan -> Alt Alan -> Hat -> Sorun)
                comboBoxArea.SelectedIndexChanged += ComboBoxArea_SelectedIndexChanged;
                comboBoxSubArea.SelectedIndexChanged += ComboBoxSubArea_SelectedIndexChanged;
                comboBoxLine.SelectedIndexChanged += ComboBoxLine_SelectedIndexChanged;

                // Karakter sayacı
                textBoxDescription.TextChanged += TextBoxDescription_TextChanged;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Başlatma sırasında hata oluştu. Detaylar log dosyasına kaydedildi.", "Hata");
            }
        }

        /// <summary>
        /// Günlük arşivleme/silme işini tetikleyen timer.
        /// </summary>
        private void InitializeTimer()
        {
            dailyCleanupTimer = new System.Windows.Forms.Timer
            {
                Interval = 86400000 // 24 saat
            };
            dailyCleanupTimer.Tick += DailyCleanupTimer_Tick;
            dailyCleanupTimer.Start();
        }

        private void DailyCleanupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Mevcut davranış: çözülenleri arşivle ve sil
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

        /// <summary>
        /// Alan seçilince: Alt Alan’ları doldur, diğerlerini temizle.
        /// </summary>
        private void ComboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxSubArea.Items.Clear();
                comboBoxSubArea.Enabled = false;
                comboBoxLine.Items.Clear();
                comboBoxLine.Enabled = false;
                comboBoxIssue.Items.Clear();
                comboBoxIssue.Enabled = false;

                if (comboBoxArea.SelectedItem == null) return;

                var area = comboBoxArea.SelectedItem.ToString();
                if (areaSubAreaMap.ContainsKey(area) && areaSubAreaMap[area] != null)
                {
                    comboBoxSubArea.Items.AddRange(areaSubAreaMap[area].ToArray());
                    comboBoxSubArea.Enabled = comboBoxSubArea.Items.Count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Alt alan listesi yüklenemedi.", "Hata");
            }
        }

        /// <summary>
        /// Alt Alan seçilince: Hat (Line) listesini doldur, sorunları temizle.
        /// </summary>
        private void ComboBoxSubArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxLine.Items.Clear();
                comboBoxLine.Enabled = false;
                comboBoxIssue.Items.Clear();
                comboBoxIssue.Enabled = false;

                if (comboBoxSubArea.SelectedItem == null) return;

                var sub = comboBoxSubArea.SelectedItem.ToString();
                if (subAreaLineMap.ContainsKey(sub) && subAreaLineMap[sub] != null)
                {
                    comboBoxLine.Items.AddRange(subAreaLineMap[sub].ToArray());
                    comboBoxLine.Enabled = comboBoxLine.Items.Count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Hat listesi yüklenemedi.", "Hata");
            }
        }

        /// <summary>
        /// Hat seçilince: ilgili Alan için sorun tiplerini yükle (GENEL dahil).
        /// </summary>
        private void ComboBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxIssue.Items.Clear();
                comboBoxIssue.Enabled = false;

                if (comboBoxArea.SelectedItem == null) return;

                var area = comboBoxArea.SelectedItem.ToString();

                // Alan’a özel sorunlar
                if (issueMap.ContainsKey(area))
                    comboBoxIssue.Items.AddRange(issueMap[area].ToArray());

                // GENEL sorunlar
                if (issueMap.ContainsKey("GENEL"))
                    comboBoxIssue.Items.AddRange(issueMap["GENEL"].ToArray());

                comboBoxIssue.Enabled = comboBoxIssue.Items.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Sorun listesi yüklenemedi.", "Hata");
            }
        }

        /// <summary>Karakter sayacı</summary>
        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            var len = textBoxDescription.Text?.Length ?? 0;
            labelCharCount.Text = $"{len} / 300";
            if (len > 250) labelCharCount.ForeColor = System.Drawing.Color.Red;
            else if (len > 200) labelCharCount.ForeColor = System.Drawing.Color.Orange;
            else labelCharCount.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
        }

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

        /// <summary>Geçmiş ticketları listeler.</summary>
        private void LoadTicketHistory()
        {
            listBoxTickets.Items.Clear();
            var tickets = DatabaseHelper.GetAllTickets();
            foreach (var t in tickets)
            {
                string status = t.Status?.ToLower() switch
                {
                    "işlemde" => " [İşlemde]",
                    "çözüldü" => " [Çözüldü]",
                    "reddedildi" => " [Reddedildi]",
                    _ => " [Beklemede]"
                };

                var subText = !string.IsNullOrEmpty(t.SubArea) ? $" - {t.SubArea}" : "";
                var assigned = !string.IsNullOrEmpty(t.AssignedTo) ? $" ({t.AssignedTo})" : "";
                listBoxTickets.Items.Add($"[{t.CreatedAt:dd/MM/yyyy HH:mm}] {t.Area}{subText} - {t.Issue}{status}{assigned}");
            }
        }

        /// <summary>
        /// Gönder: tüm seçimleri doğrula ve ticket ekle.
        /// Not: Ticket modelinde Line alanı yoksa, SubArea’yi “Alt Alan / Hat” olarak dolduruyoruz.
        /// </summary>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxArea.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir Alan seçin.", "Eksik Alan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxArea.Focus(); return;
                }
                if (comboBoxSubArea.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir Alt Alan seçin.", "Eksik Alt Alan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxSubArea.Focus(); return;
                }
                if (comboBoxLine.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir Hat seçin.", "Eksik Hat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxLine.Focus(); return;
                }
                if (comboBoxIssue.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir Sorun Tipi seçin.", "Eksik Sorun", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxIssue.Focus(); return;
                }

                var desc = (textBoxDescription.Text ?? "").Trim();
                if (desc.Length < 20 || desc.Length > 300)
                {
                    MessageBox.Show("Açıklama 20–300 karakter arasında olmalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxDescription.Focus(); return;
                }

                var firstName = (textBoxFirstName.Text ?? "").Trim();
                var lastName = (textBoxLastName.Text ?? "").Trim();
                var phone = (textBoxPhoneNumber.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(firstName)) { MessageBox.Show("Lütfen adınızı girin."); textBoxFirstName.Focus(); return; }
                if (string.IsNullOrWhiteSpace(lastName)) { MessageBox.Show("Lütfen soyadınızı girin."); textBoxLastName.Focus(); return; }
                if (string.IsNullOrWhiteSpace(phone)) { MessageBox.Show("Lütfen telefon numaranızı girin."); textBoxPhoneNumber.Focus(); return; }

                var area = comboBoxArea.SelectedItem.ToString();
                var sub = comboBoxSubArea.SelectedItem.ToString();
                var line = comboBoxLine.SelectedItem.ToString();      // Yeni
                var issue = comboBoxIssue.SelectedItem.ToString();

                // Geçici strateji: Line alanı olmadığı için SubArea “Alt Alan / Hat”
                var subWithLine = $"{sub} / {line}";

                var ticket = new Ticket
                {
                    Area = area,
                    SubArea = subWithLine,   // geçici taşıyıcı
                    Issue = issue,
                    Description = desc,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phone,
                    CreatedAt = DateTime.Now,
                    Status = "beklemede",
                    IsResolved = false
                };

                DatabaseHelper.InsertTicket(ticket);

                MessageBox.Show("Talep başarıyla oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Formu temizle ve geçmişi yenile
                comboBoxArea.SelectedIndex = -1;
                comboBoxSubArea.Items.Clear(); comboBoxSubArea.Enabled = false;
                comboBoxLine.Items.Clear(); comboBoxLine.Enabled = false;
                comboBoxIssue.Items.Clear(); comboBoxIssue.Enabled = false;
                textBoxDescription.Clear();
                textBoxFirstName.Clear();
                textBoxLastName.Clear();
                textBoxPhoneNumber.Clear();

                LoadTicketHistory();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Talep oluşturulamadı. Lütfen daha sonra tekrar deneyin.", "Hata");
            }
        }
    }
}
