using System.Drawing;
using System.Windows.Forms;

namespace TicketApp.Forms
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAreas = new System.Windows.Forms.TabPage();
            this.btnDeleteSubArea = new System.Windows.Forms.Button();
            this.btnAddSubArea = new System.Windows.Forms.Button();
            this.textBoxNewSubArea = new System.Windows.Forms.TextBox();
            this.listBoxSubAreas = new System.Windows.Forms.ListBox();
            this.lblSelectedArea = new System.Windows.Forms.Label();
            this.btnDeleteArea = new System.Windows.Forms.Button();
            this.btnAddArea = new System.Windows.Forms.Button();
            this.textBoxNewArea = new System.Windows.Forms.TextBox();
            this.listBoxAreas = new System.Windows.Forms.ListBox();
            this.tabIssues = new System.Windows.Forms.TabPage();
            this.btnDeleteIssue = new System.Windows.Forms.Button();
            this.btnAddIssue = new System.Windows.Forms.Button();
            this.textBoxNewIssue = new System.Windows.Forms.TextBox();
            this.listBoxIssues = new System.Windows.Forms.ListBox();
            this.comboBoxIssueArea = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabAreas.SuspendLayout();
            this.tabIssues.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAreas);
            this.tabControl.Controls.Add(this.tabIssues);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(784, 450);
            this.tabControl.TabIndex = 0;
            // 
            // tabAreas
            // 
            this.tabAreas.Controls.Add(this.btnDeleteSubArea);
            this.tabAreas.Controls.Add(this.btnAddSubArea);
            this.tabAreas.Controls.Add(this.textBoxNewSubArea);
            this.tabAreas.Controls.Add(this.listBoxSubAreas);
            this.tabAreas.Controls.Add(this.lblSelectedArea);
            this.tabAreas.Controls.Add(this.btnDeleteArea);
            this.tabAreas.Controls.Add(this.btnAddArea);
            this.tabAreas.Controls.Add(this.textBoxNewArea);
            this.tabAreas.Controls.Add(this.listBoxAreas);
            this.tabAreas.Location = new System.Drawing.Point(4, 24);
            this.tabAreas.Name = "tabAreas";
            this.tabAreas.Padding = new System.Windows.Forms.Padding(3);
            this.tabAreas.Size = new System.Drawing.Size(776, 422);
            this.tabAreas.TabIndex = 0;
            this.tabAreas.Text = "Alan Yönetimi";
            this.tabAreas.UseVisualStyleBackColor = true;
            this.tabAreas.Click += new System.EventHandler(this.tabAreas_Click);
            // 
            // btnDeleteSubArea
            // 
            this.btnDeleteSubArea.Enabled = false;
            this.btnDeleteSubArea.Location = new System.Drawing.Point(695, 270);
            this.btnDeleteSubArea.Name = "btnDeleteSubArea";
            this.btnDeleteSubArea.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteSubArea.TabIndex = 8;
            this.btnDeleteSubArea.Text = "Sil";
            this.btnDeleteSubArea.UseVisualStyleBackColor = true;
            // 
            // btnAddSubArea
            // 
            this.btnAddSubArea.Enabled = false;
            this.btnAddSubArea.Location = new System.Drawing.Point(610, 270);
            this.btnAddSubArea.Name = "btnAddSubArea";
            this.btnAddSubArea.Size = new System.Drawing.Size(75, 23);
            this.btnAddSubArea.TabIndex = 7;
            this.btnAddSubArea.Text = "Ekle";
            this.btnAddSubArea.UseVisualStyleBackColor = true;
            // 
            // textBoxNewSubArea
            // 
            this.textBoxNewSubArea.Enabled = false;
            this.textBoxNewSubArea.Location = new System.Drawing.Point(400, 270);
            this.textBoxNewSubArea.Name = "textBoxNewSubArea";
            this.textBoxNewSubArea.Size = new System.Drawing.Size(200, 23);
            this.textBoxNewSubArea.TabIndex = 6;
            // 
            // listBoxSubAreas
            // 
            this.listBoxSubAreas.Enabled = false;
            this.listBoxSubAreas.FormattingEnabled = true;
            this.listBoxSubAreas.ItemHeight = 15;
            this.listBoxSubAreas.Location = new System.Drawing.Point(400, 50);
            this.listBoxSubAreas.Name = "listBoxSubAreas";
            this.listBoxSubAreas.Size = new System.Drawing.Size(250, 199);
            this.listBoxSubAreas.TabIndex = 5;
            // 
            // lblSelectedArea
            // 
            this.lblSelectedArea.AutoSize = true;
            this.lblSelectedArea.Location = new System.Drawing.Point(400, 20);
            this.lblSelectedArea.Name = "lblSelectedArea";
            this.lblSelectedArea.Size = new System.Drawing.Size(96, 15);
            this.lblSelectedArea.TabIndex = 4;
            this.lblSelectedArea.Text = "Seçilen Alan: Yok";
            // 
            // btnDeleteArea
            // 
            this.btnDeleteArea.Location = new System.Drawing.Point(315, 240);
            this.btnDeleteArea.Name = "btnDeleteArea";
            this.btnDeleteArea.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteArea.TabIndex = 3;
            this.btnDeleteArea.Text = "Sil";
            this.btnDeleteArea.UseVisualStyleBackColor = true;
            // 
            // btnAddArea
            // 
            this.btnAddArea.Location = new System.Drawing.Point(230, 240);
            this.btnAddArea.Name = "btnAddArea";
            this.btnAddArea.Size = new System.Drawing.Size(75, 23);
            this.btnAddArea.TabIndex = 2;
            this.btnAddArea.Text = "Ekle";
            this.btnAddArea.UseVisualStyleBackColor = true;
            // 
            // textBoxNewArea
            // 
            this.textBoxNewArea.Location = new System.Drawing.Point(20, 240);
            this.textBoxNewArea.Name = "textBoxNewArea";
            this.textBoxNewArea.Size = new System.Drawing.Size(200, 23);
            this.textBoxNewArea.TabIndex = 1;
            // 
            // listBoxAreas
            // 
            this.listBoxAreas.FormattingEnabled = true;
            this.listBoxAreas.ItemHeight = 15;
            this.listBoxAreas.Location = new System.Drawing.Point(20, 20);
            this.listBoxAreas.Name = "listBoxAreas";
            this.listBoxAreas.Size = new System.Drawing.Size(250, 199);
            this.listBoxAreas.TabIndex = 0;
            // 
            // tabIssues
            // 
            this.tabIssues.Controls.Add(this.btnDeleteIssue);
            this.tabIssues.Controls.Add(this.btnAddIssue);
            this.tabIssues.Controls.Add(this.textBoxNewIssue);
            this.tabIssues.Controls.Add(this.listBoxIssues);
            this.tabIssues.Controls.Add(this.comboBoxIssueArea);
            this.tabIssues.Location = new System.Drawing.Point(4, 24);
            this.tabIssues.Name = "tabIssues";
            this.tabIssues.Padding = new System.Windows.Forms.Padding(3);
            this.tabIssues.Size = new System.Drawing.Size(776, 422);
            this.tabIssues.TabIndex = 1;
            this.tabIssues.Text = "Sorun Yönetimi";
            this.tabIssues.UseVisualStyleBackColor = true;
            // 
            // btnDeleteIssue
            // 
            this.btnDeleteIssue.Enabled = false;
            this.btnDeleteIssue.Location = new System.Drawing.Point(415, 280);
            this.btnDeleteIssue.Name = "btnDeleteIssue";
            this.btnDeleteIssue.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteIssue.TabIndex = 4;
            this.btnDeleteIssue.Text = "Sil";
            this.btnDeleteIssue.UseVisualStyleBackColor = true;
            // 
            // btnAddIssue
            // 
            this.btnAddIssue.Enabled = false;
            this.btnAddIssue.Location = new System.Drawing.Point(330, 280);
            this.btnAddIssue.Name = "btnAddIssue";
            this.btnAddIssue.Size = new System.Drawing.Size(75, 23);
            this.btnAddIssue.TabIndex = 3;
            this.btnAddIssue.Text = "Ekle";
            this.btnAddIssue.UseVisualStyleBackColor = true;
            // 
            // textBoxNewIssue
            // 
            this.textBoxNewIssue.Enabled = false;
            this.textBoxNewIssue.Location = new System.Drawing.Point(20, 280);
            this.textBoxNewIssue.Name = "textBoxNewIssue";
            this.textBoxNewIssue.Size = new System.Drawing.Size(300, 23);
            this.textBoxNewIssue.TabIndex = 2;
            // 
            // listBoxIssues
            // 
            this.listBoxIssues.Enabled = false;
            this.listBoxIssues.FormattingEnabled = true;
            this.listBoxIssues.ItemHeight = 15;
            this.listBoxIssues.Location = new System.Drawing.Point(20, 60);
            this.listBoxIssues.Name = "listBoxIssues";
            this.listBoxIssues.Size = new System.Drawing.Size(400, 199);
            this.listBoxIssues.TabIndex = 1;
            // 
            // comboBoxIssueArea
            // 
            this.comboBoxIssueArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIssueArea.FormattingEnabled = true;
            this.comboBoxIssueArea.Location = new System.Drawing.Point(20, 20);
            this.comboBoxIssueArea.Name = "comboBoxIssueArea";
            this.comboBoxIssueArea.Size = new System.Drawing.Size(250, 23);
            this.comboBoxIssueArea.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(20, 470);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(240, 470);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(130, 470);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 35);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Sıfırla";
            this.btnReset.UseVisualStyleBackColor = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 521);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ayarlar";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabAreas.ResumeLayout(false);
            this.tabAreas.PerformLayout();
            this.tabIssues.ResumeLayout(false);
            this.tabIssues.PerformLayout();
            this.ResumeLayout(false);

        }

        private void CreateAreaManagementTab()
        {
            // Tab page oluştur
            this.tabAreas = new TabPage("Alan Yönetimi");
            this.tabAreas.BackColor = Color.White;
            this.tabAreas.Padding = new Padding(10);

            // Ana alanlar grubu
            var groupBoxAreas = new GroupBox();
            groupBoxAreas.Text = "Ana Alanlar (UAP/FES)";
            groupBoxAreas.Location = new Point(10, 10);
            groupBoxAreas.Size = new Size(340, 300);
            groupBoxAreas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Ana alanlar listesi
            this.listBoxAreas = new ListBox();
            this.listBoxAreas.Location = new Point(10, 25);
            this.listBoxAreas.Size = new Size(320, 180);
            this.listBoxAreas.Font = new Font("Segoe UI", 9F);

            // Yeni alan textbox'ı
            this.textBoxNewArea = new TextBox();
            this.textBoxNewArea.Location = new Point(10, 215);
            this.textBoxNewArea.Size = new Size(200, 23);
            this.textBoxNewArea.Font = new Font("Segoe UI", 9F);

            // Alan ekle butonu
            this.btnAddArea = new Button();
            this.btnAddArea.Text = "Ekle";
            this.btnAddArea.Location = new Point(220, 215);
            this.btnAddArea.Size = new Size(50, 25);
            this.btnAddArea.BackColor = Color.FromArgb(46, 204, 113);
            this.btnAddArea.ForeColor = Color.White;
            this.btnAddArea.FlatStyle = FlatStyle.Flat;
            this.btnAddArea.Font = new Font("Segoe UI", 9F);

            // Alan sil butonu
            this.btnDeleteArea = new Button();
            this.btnDeleteArea.Text = "Sil";
            this.btnDeleteArea.Location = new Point(280, 215);
            this.btnDeleteArea.Size = new Size(50, 25);
            this.btnDeleteArea.BackColor = Color.FromArgb(231, 76, 60);
            this.btnDeleteArea.ForeColor = Color.White;
            this.btnDeleteArea.FlatStyle = FlatStyle.Flat;
            this.btnDeleteArea.Font = new Font("Segoe UI", 9F);

            // Uyarı etiketi
            var lblWarning = new Label();
            lblWarning.Text = "⚠️ Mevcut ticket'ları olan alanlar silinemez";
            lblWarning.Location = new Point(10, 250);
            lblWarning.Size = new Size(320, 20);
            lblWarning.ForeColor = Color.FromArgb(230, 126, 34);
            lblWarning.Font = new Font("Segoe UI", 8F);

            // Ana alanlar grubuna kontrolleri ekle
            groupBoxAreas.Controls.AddRange(new Control[] {
                this.listBoxAreas, this.textBoxNewArea, this.btnAddArea, this.btnDeleteArea, lblWarning
            });

            // Alt alanlar grubu
            var groupBoxSubAreas = new GroupBox();
            groupBoxSubAreas.Text = "Alt Alanlar";
            groupBoxSubAreas.Location = new Point(360, 10);
            groupBoxSubAreas.Size = new Size(340, 300);
            groupBoxSubAreas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Seçilen alan etiketi
            this.lblSelectedArea = new Label();
            this.lblSelectedArea.Text = "Seçilen Alan: Yok";
            this.lblSelectedArea.Location = new Point(10, 25);
            this.lblSelectedArea.Size = new Size(320, 20);
            this.lblSelectedArea.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblSelectedArea.ForeColor = Color.FromArgb(52, 73, 94);

            // Alt alanlar listesi
            this.listBoxSubAreas = new ListBox();
            this.listBoxSubAreas.Location = new Point(10, 50);
            this.listBoxSubAreas.Size = new Size(320, 155);
            this.listBoxSubAreas.Font = new Font("Segoe UI", 9F);
            this.listBoxSubAreas.Enabled = false;

            // Yeni alt alan textbox'ı
            this.textBoxNewSubArea = new TextBox();
            this.textBoxNewSubArea.Location = new Point(10, 215);
            this.textBoxNewSubArea.Size = new Size(200, 23);
            this.textBoxNewSubArea.Font = new Font("Segoe UI", 9F);
            this.textBoxNewSubArea.Enabled = false;

            // Alt alan ekle butonu
            this.btnAddSubArea = new Button();
            this.btnAddSubArea.Text = "Ekle";
            this.btnAddSubArea.Location = new Point(220, 215);
            this.btnAddSubArea.Size = new Size(50, 25);
            this.btnAddSubArea.BackColor = Color.FromArgb(46, 204, 113);
            this.btnAddSubArea.ForeColor = Color.White;
            this.btnAddSubArea.FlatStyle = FlatStyle.Flat;
            this.btnAddSubArea.Font = new Font("Segoe UI", 9F);
            this.btnAddSubArea.Enabled = false;

            // Alt alan sil butonu
            this.btnDeleteSubArea = new Button();
            this.btnDeleteSubArea.Text = "Sil";
            this.btnDeleteSubArea.Location = new Point(280, 215);
            this.btnDeleteSubArea.Size = new Size(50, 25);
            this.btnDeleteSubArea.BackColor = Color.FromArgb(231, 76, 60);
            this.btnDeleteSubArea.ForeColor = Color.White;
            this.btnDeleteSubArea.FlatStyle = FlatStyle.Flat;
            this.btnDeleteSubArea.Font = new Font("Segoe UI", 9F);
            this.btnDeleteSubArea.Enabled = false;

            // Alt alanlar grubuna kontrolleri ekle
            groupBoxSubAreas.Controls.AddRange(new Control[] {
                this.lblSelectedArea, this.listBoxSubAreas, this.textBoxNewSubArea, this.btnAddSubArea, this.btnDeleteSubArea
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

            // Tab'a kontrolleri ekle
            this.tabAreas.Controls.AddRange(new Control[] { groupBoxAreas, groupBoxSubAreas, lblGuide });
            this.tabControl.TabPages.Add(this.tabAreas);
        }

        private void CreateIssueManagementTab()
        {
            // Tab page oluştur
            this.tabIssues = new TabPage("Sorun Yönetimi");
            this.tabIssues.BackColor = Color.White;
            this.tabIssues.Padding = new Padding(10);

            // Alan seçimi etiketi
            var lblSelectArea = new Label();
            lblSelectArea.Text = "Sorun eklemek istediğiniz alanı seçin:";
            lblSelectArea.Location = new Point(10, 20);
            lblSelectArea.Size = new Size(300, 20);
            lblSelectArea.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Alan seçimi combobox'ı
            this.comboBoxIssueArea = new ComboBox();
            this.comboBoxIssueArea.Location = new Point(10, 45);
            this.comboBoxIssueArea.Size = new Size(200, 23);
            this.comboBoxIssueArea.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxIssueArea.Font = new Font("Segoe UI", 9F);

            // Sorunlar grubu
            var groupBoxIssues = new GroupBox();
            groupBoxIssues.Text = "Sorunlar";
            groupBoxIssues.Location = new Point(10, 80);
            groupBoxIssues.Size = new Size(700, 300);
            groupBoxIssues.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Sorunlar listesi
            this.listBoxIssues = new ListBox();
            this.listBoxIssues.Location = new Point(10, 25);
            this.listBoxIssues.Size = new Size(680, 180);
            this.listBoxIssues.Font = new Font("Segoe UI", 9F);
            this.listBoxIssues.Enabled = false;

            // Yeni sorun textbox'ı
            this.textBoxNewIssue = new TextBox();
            this.textBoxNewIssue.Location = new Point(10, 215);
            this.textBoxNewIssue.Size = new Size(500, 23);
            this.textBoxNewIssue.Font = new Font("Segoe UI", 9F);
            this.textBoxNewIssue.Enabled = false;

            // Sorun ekle butonu
            this.btnAddIssue = new Button();
            this.btnAddIssue.Text = "Sorun Ekle";
            this.btnAddIssue.Location = new Point(520, 215);
            this.btnAddIssue.Size = new Size(80, 25);
            this.btnAddIssue.BackColor = Color.FromArgb(46, 204, 113);
            this.btnAddIssue.ForeColor = Color.White;
            this.btnAddIssue.FlatStyle = FlatStyle.Flat;
            this.btnAddIssue.Font = new Font("Segoe UI", 9F);
            this.btnAddIssue.Enabled = false;

            // Sorun sil butonu
            this.btnDeleteIssue = new Button();
            this.btnDeleteIssue.Text = "Sil";
            this.btnDeleteIssue.Location = new Point(610, 215);
            this.btnDeleteIssue.Size = new Size(80, 25);
            this.btnDeleteIssue.BackColor = Color.FromArgb(231, 76, 60);
            this.btnDeleteIssue.ForeColor = Color.White;
            this.btnDeleteIssue.FlatStyle = FlatStyle.Flat;
            this.btnDeleteIssue.Font = new Font("Segoe UI", 9F);
            this.btnDeleteIssue.Enabled = false;

            // Sorun bilgi etiketi
            var lblIssueInfo = new Label();
            lblIssueInfo.Text = "ℹ️ GENEL kategorisindeki sorunlar tüm alanlarda görünür";
            lblIssueInfo.Location = new Point(10, 250);
            lblIssueInfo.Size = new Size(680, 20);
            lblIssueInfo.Font = new Font("Segoe UI", 8F);
            lblIssueInfo.ForeColor = Color.FromArgb(52, 152, 219);

            // Sorunlar grubuna kontrolleri ekle
            groupBoxIssues.Controls.AddRange(new Control[] {
                this.listBoxIssues, this.textBoxNewIssue, this.btnAddIssue, this.btnDeleteIssue, lblIssueInfo
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

            // Tab'a kontrolleri ekle
            this.tabIssues.Controls.AddRange(new Control[] {
                lblSelectArea, this.comboBoxIssueArea, groupBoxIssues, lblIssueGuide
            });
            this.tabControl.TabPages.Add(this.tabIssues);
        }

        private void CreateGeneralButtons()
        {
            // Kaydet butonu
            this.btnSave = new Button();
            this.btnSave.Text = "💾 Kaydet";
            this.btnSave.Location = new Point(520, 520);
            this.btnSave.Size = new Size(80, 30);
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Sıfırla butonu
            this.btnReset = new Button();
            this.btnReset.Text = "🔄 Sıfırla";
            this.btnReset.Location = new Point(610, 520);
            this.btnReset.Size = new Size(80, 30);
            this.btnReset.BackColor = Color.FromArgb(230, 126, 34);
            this.btnReset.ForeColor = Color.White;
            this.btnReset.FlatStyle = FlatStyle.Flat;
            this.btnReset.Font = new Font("Segoe UI", 9F);

            // İptal butonu
            this.btnCancel = new Button();
            this.btnCancel.Text = "❌ İptal";
            this.btnCancel.Location = new Point(700, 520);
            this.btnCancel.Size = new Size(80, 30);
            this.btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9F);

            // Form'a butonları ekle
            this.Controls.AddRange(new Control[] { this.btnSave, this.btnReset, this.btnCancel });
        }

        #endregion
    }
}