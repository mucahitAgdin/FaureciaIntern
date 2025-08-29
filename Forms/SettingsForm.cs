// TicketApp/Forms/SettingsForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TicketApp.Helpers;     // DatabaseHelper, AreaCatalog, IssueCatalog, Logger  // refs: :contentReference[oaicite:7]{index=7} :contentReference[oaicite:8]{index=8} :contentReference[oaicite:9]{index=9} :contentReference[oaicite:10]{index=10}
using TicketApp.Models;      // Ticket

namespace TicketApp.Forms
{
    /// <summary>
    /// Sistem ayarları: Alan, Alt Alan, Hat (Line) ve Sorun tiplerinin yönetimi.
    /// Zincir: Alan -> Alt Alan -> Hat -> Sorun
    /// </summary>
    public partial class SettingsForm : Form
    {
        // --- Bellek içi haritalar ---
        // Alan -> Alt Alan listesi
        private Dictionary<string, List<string>> areaSubAreaMap;

        // Alt Alan -> Hat listesi (YENİ)
        private Dictionary<string, List<string>> subAreaLineMap;

        // Alan -> Sorun listesi
        private Dictionary<string, List<string>> issueMap;

        private bool hasChanges = false; // Kaydet uyarısı için

        // --- UI Bileşenleri (Designer'da iskelet oluşturuluyor; detaylar burada) ---
        private TabControl tabControl;
        private TabPage tabAreas;
        private TabPage tabIssues;

        // Alan yönetimi
        private ListBox listBoxAreas;
        private TextBox textBoxNewArea;
        private Button btnAddArea;
        private Button btnDeleteArea;

        private Label lblSelectedArea;
        private ListBox listBoxSubAreas;
        private TextBox textBoxNewSubArea;
        private Button btnAddSubArea;
        private Button btnDeleteSubArea;

        // YENİ: Hat yönetimi
        private Label lblSelectedSubArea;
        private ListBox listBoxLines;
        private TextBox textBoxNewLine;
        private Button btnAddLine;
        private Button btnDeleteLine;

        // Sorun yönetimi
        private ComboBox comboBoxIssueArea;
        private ListBox listBoxIssues;
        private TextBox textBoxNewIssue;
        private Button btnAddIssue;
        private Button btnDeleteIssue;

        // Genel
        private Button btnSave;
        private Button btnCancel;
        private Button btnReset;

        public SettingsForm()
        {
            InitializeComponent(); // Designer: tabControl oluşturur ve Create* metodlarını çağırır  // refs: :contentReference[oaicite:11]{index=11} :contentReference[oaicite:12]{index=12}
            LoadData();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// DB'den tüm sözlükleri çeker, yoksa kataloglardan seed eder ve UI'yı tazeler.
        /// </summary>
        private void LoadData()
        {
            try
            {
                // DB hazır (Program.cs'de InitializeDatabase çağrılıyor)  // ref: :contentReference[oaicite:13]{index=13}

                // Alan–Alt Alan
                areaSubAreaMap = DatabaseHelper.GetAreaSubAreaMap();
                if (areaSubAreaMap == null || areaSubAreaMap.Count == 0)
                    areaSubAreaMap = AreaCatalog.GetAreaSubAreaMap();  // seed fallback  // ref: :contentReference[oaicite:14]{index=14}

                // Alt Alan–Hat (Line)
                subAreaLineMap = DatabaseHelper.GetSubAreaLineMap();  // YENİ  // ref: :contentReference[oaicite:15]{index=15}
                if (subAreaLineMap == null) subAreaLineMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

                // Sorunlar
                issueMap = DatabaseHelper.GetIssueMap();
                if (issueMap == null || issueMap.Count == 0)
                    issueMap = IssueCatalog.GetIssueMap();             // seed fallback  // ref: :contentReference[oaicite:16]{index=16}

                RefreshUI();
                hasChanges = false;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Veriler yüklenirken hata oluştu. Lütfen logları kontrol edin.", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Temel UI'ları yeniden doldurur.</summary>
        private void RefreshUI()
        {
            // Alan listesi
            if (listBoxAreas != null)
            {
                listBoxAreas.Items.Clear();
                foreach (var area in areaSubAreaMap.Keys.OrderBy(x => x))
                    listBoxAreas.Items.Add(area);
            }

            // Sorun alanları
            if (comboBoxIssueArea != null)
            {
                comboBoxIssueArea.Items.Clear();
                foreach (var area in issueMap.Keys.OrderBy(x => x))
                    comboBoxIssueArea.Items.Add(area);
            }

            // Seçime bağlı alanlar eventlerde dolduruluyor
        }

        // =========================================================================
        // ===============  Alan & Alt Alan & Hat (Line) Yönetimi  =================
        // =========================================================================

        private void ListBoxAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem == null) return;

            string selectedArea = listBoxAreas.SelectedItem.ToString();
            lblSelectedArea.Text = $"Seçilen Alan: {selectedArea}";

            // Alt alanları doldur
            listBoxSubAreas.Items.Clear();
            if (areaSubAreaMap.TryGetValue(selectedArea, out var subs) && subs != null)
                foreach (var sub in subs.OrderBy(x => x)) listBoxSubAreas.Items.Add(sub);

            listBoxSubAreas.Enabled = true;
            textBoxNewSubArea.Enabled = true;
            btnAddSubArea.Enabled = true;
            btnDeleteSubArea.Enabled = true;

            // Hat bölümünü temizle
            listBoxLines.Items.Clear();
            lblSelectedSubArea.Text = "Seçilen Alt Alan: Yok";
            textBoxNewLine.Enabled = false;
            btnAddLine.Enabled = false;
            btnDeleteLine.Enabled = false;
        }

        private void ListBoxSubAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSubAreas.SelectedItem == null) return;

            var sub = listBoxSubAreas.SelectedItem.ToString();
            lblSelectedSubArea.Text = $"Seçilen Alt Alan: {sub}";

            // Hatları yükle
            listBoxLines.Items.Clear();
            if (subAreaLineMap.TryGetValue(sub, out var lines) && lines != null)
                foreach (var line in lines.OrderBy(x => x)) listBoxLines.Items.Add(line);

            textBoxNewLine.Enabled = true;
            btnAddLine.Enabled = true;
            btnDeleteLine.Enabled = true;
        }

        private void BtnAddArea_Click(object sender, EventArgs e)
        {
            var newArea = (textBoxNewArea.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newArea) || areaSubAreaMap.ContainsKey(newArea))
            {
                MessageBox.Show("Geçerli ve benzersiz bir Alan giriniz.", "Uyarı");
                return;
            }

            areaSubAreaMap[newArea] = new List<string>();
            if (!issueMap.ContainsKey(newArea)) issueMap[newArea] = new List<string>();
            hasChanges = true;
            RefreshUI();
            MessageBox.Show($"'{newArea}' alanı eklendi.");
        }

        private void BtnDeleteArea_Click(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem == null) return;
            string area = listBoxAreas.SelectedItem.ToString();

            // Bu alana bağlı ticket var mı? (GetAllTickets ile kontrol)  // ref: :contentReference[oaicite:17]{index=17}
            var hasTicket = DatabaseHelper.GetAllTickets().Any(t => string.Equals(t.Area, area, StringComparison.OrdinalIgnoreCase));
            if (hasTicket)
            {
                MessageBox.Show("Bu alana bağlı mevcut talepler var. Silme işlemi engellendi.", "Uyarı");
                return;
            }

            // Sil
            areaSubAreaMap.Remove(area);
            issueMap.Remove(area);
            // İlgili alt alanların hatlarını da temizleyelim
            foreach (var sub in subAreaLineMap.Keys.Where(k => areaSubAreaMap.ContainsKey(area) && areaSubAreaMap[area].Contains(k)).ToList())
                subAreaLineMap.Remove(sub);

            hasChanges = true;
            RefreshUI();
            listBoxSubAreas.Items.Clear();
            listBoxLines.Items.Clear();
            MessageBox.Show($"'{area}' alanı silindi.");
        }

        private void BtnAddSubArea_Click(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem == null) { MessageBox.Show("Önce bir Alan seçiniz."); return; }
            string area = listBoxAreas.SelectedItem.ToString();

            var newSub = (textBoxNewSubArea.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newSub)) { MessageBox.Show("Geçerli bir Alt Alan giriniz."); return; }

            var subs = areaSubAreaMap[area];
            if (subs.Contains(newSub, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bu alt alan zaten mevcut.", "Uyarı");
                return;
            }

            subs.Add(newSub);
            if (!subAreaLineMap.ContainsKey(newSub)) subAreaLineMap[newSub] = new List<string>();
            hasChanges = true;

            // UI güncelle
            listBoxSubAreas.Items.Add(newSub);
            textBoxNewSubArea.Clear();
        }

        private void BtnDeleteSubArea_Click(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem == null || listBoxSubAreas.SelectedItem == null) return;
            string area = listBoxAreas.SelectedItem.ToString();
            string sub = listBoxSubAreas.SelectedItem.ToString();

            // Bu alt alana bağlı ticket var mı?  // ref: :contentReference[oaicite:18]{index=18}
            var hasTicket = DatabaseHelper.GetAllTickets().Any(t => string.Equals(t.SubArea, sub, StringComparison.OrdinalIgnoreCase));
            if (hasTicket)
            {
                MessageBox.Show("Bu alt alana bağlı mevcut talepler var. Silme işlemi engellendi.", "Uyarı");
                return;
            }

            // Sil
            areaSubAreaMap[area].RemoveAll(x => string.Equals(x, sub, StringComparison.OrdinalIgnoreCase));
            if (subAreaLineMap.ContainsKey(sub)) subAreaLineMap.Remove(sub);
            hasChanges = true;

            // UI güncelle
            listBoxSubAreas.Items.Remove(sub);
            listBoxLines.Items.Clear();
            lblSelectedSubArea.Text = "Seçilen Alt Alan: Yok";
        }

        private void BtnAddLine_Click(object sender, EventArgs e)
        {
            if (listBoxSubAreas.SelectedItem == null) { MessageBox.Show("Önce bir Alt Alan seçiniz."); return; }
            string sub = listBoxSubAreas.SelectedItem.ToString();

            var newLine = (textBoxNewLine.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newLine)) { MessageBox.Show("Geçerli bir Hat adı giriniz."); return; }

            if (!subAreaLineMap.ContainsKey(sub))
                subAreaLineMap[sub] = new List<string>();

            if (subAreaLineMap[sub].Contains(newLine, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bu hat zaten mevcut.", "Uyarı");
                return;
            }

            // DB'ye tek tek eklemek istersen:
            DatabaseHelper.AddLine(sub, newLine); // pratik ekleme  // ref: :contentReference[oaicite:19]{index=19}
            // Bellek içini güncelle
            subAreaLineMap[sub].Add(newLine);
            hasChanges = true;

            // UI
            listBoxLines.Items.Add(newLine);
            textBoxNewLine.Clear();
        }

        private void BtnDeleteLine_Click(object sender, EventArgs e)
        {
            if (listBoxSubAreas.SelectedItem == null || listBoxLines.SelectedItem == null) return;
            string sub = listBoxSubAreas.SelectedItem.ToString();
            string line = listBoxLines.SelectedItem.ToString();

            // Ticket var mı? DatabaseHelper.HasTicketsForLine  // ref: :contentReference[oaicite:20]{index=20}
            if (DatabaseHelper.HasTicketsForLine(line))
            {
                MessageBox.Show("Bu hatta bağlı mevcut talepler var. Silme işlemi engellendi.", "Uyarı");
                return;
            }

            // DB'den sil
            var ok = DatabaseHelper.DeleteLine(sub, line);    // bağlı ticket varsa false dönecek  // ref: :contentReference[oaicite:21]{index=21}
            if (!ok)
            {
                MessageBox.Show("Hat silinemedi. Bağlı talepler olabilir.", "Uyarı");
                return;
            }

            // Bellek içini güncelle
            if (subAreaLineMap.ContainsKey(sub))
                subAreaLineMap[sub].RemoveAll(x => string.Equals(x, line, StringComparison.OrdinalIgnoreCase));

            hasChanges = true;

            // UI
            listBoxLines.Items.Remove(line);
        }

        // =========================================================================
        // =========================  Sorun Yönetimi  ===============================
        // =========================================================================

        private void ComboBoxIssueArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxIssues.Items.Clear();
            if (comboBoxIssueArea.SelectedItem == null) return;

            string area = comboBoxIssueArea.SelectedItem.ToString();
            if (!issueMap.ContainsKey(area)) issueMap[area] = new List<string>();

            foreach (var issue in issueMap[area].OrderBy(x => x))
                listBoxIssues.Items.Add(issue);
        }

        private void BtnAddIssue_Click(object sender, EventArgs e)
        {
            if (comboBoxIssueArea.SelectedItem == null) { MessageBox.Show("Önce bir Alan seçiniz."); return; }
            string area = comboBoxIssueArea.SelectedItem.ToString();

            var newIssue = (textBoxNewIssue.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newIssue)) { MessageBox.Show("Geçerli bir Sorun adı giriniz."); return; }

            if (!issueMap.ContainsKey(area)) issueMap[area] = new List<string>();
            if (issueMap[area].Contains(newIssue, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bu sorun tipi zaten mevcut.", "Uyarı");
                return;
            }

            issueMap[area].Add(newIssue);
            hasChanges = true;

            listBoxIssues.Items.Add(newIssue);
            textBoxNewIssue.Clear();
        }

        private void BtnDeleteIssue_Click(object sender, EventArgs e)
        {
            if (comboBoxIssueArea.SelectedItem == null || listBoxIssues.SelectedItem == null) return;
            string area = comboBoxIssueArea.SelectedItem.ToString();
            string issue = listBoxIssues.SelectedItem.ToString();

            // Bu sorun başlığına sahip açık ticket olabilir; burada sadece uyarı yapıyoruz.
            var hasTicket = DatabaseHelper.GetAllTickets().Any(t =>
                string.Equals(t.Area, area, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(t.Issue, issue, StringComparison.OrdinalIgnoreCase));
            if (hasTicket)
            {
                var res = MessageBox.Show("Bu sorun tipine bağlı mevcut talepler var. Yine de kaldırmak istiyor musunuz?",
                    "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes) return;
            }

            issueMap[area].RemoveAll(x => string.Equals(x, issue, StringComparison.OrdinalIgnoreCase));
            hasChanges = true;

            listBoxIssues.Items.Remove(issue);
        }

        // =========================================================================
        // =========================  Genel Butonlar  ===============================
        // =========================================================================

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Alan/Alt Alan
                DatabaseHelper.SaveAreaSubAreaMap(areaSubAreaMap);    // ref: :contentReference[oaicite:22]{index=22}

                // Hatlar (Alt Alan → Hat)
                DatabaseHelper.SaveSubAreaLineMap(subAreaLineMap);    // YENİ  // ref: :contentReference[oaicite:23]{index=23}

                // Sorunlar (Alan → Sorun)
                DatabaseHelper.SaveIssueMap(issueMap);                // ref: :contentReference[oaicite:24]{index=24}

                hasChanges = false;
                MessageBox.Show("Ayarlar başarıyla kaydedildi.", "Bilgi");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Ayarlar kaydedilirken hata oluştu.", "Hata");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (hasChanges)
            {
                var res = MessageBox.Show("Kaydedilmemiş değişiklikler var. Vazgeçmek istiyor musunuz?",
                    "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes) return;
            }
            Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Tüm ayarları varsayılana döndürmek istiyor musunuz?",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            // Kataloglardan sıfırla (seed)
            areaSubAreaMap = AreaCatalog.GetAreaSubAreaMap();   // ref: :contentReference[oaicite:25]{index=25}
            subAreaLineMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase); // Hatları boş bırak
            issueMap = IssueCatalog.GetIssueMap();               // ref: :contentReference[oaicite:26]{index=26}
            hasChanges = true;
            RefreshUI();
            listBoxSubAreas.Items.Clear();
            listBoxLines.Items.Clear();
            listBoxIssues.Items.Clear();
        }

        // =========================================================================
        // =========================  UI Oluşturma  =================================
        // =========================================================================

        /// <summary>
        /// Alan & Alt Alan & Hat yönetim tab'ının görsel yerleşimi.
        /// (Designer, bu metodu çağırır)
        /// </summary>
        private void CreateAreaManagementTab()
        {
            tabAreas = new TabPage("Alan Yönetimi");
            tabAreas.BackColor = Color.White;
            tabAreas.Padding = new Padding(10);

            // ----- Ana alanlar grubu -----
            var groupBoxAreas = new GroupBox
            {
                Text = "Ana Alanlar (UAP / FES …)",
                Location = new Point(10, 10),
                Size = new Size(340, 300),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            listBoxAreas = new ListBox
            {
                Location = new Point(10, 25),
                Size = new Size(320, 180),
                Font = new Font("Segoe UI", 9F)
            };
            listBoxAreas.SelectedIndexChanged += ListBoxAreas_SelectedIndexChanged;

            textBoxNewArea = new TextBox
            {
                Location = new Point(10, 215),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9F)
            };

            btnAddArea = new Button
            {
                Text = "Ekle",
                Location = new Point(220, 215),
                Size = new Size(50, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F)
            };
            btnAddArea.Click += BtnAddArea_Click;

            btnDeleteArea = new Button
            {
                Text = "Sil",
                Location = new Point(280, 215),
                Size = new Size(50, 25),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F)
            };
            btnDeleteArea.Click += BtnDeleteArea_Click;

            var lblWarning = new Label
            {
                Text = "⚠️ Mevcut ticket'ları olan alan/alt alan/hat silinemez",
                Location = new Point(10, 250),
                Size = new Size(320, 20),
                ForeColor = Color.FromArgb(230, 126, 34),
                Font = new Font("Segoe UI", 8F)
            };

            groupBoxAreas.Controls.AddRange(new Control[] { listBoxAreas, textBoxNewArea, btnAddArea, btnDeleteArea, lblWarning });

            // ----- Alt alanlar grubu -----
            var groupBoxSubAreas = new GroupBox
            {
                Text = "Alt Alanlar",
                Location = new Point(360, 10),
                Size = new Size(340, 180),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            lblSelectedArea = new Label
            {
                Text = "Seçilen Alan: Yok",
                Location = new Point(10, 25),
                Size = new Size(320, 20),
                ForeColor = Color.FromArgb(52, 73, 94),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            listBoxSubAreas = new ListBox
            {
                Location = new Point(10, 50),
                Size = new Size(320, 85),
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };
            listBoxSubAreas.SelectedIndexChanged += ListBoxSubAreas_SelectedIndexChanged;

            textBoxNewSubArea = new TextBox
            {
                Location = new Point(10, 140),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };

            btnAddSubArea = new Button
            {
                Text = "Ekle",
                Location = new Point(220, 140),
                Size = new Size(50, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };
            btnAddSubArea.Click += BtnAddSubArea_Click;

            btnDeleteSubArea = new Button
            {
                Text = "Sil",
                Location = new Point(280, 140),
                Size = new Size(50, 25),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };
            btnDeleteSubArea.Click += BtnDeleteSubArea_Click;

            groupBoxSubAreas.Controls.AddRange(new Control[] { lblSelectedArea, listBoxSubAreas, textBoxNewSubArea, btnAddSubArea, btnDeleteSubArea });

            // ----- Hatlar (Line) grubu -----
            var groupBoxLines = new GroupBox
            {
                Text = "Hatlar (Line)",
                Location = new Point(360, 200),
                Size = new Size(340, 180),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            lblSelectedSubArea = new Label
            {
                Text = "Seçilen Alt Alan: Yok",
                Location = new Point(10, 25),
                Size = new Size(320, 20),
                ForeColor = Color.FromArgb(52, 73, 94),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            listBoxLines = new ListBox
            {
                Location = new Point(10, 50),
                Size = new Size(320, 85),
                Font = new Font("Segoe UI", 9F)
            };

            textBoxNewLine = new TextBox
            {
                Location = new Point(10, 140),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };

            btnAddLine = new Button
            {
                Text = "Ekle",
                Location = new Point(220, 140),
                Size = new Size(50, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };
            btnAddLine.Click += BtnAddLine_Click;

            btnDeleteLine = new Button
            {
                Text = "Sil",
                Location = new Point(280, 140),
                Size = new Size(50, 25),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };
            btnDeleteLine.Click += BtnDeleteLine_Click;

            groupBoxLines.Controls.AddRange(new Control[] { lblSelectedSubArea, listBoxLines, textBoxNewLine, btnAddLine, btnDeleteLine });

            // Kılavuz
            var lblGuide = new Label
            {
                Text = "📋 Kullanım: Önce Alan seçin → Alt Alan ekleyin/seçin → Hat ekleyin/silin.\n" +
                       "Değişiklikleri kaydetmek için 'Kaydet' butonunu kullanın.",
                Location = new Point(10, 390),
                Size = new Size(690, 40),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141)
            };

            tabAreas.Controls.AddRange(new Control[] { groupBoxAreas, groupBoxSubAreas, groupBoxLines, lblGuide });
            tabControl.TabPages.Add(tabAreas);
        }

        /// <summary>
        /// Sorun yönetimi tab'ının görsel yerleşimi.
        /// </summary>
        private void CreateIssueManagementTab()
        {
            tabIssues = new TabPage("Sorun Yönetimi")
            {
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            var lblSelect = new Label
            {
                Text = "Alan Seçiniz:",
                Location = new Point(10, 15),
                Size = new Size(100, 22)
            };

            comboBoxIssueArea = new ComboBox
            {
                Location = new Point(110, 12),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxIssueArea.SelectedIndexChanged += ComboBoxIssueArea_SelectedIndexChanged;

            listBoxIssues = new ListBox
            {
                Location = new Point(10, 50),
                Size = new Size(320, 300),
                Font = new Font("Segoe UI", 9F)
            };

            textBoxNewIssue = new TextBox
            {
                Location = new Point(350, 50),
                Size = new Size(220, 23),
                Font = new Font("Segoe UI", 9F)
            };

            btnAddIssue = new Button
            {
                Text = "Ekle",
                Location = new Point(580, 50),
                Size = new Size(60, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddIssue.Click += BtnAddIssue_Click;

            btnDeleteIssue = new Button
            {
                Text = "Sil",
                Location = new Point(650, 50),
                Size = new Size(60, 25),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteIssue.Click += BtnDeleteIssue_Click;

            var lblTip = new Label
            {
                Text = "Not: Sorun tipleri alan bazlıdır (ör. UAP-1 için ayrı, UAP-2 için ayrı).",
                Location = new Point(350, 85),
                Size = new Size(360, 40),
                ForeColor = Color.FromArgb(127, 140, 141)
            };

            tabIssues.Controls.AddRange(new Control[] {
                lblSelect, comboBoxIssueArea, listBoxIssues, textBoxNewIssue, btnAddIssue, btnDeleteIssue, lblTip
            });
            tabControl.TabPages.Add(tabIssues);
        }

        /// <summary>
        /// Formun altındaki Genel butonları oluşturur.
        /// </summary>
        private void CreateGeneralButtons()
        {
            btnSave = new Button
            {
                Text = "Kaydet",
                Location = new Point(10, 520),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Vazgeç",
                Location = new Point(120, 520),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += BtnCancel_Click;

            btnReset = new Button
            {
                Text = "Varsayılanlara Dön",
                Location = new Point(230, 520),
                Size = new Size(140, 30),
                BackColor = Color.FromArgb(243, 156, 18),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReset.Click += BtnReset_Click;

            this.Controls.AddRange(new Control[] { btnSave, btnCancel, btnReset });
        }
    }
}
