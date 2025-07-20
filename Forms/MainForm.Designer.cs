// TicketApp/Forms/MainForm.Designer.cs
namespace TicketApp.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Form üzerindeki bileşenleri tutan değişken. Dispose işlemi için gerekir.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Görsel bileşenler (UI)
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.Panel panelHistory;
        private System.Windows.Forms.GroupBox groupBoxNewTicket;
        private System.Windows.Forms.GroupBox groupBoxHistory;
        private System.Windows.Forms.ComboBox comboBoxArea;
        private System.Windows.Forms.ComboBox comboBoxSubArea;
        private System.Windows.Forms.ComboBox comboBoxIssue;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.TextBox textBoxPhoneNumber;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListBox listBoxTickets;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelSubArea;
        private System.Windows.Forms.Label labelIssue;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.Label labelPhoneNumber;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelCharCount;
        private System.Windows.Forms.PictureBox pictureBoxIcon;

        /// <summary>
        /// Bellek temizliği için kullanılan metod (otomatik çağrılır)
        /// </summary>
        /// <param name="disposing">Bileşenler manuel olarak mı yoksa otomatik mi dispose ediliyor?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Form üzerindeki tüm bileşenleri başlatır ve yerleştirir
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelForm = new System.Windows.Forms.Panel();
            this.groupBoxNewTicket = new System.Windows.Forms.GroupBox();
            this.labelArea = new System.Windows.Forms.Label();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.labelSubArea = new System.Windows.Forms.Label();
            this.comboBoxSubArea = new System.Windows.Forms.ComboBox();
            this.labelIssue = new System.Windows.Forms.Label();
            this.comboBoxIssue = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelCharCount = new System.Windows.Forms.Label();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();
            this.labelLastName = new System.Windows.Forms.Label();
            this.textBoxLastName = new System.Windows.Forms.TextBox();
            this.labelPhoneNumber = new System.Windows.Forms.Label();
            this.textBoxPhoneNumber = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.panelHistory = new System.Windows.Forms.Panel();
            this.groupBoxHistory = new System.Windows.Forms.GroupBox();
            this.listBoxTickets = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.groupBoxNewTicket.SuspendLayout();
            this.panelHistory.SuspendLayout();
            this.groupBoxHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelHeader.Controls.Add(this.pictureBoxIcon);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(900, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxIcon.Location = new System.Drawing.Point(15, 15);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxIcon.TabIndex = 1;
            this.pictureBoxIcon.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(55, 18);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(252, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "IT Destek Talep Sistemi";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelMain.Controls.Add(this.panelForm);
            this.panelMain.Controls.Add(this.panelHistory);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 60);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(10);
            this.panelMain.Size = new System.Drawing.Size(900, 540);
            this.panelMain.TabIndex = 1;
            // 
            // panelForm
            // 
            this.panelForm.Controls.Add(this.groupBoxNewTicket);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelForm.Location = new System.Drawing.Point(10, 10);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(430, 520);
            this.panelForm.TabIndex = 0;
            // 
            // groupBoxNewTicket
            // 
            this.groupBoxNewTicket.BackColor = System.Drawing.Color.White;
            this.groupBoxNewTicket.Controls.Add(this.labelArea);
            this.groupBoxNewTicket.Controls.Add(this.comboBoxArea);
            this.groupBoxNewTicket.Controls.Add(this.labelSubArea);
            this.groupBoxNewTicket.Controls.Add(this.comboBoxSubArea);
            this.groupBoxNewTicket.Controls.Add(this.labelIssue);
            this.groupBoxNewTicket.Controls.Add(this.comboBoxIssue);
            this.groupBoxNewTicket.Controls.Add(this.labelDescription);
            this.groupBoxNewTicket.Controls.Add(this.textBoxDescription);
            this.groupBoxNewTicket.Controls.Add(this.labelCharCount);
            this.groupBoxNewTicket.Controls.Add(this.labelFirstName);
            this.groupBoxNewTicket.Controls.Add(this.textBoxFirstName);
            this.groupBoxNewTicket.Controls.Add(this.labelLastName);
            this.groupBoxNewTicket.Controls.Add(this.textBoxLastName);
            this.groupBoxNewTicket.Controls.Add(this.labelPhoneNumber);
            this.groupBoxNewTicket.Controls.Add(this.textBoxPhoneNumber);
            this.groupBoxNewTicket.Controls.Add(this.btnSubmit);
            this.groupBoxNewTicket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxNewTicket.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxNewTicket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxNewTicket.Location = new System.Drawing.Point(0, 0);
            this.groupBoxNewTicket.Name = "groupBoxNewTicket";
            this.groupBoxNewTicket.Padding = new System.Windows.Forms.Padding(15);
            this.groupBoxNewTicket.Size = new System.Drawing.Size(430, 520);
            this.groupBoxNewTicket.TabIndex = 0;
            this.groupBoxNewTicket.TabStop = false;
            this.groupBoxNewTicket.Text = "🎫 Yeni Destek Talebi";
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelArea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelArea.Location = new System.Drawing.Point(18, 35);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(106, 15);
            this.labelArea.TabIndex = 0;
            this.labelArea.Text = "🏢 Alan (UAP/FES):";
            // 
            // comboBoxArea
            // 
            this.comboBoxArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxArea.FormattingEnabled = true;
            this.comboBoxArea.Location = new System.Drawing.Point(21, 53);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(385, 23);
            this.comboBoxArea.TabIndex = 1;
            // 
            // labelSubArea
            // 
            this.labelSubArea.AutoSize = true;
            this.labelSubArea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelSubArea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelSubArea.Location = new System.Drawing.Point(18, 85);
            this.labelSubArea.Name = "labelSubArea";
            this.labelSubArea.Size = new System.Drawing.Size(65, 15);
            this.labelSubArea.TabIndex = 2;
            this.labelSubArea.Text = "🏬 Alt Alan:";
            // 
            // comboBoxSubArea
            // 
            this.comboBoxSubArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubArea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxSubArea.FormattingEnabled = true;
            this.comboBoxSubArea.Location = new System.Drawing.Point(21, 103);
            this.comboBoxSubArea.Name = "comboBoxSubArea";
            this.comboBoxSubArea.Size = new System.Drawing.Size(385, 23);
            this.comboBoxSubArea.TabIndex = 3;
            // 
            // labelIssue
            // 
            this.labelIssue.AutoSize = true;
            this.labelIssue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelIssue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelIssue.Location = new System.Drawing.Point(18, 135);
            this.labelIssue.Name = "labelIssue";
            this.labelIssue.Size = new System.Drawing.Size(79, 15);
            this.labelIssue.TabIndex = 4;
            this.labelIssue.Text = "⚠️ Sorun Tipi:";
            // 
            // comboBoxIssue
            // 
            this.comboBoxIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIssue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxIssue.FormattingEnabled = true;
            this.comboBoxIssue.Location = new System.Drawing.Point(21, 153);
            this.comboBoxIssue.Name = "comboBoxIssue";
            this.comboBoxIssue.Size = new System.Drawing.Size(385, 23);
            this.comboBoxIssue.TabIndex = 5;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelDescription.Location = new System.Drawing.Point(18, 185);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(167, 15);
            this.labelDescription.TabIndex = 6;
            this.labelDescription.Text = "📝 Detaylı Açıklama (Max 300):";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxDescription.Location = new System.Drawing.Point(21, 203);
            this.textBoxDescription.MaxLength = 300;
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.Size = new System.Drawing.Size(385, 80);
            this.textBoxDescription.TabIndex = 7;
            // 
            // labelCharCount
            // 
            this.labelCharCount.AutoSize = true;
            this.labelCharCount.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelCharCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.labelCharCount.Location = new System.Drawing.Point(360, 287);
            this.labelCharCount.Name = "labelCharCount";
            this.labelCharCount.Size = new System.Drawing.Size(41, 13);
            this.labelCharCount.TabIndex = 8;
            this.labelCharCount.Text = "0 / 300";
            // 
            // labelFirstName
            // 
            this.labelFirstName.AutoSize = true;
            this.labelFirstName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelFirstName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelFirstName.Location = new System.Drawing.Point(18, 310);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(43, 15);
            this.labelFirstName.TabIndex = 9;
            this.labelFirstName.Text = "👤 Ad:";
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxFirstName.Location = new System.Drawing.Point(21, 328);
            this.textBoxFirstName.MaxLength = 50;
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.Size = new System.Drawing.Size(185, 23);
            this.textBoxFirstName.TabIndex = 10;
            // 
            // labelLastName
            // 
            this.labelLastName.AutoSize = true;
            this.labelLastName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelLastName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelLastName.Location = new System.Drawing.Point(221, 310);
            this.labelLastName.Name = "labelLastName";
            this.labelLastName.Size = new System.Drawing.Size(62, 15);
            this.labelLastName.TabIndex = 11;
            this.labelLastName.Text = "👤 Soyad:";
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxLastName.Location = new System.Drawing.Point(221, 328);
            this.textBoxLastName.MaxLength = 50;
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.Size = new System.Drawing.Size(185, 23);
            this.textBoxLastName.TabIndex = 12;
            // 
            // labelPhoneNumber
            // 
            this.labelPhoneNumber.AutoSize = true;
            this.labelPhoneNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelPhoneNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelPhoneNumber.Location = new System.Drawing.Point(18, 360);
            this.labelPhoneNumber.Name = "labelPhoneNumber";
            this.labelPhoneNumber.Size = new System.Drawing.Size(93, 15);
            this.labelPhoneNumber.TabIndex = 13;
            this.labelPhoneNumber.Text = "📞 Telefon No:";
            // 
            // textBoxPhoneNumber
            // 
            this.textBoxPhoneNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxPhoneNumber.Location = new System.Drawing.Point(21, 378);
            this.textBoxPhoneNumber.MaxLength = 20;
            this.textBoxPhoneNumber.Name = "textBoxPhoneNumber";
            this.textBoxPhoneNumber.Size = new System.Drawing.Size(385, 23);
            this.textBoxPhoneNumber.TabIndex = 14;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(21, 420);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(385, 40);
            this.btnSubmit.TabIndex = 15;
            this.btnSubmit.Text = "📤 Talebi Gönder";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // panelHistory
            // 
            this.panelHistory.Controls.Add(this.groupBoxHistory);
            this.panelHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistory.Location = new System.Drawing.Point(450, 10);
            this.panelHistory.Name = "panelHistory";
            this.panelHistory.Size = new System.Drawing.Size(440, 520);
            this.panelHistory.TabIndex = 1;
            // 
            // groupBoxHistory
            // 
            this.groupBoxHistory.BackColor = System.Drawing.Color.White;
            this.groupBoxHistory.Controls.Add(this.listBoxTickets);
            this.groupBoxHistory.Controls.Add(this.btnRefresh);
            this.groupBoxHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxHistory.Location = new System.Drawing.Point(0, 0);
            this.groupBoxHistory.Name = "groupBoxHistory";
            this.groupBoxHistory.Padding = new System.Windows.Forms.Padding(15);
            this.groupBoxHistory.Size = new System.Drawing.Size(440, 520);
            this.groupBoxHistory.TabIndex = 0;
            this.groupBoxHistory.TabStop = false;
            this.groupBoxHistory.Text = "📋 Talep Geçmişi";
            this.groupBoxHistory.Enter += new System.EventHandler(this.groupBoxHistory_Enter);
            // 
            // listBoxTickets
            // 
            this.listBoxTickets.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.listBoxTickets.FormattingEnabled = true;
            this.listBoxTickets.ItemHeight = 15;
            this.listBoxTickets.Location = new System.Drawing.Point(18, 30);
            this.listBoxTickets.Name = "listBoxTickets";
            this.listBoxTickets.Size = new System.Drawing.Size(404, 420);
            this.listBoxTickets.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(18, 465);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(404, 35);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "🔄 Listeyi Yenile";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IT Destek Talep Sistemi";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.groupBoxNewTicket.ResumeLayout(false);
            this.groupBoxNewTicket.PerformLayout();
            this.panelHistory.ResumeLayout(false);
            this.groupBoxHistory.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}