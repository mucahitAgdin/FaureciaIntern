using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketApp.Forms
{
    public partial class SettingsForm : Form
    {
<<<<<<< HEAD
        private Dictionary<string, List<string>> areaSubAreaMap;
        private Dictionary<string, List<string>> issueMap;
        private bool hasChanges = false;

        // UI Controls (declarations moved to Designer.cs)
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
            SetupEventHandlers();
=======
        public SettingsForm()
        {
            InitializeComponent();
>>>>>>> parent of ce1ee67 (settings designs)
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

<<<<<<< HEAD
        private void SetupEventHandlers()
        {
            // Alan yönetimi event handlers
            this.listBoxAreas.SelectedIndexChanged += ListBoxAreas_SelectedIndexChanged;
            this.btnAddArea.Click += BtnAddArea_Click;
            this.btnDeleteArea.Click += BtnDeleteArea_Click;
            this.btnAddSubArea.Click += BtnAddSubArea_Click;
            this.btnDeleteSubArea.Click += BtnDeleteSubArea_Click;

            // Sorun yönetimi event handlers
            this.comboBoxIssueArea.SelectedIndexChanged += ComboBoxIssueArea_SelectedIndexChanged;
            this.btnAddIssue.Click += BtnAddIssue_Click;
            this.btnDeleteIssue.Click += BtnDeleteIssue_Click;

            // Genel buton event handlers
            this.btnSave.Click += BtnSave_Click;
            this.btnReset.Click += BtnReset_Click;
            this.btnCancel.Click += BtnCancel_Click;

            // PlaceholderText functionality for textBoxNewArea
            SetupPlaceholderText();
        }

        private void SetupPlaceholderText()
        {
            // PlaceholderText özelliği .NET Framework'te mevcut olmayabilir
            // Bu durumda alternatif kullanılıyor
            try
            {
                this.textBoxNewArea.GetType().GetProperty("PlaceholderText")?.SetValue(this.textBoxNewArea, "Yeni alan adı (örn: UAP-5)");
            }
            catch
            {
                // PlaceholderText desteklenmiyorsa, varsayılan metin kullan
                this.textBoxNewArea.Text = "Yeni alan adı (örn: UAP-5)";
                this.textBoxNewArea.ForeColor = Color.Gray;
                this.textBoxNewArea.Enter += (s, e) => {
                    if (this.textBoxNewArea.Text == "Yeni alan adı (örn: UAP-5)")
                    {
                        this.textBoxNewArea.Text = "";
                        this.textBoxNewArea.ForeColor = Color.Black;
                    }
                };
                this.textBoxNewArea.Leave += (s, e) => {
                    if (string.IsNullOrEmpty(this.textBoxNewArea.Text))
                    {
                        this.textBoxNewArea.Text = "Yeni alan adı (örn: UAP-5)";
                        this.textBoxNewArea.ForeColor = Color.Gray;
                    }
                };
            }
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

            var result = MessageBox.Show($"'{selectedSubArea}' alt alanını silmek istediğinizden emin misiniz?\n" +
                                       "Bu işlem geri alınamaz.", "Onay",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                MessageBox.Show("Bu soruna ait mevcut ticket'lar bulunduğu için silinemez.",
                              "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"'{selectedIssue}' sorununu silmek istediğinizden emin misiniz?\n" +
                                       "Bu işlem geri alınamaz.", "Onay",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
            try
            {
                if (!hasChanges)
                {
                    MessageBox.Show("Kaydedilecek değişiklik bulunmamaktadır.", "Bilgi",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show("Değişiklikleri kaydetmek istediğinizden emin misiniz?", "Onay",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Veritabanını güncelle
                    DatabaseHelper.SaveAreaSubAreaMap(areaSubAreaMap);
                    DatabaseHelper.SaveIssueMap(issueMap);

                    hasChanges = false;
                    MessageBox.Show("Değişiklikler başarıyla kaydedildi.", "Başarılı",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Kaydetme sırasında hata oluştu. Lütfen logları kontrol edin.", "Hata",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Tüm değişiklikleri geri almak istediğinizden emin misiniz?\n" +
                                       "Bu işlem geri alınamaz.", "Onay",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                LoadData();
                hasChanges = false;
                MessageBox.Show("Tüm değişiklikler geri alındı.", "Başarılı",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (hasChanges)
            {
                var result = MessageBox.Show("Kaydedilmemiş değişiklikler var. Çıkmak istediğinizden emin misiniz?",
                                           "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (hasChanges)
            {
                var result = MessageBox.Show("Kaydedilmemiş değişiklikler var. Çıkmak istediğinizden emin misiniz?",
                                           "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnFormClosing(e);
=======
>>>>>>> parent of ce1ee67 (settings designs)
        }

        private void tabAreas_Click(object sender, EventArgs e)
        {

        }
    }
}
