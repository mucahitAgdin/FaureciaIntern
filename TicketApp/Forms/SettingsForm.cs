using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TicketApp.Helpers;

namespace TicketApp.Forms
{
    public partial class SettingsForm : Form
    {
        private Dictionary<string, List<string>> areaSubAreaMap;
        private Dictionary<string, List<string>> issueMap;
        private bool hasChanges = false;

        // UI Controls
        private TabControl tabControl;
        private TabPage tabAreas;
        private TabPage tabIssues;

        // Alan yönetimi kontrolleri
        private ListBox listBoxAreas;
        private TextBox textBoxNewArea;
        private Button btnAddArea;
        private Button btnDeleteArea;
        private Label lblSelectedArea;
        private ListBox listBoxSubAreas;
        private TextBox textBoxNewSubArea;
        private Button btnAddSubArea;
        private Button btnDeleteSubArea;

        // Sorun yönetimi kontrolleri
        private ComboBox comboBoxIssueArea;
        private ListBox listBoxIssues;
        private TextBox textBoxNewIssue;
        private Button btnAddIssue;
        private Button btnDeleteIssue;

        // Genel kontroller
        private Button btnSave;
        private Button btnCancel;
        private Button btnReset;

        public SettingsForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde yapılacak işlemler
            LoadData();
        }


        private void CreateAreaManagementTab()
        {
            tabAreas = new TabPage("Alan Yönetimi");
            tabAreas.BackColor = Color.White;
            tabAreas.Padding = new Padding(10);

            // Ana alanlar grubu
            var groupBoxAreas = new GroupBox();
            groupBoxAreas.Text = "Ana Alanlar (UAP/FES)";
            groupBoxAreas.Location = new Point(10, 10);
            groupBoxAreas.Size = new Size(340, 300);
            groupBoxAreas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            listBoxAreas = new ListBox();
            listBoxAreas.Location = new Point(10, 25);
            listBoxAreas.Size = new Size(320, 180);
            listBoxAreas.Font = new Font("Segoe UI", 9F);
            listBoxAreas.SelectedIndexChanged += ListBoxAreas_SelectedIndexChanged;

            textBoxNewArea = new TextBox();
            textBoxNewArea.Location = new Point(10, 215);
            textBoxNewArea.Size = new Size(200, 23);
            textBoxNewArea.Font = new Font("Segoe UI", 9F);

            // PlaceholderText özelliği .NET Framework'te mevcut olmayabilir
            // Bu durumda sadece kaldırabilirsiniz veya alternatif kullanabilirsiniz
            try
            {
                textBoxNewArea.GetType().GetProperty("PlaceholderText")?.SetValue(textBoxNewArea, "Yeni alan adı (örn: UAP-5)");
            }
            catch
            {
                // PlaceholderText desteklenmiyorsa, varsayılan metin kullan
                textBoxNewArea.Text = "Yeni alan adı (örn: UAP-5)";
                textBoxNewArea.ForeColor = Color.Gray;
                textBoxNewArea.Enter += (s, e) => {
                    if (textBoxNewArea.Text == "Yeni alan adı (örn: UAP-5)")
                    {
                        textBoxNewArea.Text = "";
                        textBoxNewArea.ForeColor = Color.Black;
                    }
                };
                textBoxNewArea.Leave += (s, e) => {
                    if (string.IsNullOrEmpty(textBoxNewArea.Text))
                    {
                        textBoxNewArea.Text = "Yeni alan adı (örn: UAP-5)";
                        textBoxNewArea.ForeColor = Color.Gray;
                    }
                };
            }

            btnAddArea = new Button();
            btnAddArea.Text = "Ekle";
            btnAddArea.Location = new Point(220, 215);
            btnAddArea.Size = new Size(50, 25);
            btnAddArea.BackColor = Color.FromArgb(46, 204, 113);
            btnAddArea.ForeColor = Color.White;
            btnAddArea.FlatStyle = FlatStyle.Flat;
            btnAddArea.Font = new Font("Segoe UI", 9F);
            btnAddArea.Click += BtnAddArea_Click;

            btnDeleteArea = new Button();
            btnDeleteArea.Text = "Sil";
            btnDeleteArea.Location = new Point(280, 215);
            btnDeleteArea.Size = new Size(50, 25);
            btnDeleteArea.BackColor = Color.FromArgb(231, 76, 60);
            btnDeleteArea.ForeColor = Color.White;
            btnDeleteArea.FlatStyle = FlatStyle.Flat;
            btnDeleteArea.Font = new Font("Segoe UI", 9F);
            btnDeleteArea.Click += BtnDeleteArea_Click;

            var lblWarning = new Label();
            lblWarning.Text = "⚠️ Mevcut ticket'ları olan alanlar silinemez";
            lblWarning.Location = new Point(10, 250);
            lblWarning.Size = new Size(320, 20);
            lblWarning.ForeColor = Color.FromArgb(230, 126, 34);
            lblWarning.Font = new Font("Segoe UI", 8F);

            groupBoxAreas.Controls.AddRange(new Control[] {
                listBoxAreas, textBoxNewArea, btnAddArea, btnDeleteArea, lblWarning
            });

            // Alt alanlar grubu
            var groupBoxSubAreas = new GroupBox();
            groupBoxSubAreas.Text = "Alt Alanlar";
            groupBoxSubAreas.Location = new Point(360, 10);
            groupBoxSubAreas.Size = new Size(340, 300);
            groupBoxSubAreas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            lblSelectedArea = new Label();
            lblSelectedArea.Text = "Seçilen Alan: Yok";
            lblSelectedArea.Location = new Point(10, 25);
            lblSelectedArea.Size = new Size(320, 20);
            lblSelectedArea.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSelectedArea.ForeColor = Color.FromArgb(52, 73, 94);

            listBoxSubAreas = new ListBox();
            listBoxSubAreas.Location = new Point(10, 50);
            listBoxSubAreas.Size = new Size(320, 155);
            listBoxSubAreas.Font = new Font("Segoe UI", 9F);
            listBoxSubAreas.Enabled = false;

            textBoxNewSubArea = new TextBox();
            textBoxNewSubArea.Location = new Point(10, 215);
            textBoxNewSubArea.Size = new Size(200, 23);
            textBoxNewSubArea.Font = new Font("Segoe UI", 9F);
            textBoxNewSubArea.Enabled = false;

            btnAddSubArea = new Button();
            btnAddSubArea.Text = "Ekle";
            btnAddSubArea.Location = new Point(220, 215);
            btnAddSubArea.Size = new Size(50, 25);
            btnAddSubArea.BackColor = Color.FromArgb(46, 204, 113);
            btnAddSubArea.ForeColor = Color.White;
            btnAddSubArea.FlatStyle = FlatStyle.Flat;
            btnAddSubArea.Font = new Font("Segoe UI", 9F);
            btnAddSubArea.Enabled = false;
            btnAddSubArea.Click += BtnAddSubArea_Click;

            btnDeleteSubArea = new Button();
            btnDeleteSubArea.Text = "Sil";
            btnDeleteSubArea.Location = new Point(280, 215);
            btnDeleteSubArea.Size = new Size(50, 25);
            btnDeleteSubArea.BackColor = Color.FromArgb(231, 76, 60);
            btnDeleteSubArea.ForeColor = Color.White;
            btnDeleteSubArea.FlatStyle = FlatStyle.Flat;
            btnDeleteSubArea.Font = new Font("Segoe UI", 9F);
            btnDeleteSubArea.Enabled = false;
            btnDeleteSubArea.Click += BtnDeleteSubArea_Click;

            groupBoxSubAreas.Controls.AddRange(new Control[] {
                lblSelectedArea, listBoxSubAreas, textBoxNewSubArea, btnAddSubArea, btnDeleteSubArea
            });

            // Kullanım kılavuzu
            var lblGuide = new Label();
            lblGuide.Text = "📋 Kullanım Kılavuzu:\n" +
                          "• Ana alanlar (UAP-1, UAP-2, FES vb.) sisteminizin temel bölümleridir\n" +
                          "• Her ana alana ait alt alanlar tanımlayabilirsiniz\n" +
                          "• Değişiklikleri kaydetmek için 'Kaydet' butonunu kullanın";
            lblGuide.Location = new Point(10, 320);
            lblGuide.Size = new Size(700, 60);
            lblGuide.Font = new Font("Segoe UI", 9F);
            lblGuide.ForeColor = Color.FromArgb(127, 140, 141);

            tabAreas.Controls.AddRange(new Control[] { groupBoxAreas, groupBoxSubAreas, lblGuide });
            tabControl.TabPages.Add(tabAreas);
        }

        private void CreateIssueManagementTab()
        {
            tabIssues = new TabPage("Sorun Yönetimi");
            tabIssues.BackColor = Color.White;
            tabIssues.Padding = new Padding(10);

            // Alan seçimi
            var lblSelectArea = new Label();
            lblSelectArea.Text = "Sorun eklemek istediğiniz alanı seçin:";
            lblSelectArea.Location = new Point(10, 20);
            lblSelectArea.Size = new Size(300, 20);
            lblSelectArea.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            comboBoxIssueArea = new ComboBox();
            comboBoxIssueArea.Location = new Point(10, 45);
            comboBoxIssueArea.Size = new Size(200, 23);
            comboBoxIssueArea.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxIssueArea.Font = new Font("Segoe UI", 9F);
            comboBoxIssueArea.SelectedIndexChanged += ComboBoxIssueArea_SelectedIndexChanged;

            // Sorunlar grubu
            var groupBoxIssues = new GroupBox();
            groupBoxIssues.Text = "Sorunlar";
            groupBoxIssues.Location = new Point(10, 80);
            groupBoxIssues.Size = new Size(700, 300);
            groupBoxIssues.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            listBoxIssues = new ListBox();
            listBoxIssues.Location = new Point(10, 25);
            listBoxIssues.Size = new Size(680, 180);
            listBoxIssues.Font = new Font("Segoe UI", 9F);
            listBoxIssues.Enabled = false;

            textBoxNewIssue = new TextBox();
            textBoxNewIssue.Location = new Point(10, 215);
            textBoxNewIssue.Size = new Size(500, 23);
            textBoxNewIssue.Font = new Font("Segoe UI", 9F);
            textBoxNewIssue.Enabled = false;

            btnAddIssue = new Button();
            btnAddIssue.Text = "Sorun Ekle";
            btnAddIssue.Location = new Point(520, 215);
            btnAddIssue.Size = new Size(80, 25);
            btnAddIssue.BackColor = Color.FromArgb(46, 204, 113);
            btnAddIssue.ForeColor = Color.White;
            btnAddIssue.FlatStyle = FlatStyle.Flat;
            btnAddIssue.Font = new Font("Segoe UI", 9F);
            btnAddIssue.Enabled = false;
            btnAddIssue.Click += BtnAddIssue_Click;

            btnDeleteIssue = new Button();
            btnDeleteIssue.Text = "Sil";
            btnDeleteIssue.Location = new Point(610, 215);
            btnDeleteIssue.Size = new Size(80, 25);
            btnDeleteIssue.BackColor = Color.FromArgb(231, 76, 60);
            btnDeleteIssue.ForeColor = Color.White;
            btnDeleteIssue.FlatStyle = FlatStyle.Flat;
            btnDeleteIssue.Font = new Font("Segoe UI", 9F);
            btnDeleteIssue.Enabled = false;
            btnDeleteIssue.Click += BtnDeleteIssue_Click;

            var lblIssueInfo = new Label();
            lblIssueInfo.Text = "ℹ️ GENEL kategorisindeki sorunlar tüm alanlarda görünür";
            lblIssueInfo.Location = new Point(10, 250);
            lblIssueInfo.Size = new Size(680, 20);
            lblIssueInfo.Font = new Font("Segoe UI", 8F);
            lblIssueInfo.ForeColor = Color.FromArgb(52, 152, 219);

            groupBoxIssues.Controls.AddRange(new Control[] {
                listBoxIssues, textBoxNewIssue, btnAddIssue, btnDeleteIssue, lblIssueInfo
            });

            // Kullanım kılavuzu
            var lblIssueGuide = new Label();
            lblIssueGuide.Text = "📋 Sorun Yönetimi:\n" +
                              "• Her alan için özel sorun tipleri tanımlayabilirsiniz\n" +
                              "• GENEL kategorisindeki sorunlar tüm alanlarda görünür\n" +
                              "• Mevcut ticket'ları olan sorunlar silinemez";
            lblIssueGuide.Location = new Point(10, 390);
            lblIssueGuide.Size = new Size(700, 60);
            lblIssueGuide.Font = new Font("Segoe UI", 9F);
            lblIssueGuide.ForeColor = Color.FromArgb(127, 140, 141);

            tabIssues.Controls.AddRange(new Control[] {
                lblSelectArea, comboBoxIssueArea, groupBoxIssues, lblIssueGuide
            });
            tabControl.TabPages.Add(tabIssues);
        }

        private void CreateGeneralButtons()
        {
            btnSave = new Button();
            btnSave.Text = "💾 Kaydet";
            btnSave.Location = new Point(520, 520);
            btnSave.Size = new Size(80, 30);
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.Click += BtnSave_Click;

            btnReset = new Button();
            btnReset.Text = "🔄 Sıfırla";
            btnReset.Location = new Point(610, 520);
            btnReset.Size = new Size(80, 30);
            btnReset.BackColor = Color.FromArgb(230, 126, 34);
            btnReset.ForeColor = Color.White;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 9F);
            btnReset.Click += BtnReset_Click;

            btnCancel = new Button();
            btnCancel.Text = "❌ İptal";
            btnCancel.Location = new Point(700, 520);
            btnCancel.Size = new Size(80, 30);
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.Click += BtnCancel_Click;

            this.Controls.AddRange(new Control[] { btnSave, btnReset, btnCancel });
        }

        private void LoadData()
        {
            try
            {
                // Veritabanından mevcut alan ve sorun verilerini yükle
                areaSubAreaMap = DatabaseHelper.GetAreaSubAreaMap();
                issueMap = DatabaseHelper.GetIssueMap();

                // Eğer veriler yoksa varsayılan değerleri kullan
                if (areaSubAreaMap == null || areaSubAreaMap.Count == 0)
                {
                    areaSubAreaMap = AreaCatalog.GetAreaSubAreaMap();
                }

                if (issueMap == null || issueMap.Count == 0)
                {
                    issueMap = IssueCatalog.GetIssueMap();
                }

                RefreshUI();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Veriler yüklenirken hata oluştu. Lütfen logları kontrol edin.", "Hata",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshUI()
        {
            if (listBoxAreas != null)
            {
                // Alan listesini güncelle
                listBoxAreas.Items.Clear();
                foreach (var area in areaSubAreaMap.Keys)
                {
                    listBoxAreas.Items.Add(area);
                }
            }

            if (comboBoxIssueArea != null)
            {
                // Sorun alanları combobox'ını güncelle
                comboBoxIssueArea.Items.Clear();
                foreach (var area in issueMap.Keys)
                {
                    comboBoxIssueArea.Items.Add(area);
                }
            }
        }

        private void ListBoxAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem != null)
            {
                string selectedArea = listBoxAreas.SelectedItem.ToString();
                lblSelectedArea.Text = $"Seçilen Alan: {selectedArea}";

                // Alt alanları yükle
                listBoxSubAreas.Items.Clear();
                if (areaSubAreaMap.ContainsKey(selectedArea))
                {
                    foreach (var subArea in areaSubAreaMap[selectedArea])
                    {
                        listBoxSubAreas.Items.Add(subArea);
                    }
                }

                // Alt alan kontrollerini aktif et
                listBoxSubAreas.Enabled = true;
                textBoxNewSubArea.Enabled = true;
                btnAddSubArea.Enabled = true;
                btnDeleteSubArea.Enabled = true;
            }
        }

        private void ComboBoxIssueArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxIssueArea.SelectedItem != null)
            {
                string selectedArea = comboBoxIssueArea.SelectedItem.ToString();

                // Sorunları yükle
                listBoxIssues.Items.Clear();
                if (issueMap.ContainsKey(selectedArea))
                {
                    foreach (var issue in issueMap[selectedArea])
                    {
                        listBoxIssues.Items.Add(issue);
                    }
                }

                // Sorun kontrollerini aktif et
                listBoxIssues.Enabled = true;
                textBoxNewIssue.Enabled = true;
                btnAddIssue.Enabled = true;
                btnDeleteIssue.Enabled = true;
            }
        }

        private void BtnAddArea_Click(object sender, EventArgs e)
        {
            string newArea = textBoxNewArea.Text.Trim();

            // Placeholder text kontrolü
            if (string.IsNullOrEmpty(newArea) || newArea == "Yeni alan adı (örn: UAP-5)")
            {
                MessageBox.Show("Lütfen alan adını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (areaSubAreaMap.ContainsKey(newArea))
            {
                MessageBox.Show("Bu alan zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            areaSubAreaMap[newArea] = new List<string>();
            issueMap[newArea] = new List<string>();

            textBoxNewArea.Clear();
            hasChanges = true;
            RefreshUI();

            MessageBox.Show($"'{newArea}' alanı başarıyla eklendi.", "Başarılı",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteArea_Click(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silinecek alanı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedArea = listBoxAreas.SelectedItem.ToString();

            // Mevcut ticket kontrolü
            if (DatabaseHelper.HasTicketsForArea(selectedArea))
            {
                MessageBox.Show("Bu alana ait mevcut ticket'lar bulunduğu için silinemez.",
                              "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"'{selectedArea}' alanını silmek istediğinizden emin misiniz?\n" +
                                       "Bu işlem geri alınamaz.", "Onay",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                areaSubAreaMap.Remove(selectedArea);
                issueMap.Remove(selectedArea);
                hasChanges = true;
                RefreshUI();

                // Alt alan kontrollerini devre dışı bırak
                listBoxSubAreas.Items.Clear();
                listBoxSubAreas.Enabled = false;
                textBoxNewSubArea.Enabled = false;
                btnAddSubArea.Enabled = false;
                btnDeleteSubArea.Enabled = false;
                lblSelectedArea.Text = "Seçilen Alan: Yok";

                MessageBox.Show($"'{selectedArea}' alanı başarıyla silindi.", "Başarılı",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddSubArea_Click(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem == null)
            {
                MessageBox.Show("Lütfen önce bir alan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newSubArea = textBoxNewSubArea.Text.Trim();
            if (string.IsNullOrEmpty(newSubArea))
            {
                MessageBox.Show("Lütfen alt alan adını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedArea = listBoxAreas.SelectedItem.ToString();

            if (areaSubAreaMap[selectedArea].Contains(newSubArea))
            {
                MessageBox.Show("Bu alt alan zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            areaSubAreaMap[selectedArea].Add(newSubArea);
            listBoxSubAreas.Items.Add(newSubArea);
            textBoxNewSubArea.Clear();
            hasChanges = true;

            MessageBox.Show($"'{newSubArea}' alt alanı başarıyla eklendi.", "Başarılı",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteSubArea_Click(object sender, EventArgs e)
        {
            if (listBoxAreas.SelectedItem == null || listBoxSubAreas.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silinecek alt alanı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedArea = listBoxAreas.SelectedItem.ToString();
            string selectedSubArea = listBoxSubAreas.SelectedItem.ToString();

            // Mevcut ticket kontrolü
            if (DatabaseHelper.HasTicketsForSubArea(selectedArea, selectedSubArea))
            {
                MessageBox.Show("Bu alt alana ait mevcut ticket'lar bulunduğu için silinemez.",
                              "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"'{selectedSubArea}' alt alanını silmek istediğinizden emin misiniz?",
                                       "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                areaSubAreaMap[selectedArea].Remove(selectedSubArea);
                listBoxSubAreas.Items.Remove(selectedSubArea);
                hasChanges = true;

                MessageBox.Show($"'{selectedSubArea}' alt alanı başarıyla silindi.", "Başarılı",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddIssue_Click(object sender, EventArgs e)
        {
            if (comboBoxIssueArea.SelectedItem == null)
            {
                MessageBox.Show("Lütfen önce bir alan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newIssue = textBoxNewIssue.Text.Trim();
            if (string.IsNullOrEmpty(newIssue))
            {
                MessageBox.Show("Lütfen sorun tanımını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedArea = comboBoxIssueArea.SelectedItem.ToString();

            if (issueMap[selectedArea].Contains(newIssue))
            {
                MessageBox.Show("Bu sorun zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            issueMap[selectedArea].Add(newIssue);
            listBoxIssues.Items.Add(newIssue);
            textBoxNewIssue.Clear();
            hasChanges = true;

            MessageBox.Show($"'{newIssue}' sorunu başarıyla eklendi.", "Başarılı",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteIssue_Click(object sender, EventArgs e)
        {
            if (comboBoxIssueArea.SelectedItem == null || listBoxIssues.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silinecek sorunu seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedArea = comboBoxIssueArea.SelectedItem.ToString();
            string selectedIssue = listBoxIssues.SelectedItem.ToString();

            // Mevcut ticket kontrolü
            if (DatabaseHelper.HasTicketsForIssue(selectedIssue))
            {
                MessageBox.Show("Bu sorun tipine ait mevcut ticket'lar bulunduğu için silinemez.",
                              "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"'{selectedIssue}' sorununu silmek istediğinizden emin misiniz?",
                                       "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                issueMap[selectedArea].Remove(selectedIssue);
                listBoxIssues.Items.Remove(selectedIssue);
                hasChanges = true;

                MessageBox.Show($"'{selectedIssue}' sorunu başarıyla silindi.", "Başarılı",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!hasChanges)
            {
                MessageBox.Show("Kaydedilecek değişiklik bulunmuyor.", "Bilgi",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Veritabanına kaydet
                DatabaseHelper.SaveAreaSubAreaMap(areaSubAreaMap);
                DatabaseHelper.SaveIssueMap(issueMap);

                hasChanges = false;
                MessageBox.Show("Tüm değişiklikler başarıyla kaydedildi.", "Başarılı",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Kaydetme işlemi sırasında hata oluştu. Lütfen logları kontrol edin.", "Hata",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Tüm değişiklikler geri alınacak. Emin misiniz?", "Onay",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                hasChanges = false;
                LoadData();

                // Form kontrollerini sıfırla
                listBoxSubAreas.Items.Clear();
                listBoxSubAreas.Enabled = false;
                textBoxNewSubArea.Enabled = false;
                btnAddSubArea.Enabled = false;
                btnDeleteSubArea.Enabled = false;
                lblSelectedArea.Text = "Seçilen Alan: Yok";

                listBoxIssues.Items.Clear();
                listBoxIssues.Enabled = false;
                textBoxNewIssue.Enabled = false;
                btnAddIssue.Enabled = false;
                btnDeleteIssue.Enabled = false;

                // Text kutularını temizle
                textBoxNewArea.Clear();
                textBoxNewSubArea.Clear();
                textBoxNewIssue.Clear();

                // Seçimleri temizle
                listBoxAreas.ClearSelected();
                comboBoxIssueArea.SelectedIndex = -1;

                MessageBox.Show("Tüm değişiklikler geri alındı.", "Başarılı",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (hasChanges)
            {
                var result = MessageBox.Show("Kaydedilmemiş değişiklikler var. Çıkmak istediğinizden emin misiniz?",
                                           "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (hasChanges)
            {
                var result = MessageBox.Show("Kaydedilmemiş değişiklikler var. Çıkmak istediğinizden emin misiniz?",
                                           "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnFormClosing(e);
        }
    }
}