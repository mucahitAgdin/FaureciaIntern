using System.Drawing;
using System.Windows.Forms;

namespace TicketApp.Forms
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;

        // Ana kontroller
        private Label lblWelcome;
        private Label lblTotalCount;
        private Button btnSettings;
        private Button btnLogout;
        private Button btnRefresh;
        private TextBox txtDescription;
        private Label lblSelectedTicket;

        // Bekleyen ticket'lar bölümü
        private GroupBox gbBekleyen;
        private DataGridView dgvBekleyen;
        private Label lblBekleyenCount;
        private Button btnMarkInProgress;

        // İşlemdeki ticket'lar bölümü  
        private GroupBox gbIslemde;
        private DataGridView dgvIslemde;
        private Label lblIslemdeCount;
        private Button btnMarkCompleted;

        // Çözülen ticket'lar bölümü
        private GroupBox gbCozulen;
        private DataGridView dgvCozulen;
        private Label lblCozulenCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblSelectedTicket = new System.Windows.Forms.Label();
            this.gbBekleyen = new System.Windows.Forms.GroupBox();
            this.dgvBekleyen = new System.Windows.Forms.DataGridView();
            this.lblBekleyenCount = new System.Windows.Forms.Label();
            this.btnMarkInProgress = new System.Windows.Forms.Button();
            this.gbIslemde = new System.Windows.Forms.GroupBox();
            this.dgvIslemde = new System.Windows.Forms.DataGridView();
            this.lblIslemdeCount = new System.Windows.Forms.Label();
            this.btnMarkCompleted = new System.Windows.Forms.Button();
            this.gbCozulen = new System.Windows.Forms.GroupBox();
            this.dgvCozulen = new System.Windows.Forms.DataGridView();
            this.lblCozulenCount = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbBekleyen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBekleyen)).BeginInit();
            this.gbIslemde.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIslemde)).BeginInit();
            this.gbCozulen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCozulen)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblWelcome.Location = new System.Drawing.Point(20, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(233, 25);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Hoş geldiniz, [username]";
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotalCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblTotalCount.Location = new System.Drawing.Point(20, 55);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(78, 20);
            this.lblTotalCount.TabIndex = 1;
            this.lblTotalCount.Text = "Toplam: 0";
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(20, 90);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 35);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "Ayarlar";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(130, 90);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 35);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Çıkış Yap";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(240, 90);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 35);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Yenile";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescription.Location = new System.Drawing.Point(20, 470);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(1080, 80);
            this.txtDescription.TabIndex = 9;
            this.txtDescription.Text = "Bir ticket seçin...";
            // 
            // lblSelectedTicket
            // 
            this.lblSelectedTicket.AutoSize = true;
            this.lblSelectedTicket.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectedTicket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblSelectedTicket.Location = new System.Drawing.Point(20, 440);
            this.lblSelectedTicket.Name = "lblSelectedTicket";
            this.lblSelectedTicket.Size = new System.Drawing.Size(133, 19);
            this.lblSelectedTicket.TabIndex = 8;
            this.lblSelectedTicket.Text = "Seçilen Ticket: Yok";
            // 
            // gbBekleyen
            // 
            this.gbBekleyen.Controls.Add(this.dgvBekleyen);
            this.gbBekleyen.Controls.Add(this.lblBekleyenCount);
            this.gbBekleyen.Controls.Add(this.btnMarkInProgress);
            this.gbBekleyen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbBekleyen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.gbBekleyen.Location = new System.Drawing.Point(20, 140);
            this.gbBekleyen.Name = "gbBekleyen";
            this.gbBekleyen.Size = new System.Drawing.Size(350, 280);
            this.gbBekleyen.TabIndex = 5;
            this.gbBekleyen.TabStop = false;
            this.gbBekleyen.Text = "📋 BEKLEYEN TICKET\'LAR";
            // 
            // dgvBekleyen
            // 
            this.dgvBekleyen.AllowUserToAddRows = false;
            this.dgvBekleyen.AllowUserToDeleteRows = false;
            this.dgvBekleyen.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.dgvBekleyen.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBekleyen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBekleyen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgvBekleyen.Location = new System.Drawing.Point(10, 50);
            this.dgvBekleyen.MultiSelect = false;
            this.dgvBekleyen.Name = "dgvBekleyen";
            this.dgvBekleyen.ReadOnly = true;
            this.dgvBekleyen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBekleyen.Size = new System.Drawing.Size(330, 180);
            this.dgvBekleyen.TabIndex = 0;
            this.dgvBekleyen.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            this.dgvBekleyen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseClick);
            // 
            // lblBekleyenCount
            // 
            this.lblBekleyenCount.AutoSize = true;
            this.lblBekleyenCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBekleyenCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblBekleyenCount.Location = new System.Drawing.Point(10, 25);
            this.lblBekleyenCount.Name = "lblBekleyenCount";
            this.lblBekleyenCount.Size = new System.Drawing.Size(72, 15);
            this.lblBekleyenCount.TabIndex = 1;
            this.lblBekleyenCount.Text = "Bekleyen: 0";
            // 
            // btnMarkInProgress
            // 
            this.btnMarkInProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnMarkInProgress.FlatAppearance.BorderSize = 0;
            this.btnMarkInProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkInProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMarkInProgress.ForeColor = System.Drawing.Color.White;
            this.btnMarkInProgress.Location = new System.Drawing.Point(10, 240);
            this.btnMarkInProgress.Name = "btnMarkInProgress";
            this.btnMarkInProgress.Size = new System.Drawing.Size(150, 30);
            this.btnMarkInProgress.TabIndex = 2;
            this.btnMarkInProgress.Text = "▶️ İşleme Al";
            this.btnMarkInProgress.UseVisualStyleBackColor = false;
            this.btnMarkInProgress.Click += new System.EventHandler(this.btnMarkInProgress_Click);
            // 
            // gbIslemde
            // 
            this.gbIslemde.Controls.Add(this.dgvIslemde);
            this.gbIslemde.Controls.Add(this.lblIslemdeCount);
            this.gbIslemde.Controls.Add(this.btnMarkCompleted);
            this.gbIslemde.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbIslemde.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.gbIslemde.Location = new System.Drawing.Point(385, 140);
            this.gbIslemde.Name = "gbIslemde";
            this.gbIslemde.Size = new System.Drawing.Size(350, 280);
            this.gbIslemde.TabIndex = 6;
            this.gbIslemde.TabStop = false;
            this.gbIslemde.Text = "⚙️ İŞLEMDEKİ TICKET\'LAR";
            // 
            // dgvIslemde
            // 
            this.dgvIslemde.AllowUserToAddRows = false;
            this.dgvIslemde.AllowUserToDeleteRows = false;
            this.dgvIslemde.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.dgvIslemde.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvIslemde.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIslemde.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.dgvIslemde.Location = new System.Drawing.Point(10, 50);
            this.dgvIslemde.MultiSelect = false;
            this.dgvIslemde.Name = "dgvIslemde";
            this.dgvIslemde.ReadOnly = true;
            this.dgvIslemde.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIslemde.Size = new System.Drawing.Size(330, 180);
            this.dgvIslemde.TabIndex = 0;
            this.dgvIslemde.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            this.dgvIslemde.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseClick);
            // 
            // lblIslemdeCount
            // 
            this.lblIslemdeCount.AutoSize = true;
            this.lblIslemdeCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblIslemdeCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblIslemdeCount.Location = new System.Drawing.Point(10, 25);
            this.lblIslemdeCount.Name = "lblIslemdeCount";
            this.lblIslemdeCount.Size = new System.Drawing.Size(64, 15);
            this.lblIslemdeCount.TabIndex = 1;
            this.lblIslemdeCount.Text = "İşlemde: 0";
            // 
            // btnMarkCompleted
            // 
            this.btnMarkCompleted.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnMarkCompleted.FlatAppearance.BorderSize = 0;
            this.btnMarkCompleted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkCompleted.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMarkCompleted.ForeColor = System.Drawing.Color.White;
            this.btnMarkCompleted.Location = new System.Drawing.Point(10, 240);
            this.btnMarkCompleted.Name = "btnMarkCompleted";
            this.btnMarkCompleted.Size = new System.Drawing.Size(150, 30);
            this.btnMarkCompleted.TabIndex = 2;
            this.btnMarkCompleted.Text = "✅ Tamamlandı";
            this.btnMarkCompleted.UseVisualStyleBackColor = false;
            this.btnMarkCompleted.Click += new System.EventHandler(this.btnMarkCompleted_Click);
            // 
            // gbCozulen
            // 
            this.gbCozulen.Controls.Add(this.dgvCozulen);
            this.gbCozulen.Controls.Add(this.lblCozulenCount);
            this.gbCozulen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbCozulen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.gbCozulen.Location = new System.Drawing.Point(750, 140);
            this.gbCozulen.Name = "gbCozulen";
            this.gbCozulen.Size = new System.Drawing.Size(350, 280);
            this.gbCozulen.TabIndex = 7;
            this.gbCozulen.TabStop = false;
            this.gbCozulen.Text = "✅ ÇÖZÜLEN TICKET\'LAR";
            // 
            // dgvCozulen
            // 
            this.dgvCozulen.AllowUserToAddRows = false;
            this.dgvCozulen.AllowUserToDeleteRows = false;
            this.dgvCozulen.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(240)))));
            this.dgvCozulen.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCozulen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCozulen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12});
            this.dgvCozulen.Location = new System.Drawing.Point(10, 50);
            this.dgvCozulen.MultiSelect = false;
            this.dgvCozulen.Name = "dgvCozulen";
            this.dgvCozulen.ReadOnly = true;
            this.dgvCozulen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCozulen.Size = new System.Drawing.Size(330, 220);
            this.dgvCozulen.TabIndex = 0;
            this.dgvCozulen.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCozulen_CellContentClick);
            this.dgvCozulen.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            this.dgvCozulen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseClick);
            // 
            // lblCozulenCount
            // 
            this.lblCozulenCount.AutoSize = true;
            this.lblCozulenCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCozulenCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblCozulenCount.Location = new System.Drawing.Point(10, 25);
            this.lblCozulenCount.Name = "lblCozulenCount";
            this.lblCozulenCount.Size = new System.Drawing.Size(64, 15);
            this.lblCozulenCount.TabIndex = 1;
            this.lblCozulenCount.Text = "Çözülen: 0";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Alan";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Sorun";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Tarih";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "ID";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Alan";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Sorun";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Tarih";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "ID";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Alan";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Sorun";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Tarih";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1258, 649);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTotalCount);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.gbBekleyen);
            this.Controls.Add(this.gbIslemde);
            this.Controls.Add(this.gbCozulen);
            this.Controls.Add(this.lblSelectedTicket);
            this.Controls.Add(this.txtDescription);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🎫 Admin Paneli - Ticket Yönetimi";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.gbBekleyen.ResumeLayout(false);
            this.gbBekleyen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBekleyen)).EndInit();
            this.gbIslemde.ResumeLayout(false);
            this.gbIslemde.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIslemde)).EndInit();
            this.gbCozulen.ResumeLayout(false);
            this.gbCozulen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCozulen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
    }
}