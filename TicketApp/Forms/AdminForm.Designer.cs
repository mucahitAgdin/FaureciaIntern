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
            this.lblWelcome = new Label();
            this.lblTotalCount = new Label();
            this.btnSettings = new Button();
            this.btnLogout = new Button();
            this.btnRefresh = new Button();
            this.txtDescription = new TextBox();
            this.lblSelectedTicket = new Label();

            // Bekleyen bölümü
            this.gbBekleyen = new GroupBox();
            this.dgvBekleyen = new DataGridView();
            this.lblBekleyenCount = new Label();
            this.btnMarkInProgress = new Button();

            // İşlemde bölümü
            this.gbIslemde = new GroupBox();
            this.dgvIslemde = new DataGridView();
            this.lblIslemdeCount = new Label();
            this.btnMarkCompleted = new Button();

            // Çözülen bölümü
            this.gbCozulen = new GroupBox();
            this.dgvCozulen = new DataGridView();
            this.lblCozulenCount = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvBekleyen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIslemde)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCozulen)).BeginInit();
            this.gbBekleyen.SuspendLayout();
            this.gbIslemde.SuspendLayout();
            this.gbCozulen.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblWelcome.ForeColor = Color.FromArgb(0, 122, 204);
            this.lblWelcome.Location = new Point(20, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new Size(200, 25);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Hoş geldiniz, [username]";

            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTotalCount.ForeColor = Color.FromArgb(68, 68, 68);
            this.lblTotalCount.Location = new Point(20, 55);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new Size(80, 20);
            this.lblTotalCount.TabIndex = 1;
            this.lblTotalCount.Text = "Toplam: 0";

            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = Color.FromArgb(0, 122, 204);
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = FlatStyle.Flat;
            this.btnSettings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSettings.ForeColor = Color.White;
            this.btnSettings.Location = new Point(20, 90);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new Size(100, 35);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "Ayarlar";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);

            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = FlatStyle.Flat;
            this.btnLogout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnLogout.ForeColor = Color.White;
            this.btnLogout.Location = new Point(130, 90);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new Size(100, 35);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Çıkış Yap";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = Color.FromArgb(40, 167, 69);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Location = new Point(240, 90);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(100, 35);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Yenile";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // 
            // BEKLEYEN BÖLÜMÜ
            // 
            this.gbBekleyen.Controls.Add(this.dgvBekleyen);
            this.gbBekleyen.Controls.Add(this.lblBekleyenCount);
            this.gbBekleyen.Controls.Add(this.btnMarkInProgress);
            this.gbBekleyen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.gbBekleyen.ForeColor = Color.FromArgb(255, 193, 7);
            this.gbBekleyen.Location = new Point(20, 140);
            this.gbBekleyen.Name = "gbBekleyen";
            this.gbBekleyen.Size = new Size(350, 280);
            this.gbBekleyen.TabIndex = 5;
            this.gbBekleyen.TabStop = false;
            this.gbBekleyen.Text = "📋 BEKLEYEN TICKET'LAR";

            this.dgvBekleyen.AllowUserToAddRows = false;
            this.dgvBekleyen.AllowUserToDeleteRows = false;
            this.dgvBekleyen.BackgroundColor = Color.FromArgb(255, 248, 220);
            this.dgvBekleyen.BorderStyle = BorderStyle.None;
            this.dgvBekleyen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBekleyen.Location = new Point(10, 50);
            this.dgvBekleyen.MultiSelect = false;
            this.dgvBekleyen.Name = "dgvBekleyen";
            this.dgvBekleyen.ReadOnly = true;
            this.dgvBekleyen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBekleyen.Size = new Size(330, 180);
            this.dgvBekleyen.TabIndex = 0;
            this.dgvBekleyen.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            this.dgvBekleyen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseClick);

            // Bekleyen grid kolonları
            this.dgvBekleyen.Columns.Add("ID", "ID");
            this.dgvBekleyen.Columns.Add("Area", "Alan");
            this.dgvBekleyen.Columns.Add("Issue", "Sorun");
            this.dgvBekleyen.Columns.Add("Date", "Tarih");
            this.dgvBekleyen.Columns[0].Width = 40;
            this.dgvBekleyen.Columns[1].Width = 80;
            this.dgvBekleyen.Columns[2].Width = 120;
            this.dgvBekleyen.Columns[3].Width = 80;

            this.lblBekleyenCount.AutoSize = true;
            this.lblBekleyenCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblBekleyenCount.ForeColor = Color.FromArgb(68, 68, 68);
            this.lblBekleyenCount.Location = new Point(10, 25);
            this.lblBekleyenCount.Name = "lblBekleyenCount";
            this.lblBekleyenCount.Size = new Size(80, 15);
            this.lblBekleyenCount.TabIndex = 1;
            this.lblBekleyenCount.Text = "Bekleyen: 0";

            this.btnMarkInProgress.BackColor = Color.FromArgb(0, 123, 255);
            this.btnMarkInProgress.FlatAppearance.BorderSize = 0;
            this.btnMarkInProgress.FlatStyle = FlatStyle.Flat;
            this.btnMarkInProgress.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnMarkInProgress.ForeColor = Color.White;
            this.btnMarkInProgress.Location = new Point(10, 240);
            this.btnMarkInProgress.Name = "btnMarkInProgress";
            this.btnMarkInProgress.Size = new Size(150, 30);
            this.btnMarkInProgress.TabIndex = 2;
            this.btnMarkInProgress.Text = "▶️ İşleme Al";
            this.btnMarkInProgress.UseVisualStyleBackColor = false;
            this.btnMarkInProgress.Click += new System.EventHandler(this.btnMarkInProgress_Click);

            // 
            // İŞLEMDE BÖLÜMÜ
            // 
            this.gbIslemde.Controls.Add(this.dgvIslemde);
            this.gbIslemde.Controls.Add(this.lblIslemdeCount);
            this.gbIslemde.Controls.Add(this.btnMarkCompleted);
            this.gbIslemde.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.gbIslemde.ForeColor = Color.FromArgb(0, 123, 255);
            this.gbIslemde.Location = new Point(385, 140);
            this.gbIslemde.Name = "gbIslemde";
            this.gbIslemde.Size = new Size(350, 280);
            this.gbIslemde.TabIndex = 6;
            this.gbIslemde.TabStop = false;
            this.gbIslemde.Text = "⚙️ İŞLEMDEKİ TICKET'LAR";

            this.dgvIslemde.AllowUserToAddRows = false;
            this.dgvIslemde.AllowUserToDeleteRows = false;
            this.dgvIslemde.BackgroundColor = Color.FromArgb(230, 244, 255);
            this.dgvIslemde.BorderStyle = BorderStyle.None;
            this.dgvIslemde.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIslemde.Location = new Point(10, 50);
            this.dgvIslemde.MultiSelect = false;
            this.dgvIslemde.Name = "dgvIslemde";
            this.dgvIslemde.ReadOnly = true;
            this.dgvIslemde.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvIslemde.Size = new Size(330, 180);
            this.dgvIslemde.TabIndex = 0;
            this.dgvIslemde.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            this.dgvIslemde.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseClick);

            // İşlemde grid kolonları
            this.dgvIslemde.Columns.Add("ID", "ID");
            this.dgvIslemde.Columns.Add("Area", "Alan");
            this.dgvIslemde.Columns.Add("Issue", "Sorun");
            this.dgvIslemde.Columns.Add("Date", "Tarih");
            this.dgvIslemde.Columns[0].Width = 40;
            this.dgvIslemde.Columns[1].Width = 80;
            this.dgvIslemde.Columns[2].Width = 120;
            this.dgvIslemde.Columns[3].Width = 80;

            this.lblIslemdeCount.AutoSize = true;
            this.lblIslemdeCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblIslemdeCount.ForeColor = Color.FromArgb(68, 68, 68);
            this.lblIslemdeCount.Location = new Point(10, 25);
            this.lblIslemdeCount.Name = "lblIslemdeCount";
            this.lblIslemdeCount.Size = new Size(80, 15);
            this.lblIslemdeCount.TabIndex = 1;
            this.lblIslemdeCount.Text = "İşlemde: 0";

            this.btnMarkCompleted.BackColor = Color.FromArgb(40, 167, 69);
            this.btnMarkCompleted.FlatAppearance.BorderSize = 0;
            this.btnMarkCompleted.FlatStyle = FlatStyle.Flat;
            this.btnMarkCompleted.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnMarkCompleted.ForeColor = Color.White;
            this.btnMarkCompleted.Location = new Point(10, 240);
            this.btnMarkCompleted.Name = "btnMarkCompleted";
            this.btnMarkCompleted.Size = new Size(150, 30);
            this.btnMarkCompleted.TabIndex = 2;
            this.btnMarkCompleted.Text = "✅ Tamamlandı";
            this.btnMarkCompleted.UseVisualStyleBackColor = false;
            this.btnMarkCompleted.Click += new System.EventHandler(this.btnMarkCompleted_Click);

            // 
            // ÇÖZÜLEN BÖLÜMÜ
            // 
            this.gbCozulen.Controls.Add(this.dgvCozulen);
            this.gbCozulen.Controls.Add(this.lblCozulenCount);
            this.gbCozulen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.gbCozulen.ForeColor = Color.FromArgb(40, 167, 69);
            this.gbCozulen.Location = new Point(750, 140);
            this.gbCozulen.Name = "gbCozulen";
            this.gbCozulen.Size = new Size(350, 280);
            this.gbCozulen.TabIndex = 7;
            this.gbCozulen.TabStop = false;
            this.gbCozulen.Text = "✅ ÇÖZÜLEN TICKET'LAR";

            this.dgvCozulen.AllowUserToAddRows = false;
            this.dgvCozulen.AllowUserToDeleteRows = false;
            this.dgvCozulen.BackgroundColor = Color.FromArgb(240, 255, 240);
            this.dgvCozulen.BorderStyle = BorderStyle.None;
            this.dgvCozulen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCozulen.Location = new Point(10, 50);
            this.dgvCozulen.MultiSelect = false;
            this.dgvCozulen.Name = "dgvCozulen";
            this.dgvCozulen.ReadOnly = true;
            this.dgvCozulen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCozulen.Size = new Size(330, 220);
            this.dgvCozulen.TabIndex = 0;
            this.dgvCozulen.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            this.dgvCozulen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseClick);

            // Çözülen grid kolonları
            this.dgvCozulen.Columns.Add("ID", "ID");
            this.dgvCozulen.Columns.Add("Area", "Alan");
            this.dgvCozulen.Columns.Add("Issue", "Sorun");
            this.dgvCozulen.Columns.Add("Date", "Tarih");
            this.dgvCozulen.Columns[0].Width = 40;
            this.dgvCozulen.Columns[1].Width = 80;
            this.dgvCozulen.Columns[2].Width = 120;
            this.dgvCozulen.Columns[3].Width = 80;

            this.lblCozulenCount.AutoSize = true;
            this.lblCozulenCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCozulenCount.ForeColor = Color.FromArgb(68, 68, 68);
            this.lblCozulenCount.Location = new Point(10, 25);
            this.lblCozulenCount.Name = "lblCozulenCount";
            this.lblCozulenCount.Size = new Size(80, 15);
            this.lblCozulenCount.TabIndex = 1;
            this.lblCozulenCount.Text = "Çözülen: 0";

            // 
            // AÇIKLAMA BÖLÜMÜ
            // 
            this.lblSelectedTicket.AutoSize = true;
            this.lblSelectedTicket.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblSelectedTicket.ForeColor = Color.FromArgb(68, 68, 68);
            this.lblSelectedTicket.Location = new Point(20, 440);
            this.lblSelectedTicket.Name = "lblSelectedTicket";
            this.lblSelectedTicket.Size = new Size(150, 19);
            this.lblSelectedTicket.TabIndex = 8;
            this.lblSelectedTicket.Text = "Seçilen Ticket: Yok";

            this.txtDescription.BackColor = Color.FromArgb(248, 249, 250);
            this.txtDescription.BorderStyle = BorderStyle.FixedSingle;
            this.txtDescription.Font = new Font("Segoe UI", 9F);
            this.txtDescription.Location = new Point(20, 470);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(1080, 80);
            this.txtDescription.TabIndex = 9;
            this.txtDescription.Text = "Bir ticket seçin...";

            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.ClientSize = new Size(1120, 570);
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
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AdminForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "🎫 Admin Paneli - Ticket Yönetimi";
            this.Load += new System.EventHandler(this.AdminForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvBekleyen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIslemde)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCozulen)).EndInit();
            this.gbBekleyen.ResumeLayout(false);
            this.gbBekleyen.PerformLayout();
            this.gbIslemde.ResumeLayout(false);
            this.gbIslemde.PerformLayout();
            this.gbCozulen.ResumeLayout(false);
            this.gbCozulen.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}