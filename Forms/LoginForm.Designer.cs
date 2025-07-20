// Forms/LoginForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;

namespace TicketApp.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Ana kontroller
        private Panel panelHeader;
        private Panel panelMainMenu;
        private Panel panelAdminLogin;

        // Başlık kontrolleri
        private PictureBox pictureBoxLogo;
        private Label lblFormTitle;
        private Label lblFormSubtitle;

        // Ana menü kontrolleri
        private Button btnITPanel;
        private Button btnUserPanel;
        private Label lblChoosePanel;

        // Admin giriş kontrolleri
        private GroupBox groupBoxAdminLogin;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnAdminLogin;
        private Button btnBack;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            // LoginForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IT Destek Sistemi";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            // Başlık paneli
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Size = new System.Drawing.Size(500, 120);

            // Logo (isteğe bağlı)
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Location = new System.Drawing.Point(200, 10);
            this.pictureBoxLogo.Size = new System.Drawing.Size(100, 40);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;

            // Form başlığı
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblFormTitle.AutoSize = false;
            this.lblFormTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(0, 45);
            this.lblFormTitle.Size = new System.Drawing.Size(500, 40);
            this.lblFormTitle.Text = "Hoş Geldiniz";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Alt başlık
            this.lblFormSubtitle = new System.Windows.Forms.Label();
            this.lblFormSubtitle.AutoSize = false;
            this.lblFormSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFormSubtitle.ForeColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.lblFormSubtitle.Location = new System.Drawing.Point(0, 85);
            this.lblFormSubtitle.Size = new System.Drawing.Size(500, 25);
            this.lblFormSubtitle.Text = "Lütfen giriş türünü seçin";
            this.lblFormSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Ana menü paneli
            this.panelMainMenu = new System.Windows.Forms.Panel();
            this.panelMainMenu.Location = new System.Drawing.Point(0, 120);
            this.panelMainMenu.Size = new System.Drawing.Size(500, 330);
            this.panelMainMenu.BackColor = System.Drawing.Color.White;

            // Panel seçim etiketi
            this.lblChoosePanel = new System.Windows.Forms.Label();
            this.lblChoosePanel.AutoSize = false;
            this.lblChoosePanel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblChoosePanel.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.lblChoosePanel.Location = new System.Drawing.Point(0, 30);
            this.lblChoosePanel.Size = new System.Drawing.Size(500, 30);
            this.lblChoosePanel.Text = "Hangi panele giriş yapmak istiyorsunuz?";
            this.lblChoosePanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // IT Paneli butonu
            this.btnITPanel = new System.Windows.Forms.Button();
            this.btnITPanel.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnITPanel.FlatAppearance.BorderSize = 0;
            this.btnITPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnITPanel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnITPanel.ForeColor = System.Drawing.Color.White;
            this.btnITPanel.Location = new System.Drawing.Point(125, 90);
            this.btnITPanel.Size = new System.Drawing.Size(250, 80);
            this.btnITPanel.TabIndex = 0;
            this.btnITPanel.Text = "🔧 IT YÖNETİM PANELİ\n\nAdmin Girişi";
            this.btnITPanel.UseVisualStyleBackColor = false;
            this.btnITPanel.Click += new System.EventHandler(this.btnITPanel_Click);
            this.btnITPanel.Cursor = System.Windows.Forms.Cursors.Hand;

            // IT Paneli hover efekti
            this.btnITPanel.MouseEnter += (s, e) => {
                this.btnITPanel.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            };
            this.btnITPanel.MouseLeave += (s, e) => {
                this.btnITPanel.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            };

            // Kullanıcı Paneli butonu
            this.btnUserPanel = new System.Windows.Forms.Button();
            this.btnUserPanel.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnUserPanel.FlatAppearance.BorderSize = 0;
            this.btnUserPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserPanel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnUserPanel.ForeColor = System.Drawing.Color.White;
            this.btnUserPanel.Location = new System.Drawing.Point(125, 190);
            this.btnUserPanel.Size = new System.Drawing.Size(250, 80);
            this.btnUserPanel.TabIndex = 1;
            this.btnUserPanel.Text = "🎫 KULLANICI PANELİ\n\nDestek Talebi Oluştur";
            this.btnUserPanel.UseVisualStyleBackColor = false;
            this.btnUserPanel.Click += new System.EventHandler(this.btnUserPanel_Click);
            this.btnUserPanel.Cursor = System.Windows.Forms.Cursors.Hand;

            // Kullanıcı Paneli hover efekti
            this.btnUserPanel.MouseEnter += (s, e) => {
                this.btnUserPanel.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            };
            this.btnUserPanel.MouseLeave += (s, e) => {
                this.btnUserPanel.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            };

            // Admin giriş paneli
            this.panelAdminLogin = new System.Windows.Forms.Panel();
            this.panelAdminLogin.Location = new System.Drawing.Point(0, 120);
            this.panelAdminLogin.Size = new System.Drawing.Size(500, 330);
            this.panelAdminLogin.BackColor = System.Drawing.Color.White;
            this.panelAdminLogin.Visible = false;

            // Admin giriş grup kutusu
            this.groupBoxAdminLogin = new System.Windows.Forms.GroupBox();
            this.groupBoxAdminLogin.Text = "🔐 Güvenli Admin Girişi";
            this.groupBoxAdminLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.groupBoxAdminLogin.Location = new System.Drawing.Point(75, 30);
            this.groupBoxAdminLogin.Size = new System.Drawing.Size(350, 250);
            this.groupBoxAdminLogin.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);

            // Kullanıcı adı etiketi
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUsername.Location = new System.Drawing.Point(30, 50);
            this.lblUsername.Size = new System.Drawing.Size(77, 19);
            this.lblUsername.Text = "Kullanıcı Adı:";

            // Kullanıcı adı kutusu
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUsername.Location = new System.Drawing.Point(30, 75);
            this.txtUsername.Size = new System.Drawing.Size(290, 27);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Text = "admin";

            // Şifre etiketi
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPassword.Location = new System.Drawing.Point(30, 115);
            this.lblPassword.Size = new System.Drawing.Size(37, 19);
            this.lblPassword.Text = "Şifre:";

            // Şifre kutusu
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.Location = new System.Drawing.Point(30, 140);
            this.txtPassword.Size = new System.Drawing.Size(290, 27);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);

            // Giriş yap butonu
            this.btnAdminLogin = new System.Windows.Forms.Button();
            this.btnAdminLogin.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnAdminLogin.FlatAppearance.BorderSize = 0;
            this.btnAdminLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminLogin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAdminLogin.ForeColor = System.Drawing.Color.White;
            this.btnAdminLogin.Location = new System.Drawing.Point(30, 190);
            this.btnAdminLogin.Size = new System.Drawing.Size(135, 40);
            this.btnAdminLogin.TabIndex = 2;
            this.btnAdminLogin.Text = "Giriş Yap";
            this.btnAdminLogin.UseVisualStyleBackColor = false;
            this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
            this.btnAdminLogin.Cursor = System.Windows.Forms.Cursors.Hand;

            // Geri dön butonu
            this.btnBack = new System.Windows.Forms.Button();
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(185, 190);
            this.btnBack.Size = new System.Drawing.Size(135, 40);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Geri Dön";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;

            // Kontrolleri ekle
            this.panelHeader.Controls.Add(this.pictureBoxLogo);
            this.panelHeader.Controls.Add(this.lblFormTitle);
            this.panelHeader.Controls.Add(this.lblFormSubtitle);

            this.panelMainMenu.Controls.Add(this.lblChoosePanel);
            this.panelMainMenu.Controls.Add(this.btnITPanel);
            this.panelMainMenu.Controls.Add(this.btnUserPanel);

            this.groupBoxAdminLogin.Controls.Add(this.lblUsername);
            this.groupBoxAdminLogin.Controls.Add(this.txtUsername);
            this.groupBoxAdminLogin.Controls.Add(this.lblPassword);
            this.groupBoxAdminLogin.Controls.Add(this.txtPassword);
            this.groupBoxAdminLogin.Controls.Add(this.btnAdminLogin);
            this.groupBoxAdminLogin.Controls.Add(this.btnBack);

            this.panelAdminLogin.Controls.Add(this.groupBoxAdminLogin);

            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelMainMenu);
            this.Controls.Add(this.panelAdminLogin);

            this.ResumeLayout(false);
        }

        #endregion
    }
}