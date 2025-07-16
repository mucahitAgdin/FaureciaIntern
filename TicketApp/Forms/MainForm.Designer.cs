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
        private System.Windows.Forms.ComboBox comboBoxIssue;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListBox listBoxTickets;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelIssue;
        private System.Windows.Forms.Label labelDescription;
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelForm = new System.Windows.Forms.Panel();
            this.panelHistory = new System.Windows.Forms.Panel();
            this.groupBoxNewTicket = new System.Windows.Forms.GroupBox();
            this.groupBoxHistory = new System.Windows.Forms.GroupBox();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.comboBoxIssue = new System.Windows.Forms.ComboBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.listBoxTickets = new System.Windows.Forms.ListBox();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelIssue = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelCharCount = new System.Windows.Forms.Label();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();

            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.panelHistory.SuspendLayout();
            this.groupBoxNewTicket.SuspendLayout();
            this.groupBoxHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
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
            this.panelHeader.Size = new System.Drawing.Size(800, 60);
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
            //this.pictureBoxIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxIcon_Paint);

            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(55, 18);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(241, 30);
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
            this.panelMain.Size = new System.Drawing.Size(800, 490);
            this.panelMain.TabIndex = 1;

            // 
            // panelForm
            // 
            this.panelForm.Controls.Add(this.groupBoxNewTicket);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelForm.Location = new System.Drawing.Point(10, 10);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(380, 470);
            this.panelForm.TabIndex = 0;

            // 
            // panelHistory
            // 
            this.panelHistory.Controls.Add(this.groupBoxHistory);
            this.panelHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistory.Location = new System.Drawing.Point(390, 10);
            this.panelHistory.Name = "panelHistory";
            this.panelHistory.Size = new System.Drawing.Size(400, 470);
            this.panelHistory.TabIndex = 1;

            // 
            // groupBoxNewTicket
            // 
            this.groupBoxNewTicket.BackColor = System.Drawing.Color.White;
            this.groupBoxNewTicket.Controls.Add(this.labelArea);
            this.groupBoxNewTicket.Controls.Add(this.comboBoxArea);
            this.groupBoxNewTicket.Controls.Add(this.labelIssue);
            this.groupBoxNewTicket.Controls.Add(this.comboBoxIssue);
            this.groupBoxNewTicket.Controls.Add(this.labelDescription);
            this.groupBoxNewTicket.Controls.Add(this.textBoxDescription);
            this.groupBoxNewTicket.Controls.Add(this.labelCharCount);
            this.groupBoxNewTicket.Controls.Add(this.btnSubmit);
            this.groupBoxNewTicket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxNewTicket.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxNewTicket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxNewTicket.Location = new System.Drawing.Point(0, 0);
            this.groupBoxNewTicket.Name = "groupBoxNewTicket";
            this.groupBoxNewTicket.Padding = new System.Windows.Forms.Padding(15);
            this.groupBoxNewTicket.Size = new System.Drawing.Size(380, 470);
            this.groupBoxNewTicket.TabIndex = 0;
            this.groupBoxNewTicket.TabStop = false;
            this.groupBoxNewTicket.Text = "🎫 Yeni Destek Talebi";

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
            this.groupBoxHistory.Size = new System.Drawing.Size(400, 470);
            this.groupBoxHistory.TabIndex = 0;
            this.groupBoxHistory.TabStop = false;
            this.groupBoxHistory.Text = "📋 Talep Geçmişi";

            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelArea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelArea.Location = new System.Drawing.Point(18, 40);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(108, 15);
            this.labelArea.TabIndex = 0;
            this.labelArea.Text = "🏢 Alan (UAP/FES):";

            // 
            // comboBoxArea
            // 
            this.comboBoxArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxArea.FormattingEnabled = true;
            this.comboBoxArea.Location = new System.Drawing.Point(21, 60);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(335, 23);
            this.comboBoxArea.TabIndex = 1;

            // 
            // labelIssue
            // 
            this.labelIssue.AutoSize = true;
            this.labelIssue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelIssue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelIssue.Location = new System.Drawing.Point(18, 100);
            this.labelIssue.Name = "labelIssue";
            this.labelIssue.Size = new System.Drawing.Size(78, 15);
            this.labelIssue.TabIndex = 2;
            this.labelIssue.Text = "⚠️ Sorun Tipi:";

            // 
            // comboBoxIssue
            // 
            this.comboBoxIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIssue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxIssue.FormattingEnabled = true;
            this.comboBoxIssue.Location = new System.Drawing.Point(21, 120);
            this.comboBoxIssue.Name = "comboBoxIssue";
            this.comboBoxIssue.Size = new System.Drawing.Size(335, 23);
            this.comboBoxIssue.TabIndex = 3;

            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.labelDescription.Location = new System.Drawing.Point(18, 160);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(149, 15);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "📝 Detaylı Açıklama (Max 300):";

            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxDescription.Location = new System.Drawing.Point(21, 180);
            this.textBoxDescription.MaxLength = 300;
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.Size = new System.Drawing.Size(335, 120);
            this.textBoxDescription.TabIndex = 5;
            //this.textBoxDescription.TextChanged += new System.EventHandler(this.TextBoxDescription_TextChanged);

            // 
            // labelCharCount
            // 
            this.labelCharCount.AutoSize = true;
            this.labelCharCount.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelCharCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.labelCharCount.Location = new System.Drawing.Point(310, 305);
            this.labelCharCount.Name = "labelCharCount";
            this.labelCharCount.Size = new System.Drawing.Size(44, 13);
            this.labelCharCount.TabIndex = 6;
            this.labelCharCount.Text = "0 / 300";

            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(21, 330);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(335, 40);
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Text = "📤 Talebi Gönder";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            //this.btnSubmit.MouseEnter += new System.EventHandler(this.BtnSubmit_MouseEnter);
            //this.btnSubmit.MouseLeave += new System.EventHandler(this.BtnSubmit_MouseLeave);

            // 
            // listBoxTickets
            // 
            this.listBoxTickets.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.listBoxTickets.FormattingEnabled = true;
            this.listBoxTickets.ItemHeight = 15;
            this.listBoxTickets.Location = new System.Drawing.Point(18, 30);
            this.listBoxTickets.Name = "listBoxTickets";
            this.listBoxTickets.Size = new System.Drawing.Size(350, 364);
            this.listBoxTickets.TabIndex = 0;

            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(18, 410);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(350, 35);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "🔄 Listeyi Yenile";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 550);
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
            this.panelMain.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.panelHistory.ResumeLayout(false);
            this.groupBoxNewTicket.ResumeLayout(false);
            this.groupBoxNewTicket.PerformLayout();
            this.groupBoxHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
