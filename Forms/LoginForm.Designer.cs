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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblFormSubtitle = new System.Windows.Forms.Label();
            this.panelMainMenu = new System.Windows.Forms.Panel();
            this.lblChoosePanel = new System.Windows.Forms.Label();
            this.btnITPanel = new System.Windows.Forms.Button();
            this.btnUserPanel = new System.Windows.Forms.Button();
            this.panelAdminLogin = new System.Windows.Forms.Panel();
            this.groupBoxAdminLogin = new System.Windows.Forms.GroupBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnAdminLogin = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelMainMenu.SuspendLayout();
            this.panelAdminLogin.SuspendLayout();
            this.groupBoxAdminLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelHeader.Controls.Add(this.pictureBoxLogo);
            this.panelHeader.Controls.Add(this.lblFormTitle);
            this.panelHeader.Controls.Add(this.lblFormSubtitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(429, 104);
            this.panelHeader.TabIndex = 0;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Location = new System.Drawing.Point(171, 9);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(86, 35);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(0, 39);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(429, 35);
            this.lblFormTitle.TabIndex = 1;
            this.lblFormTitle.Text = "Hoş Geldiniz";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFormSubtitle
            // 
            this.lblFormSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFormSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblFormSubtitle.Location = new System.Drawing.Point(0, 74);
            this.lblFormSubtitle.Name = "lblFormSubtitle";
            this.lblFormSubtitle.Size = new System.Drawing.Size(429, 22);
            this.lblFormSubtitle.TabIndex = 2;
            this.lblFormSubtitle.Text = "Lütfen giriş türünü seçin";
            this.lblFormSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFormSubtitle.Click += new System.EventHandler(this.lblFormSubtitle_Click);
            // 
            // panelMainMenu
            // 
            this.panelMainMenu.BackColor = System.Drawing.Color.White;
            this.panelMainMenu.Controls.Add(this.lblChoosePanel);
            this.panelMainMenu.Controls.Add(this.btnITPanel);
            this.panelMainMenu.Controls.Add(this.btnUserPanel);
            this.panelMainMenu.Location = new System.Drawing.Point(0, 104);
            this.panelMainMenu.Name = "panelMainMenu";
            this.panelMainMenu.Size = new System.Drawing.Size(429, 286);
            this.panelMainMenu.TabIndex = 1;
            // 
            // lblChoosePanel
            // 
            this.lblChoosePanel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblChoosePanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblChoosePanel.Location = new System.Drawing.Point(0, 26);
            this.lblChoosePanel.Name = "lblChoosePanel";
            this.lblChoosePanel.Size = new System.Drawing.Size(429, 26);
            this.lblChoosePanel.TabIndex = 0;
            this.lblChoosePanel.Text = "Hangi panele giriş yapmak istiyorsunuz?";
            this.lblChoosePanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblChoosePanel.Click += new System.EventHandler(this.lblChoosePanel_Click);
            // 
            // btnITPanel
            // 
            this.btnITPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnITPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnITPanel.FlatAppearance.BorderSize = 0;
            this.btnITPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnITPanel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnITPanel.ForeColor = System.Drawing.Color.White;
            this.btnITPanel.Location = new System.Drawing.Point(107, 78);
            this.btnITPanel.Name = "btnITPanel";
            this.btnITPanel.Size = new System.Drawing.Size(214, 69);
            this.btnITPanel.TabIndex = 0;
            this.btnITPanel.Text = "🔧 IT YÖNETİM PANELİ\n\nAdmin Girişi";
            this.btnITPanel.UseVisualStyleBackColor = false;
            this.btnITPanel.Click += new System.EventHandler(this.btnITPanel_Click);
            // 
            // btnUserPanel
            // 
            this.btnUserPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnUserPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserPanel.FlatAppearance.BorderSize = 0;
            this.btnUserPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserPanel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnUserPanel.ForeColor = System.Drawing.Color.White;
            this.btnUserPanel.Location = new System.Drawing.Point(107, 165);
            this.btnUserPanel.Name = "btnUserPanel";
            this.btnUserPanel.Size = new System.Drawing.Size(214, 69);
            this.btnUserPanel.TabIndex = 1;
            this.btnUserPanel.Text = "🎫 KULLANICI PANELİ\n\nDestek Talebi Oluştur";
            this.btnUserPanel.UseVisualStyleBackColor = false;
            this.btnUserPanel.Click += new System.EventHandler(this.btnUserPanel_Click);
            // 
            // panelAdminLogin
            // 
            this.panelAdminLogin.BackColor = System.Drawing.Color.White;
            this.panelAdminLogin.Controls.Add(this.groupBoxAdminLogin);
            this.panelAdminLogin.Location = new System.Drawing.Point(0, 104);
            this.panelAdminLogin.Name = "panelAdminLogin";
            this.panelAdminLogin.Size = new System.Drawing.Size(429, 286);
            this.panelAdminLogin.TabIndex = 2;
            this.panelAdminLogin.Visible = false;
            // 
            // groupBoxAdminLogin
            // 
            this.groupBoxAdminLogin.Controls.Add(this.lblUsername);
            this.groupBoxAdminLogin.Controls.Add(this.txtUsername);
            this.groupBoxAdminLogin.Controls.Add(this.lblPassword);
            this.groupBoxAdminLogin.Controls.Add(this.txtPassword);
            this.groupBoxAdminLogin.Controls.Add(this.btnAdminLogin);
            this.groupBoxAdminLogin.Controls.Add(this.btnBack);
            this.groupBoxAdminLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.groupBoxAdminLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxAdminLogin.Location = new System.Drawing.Point(64, 26);
            this.groupBoxAdminLogin.Name = "groupBoxAdminLogin";
            this.groupBoxAdminLogin.Size = new System.Drawing.Size(300, 217);
            this.groupBoxAdminLogin.TabIndex = 0;
            this.groupBoxAdminLogin.TabStop = false;
            this.groupBoxAdminLogin.Text = "🔐 Güvenli Admin Girişi";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUsername.Location = new System.Drawing.Point(26, 43);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(85, 19);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Kullanıcı Adı:";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUsername.Location = new System.Drawing.Point(26, 65);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(249, 27);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Text = "admin";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPassword.Location = new System.Drawing.Point(26, 100);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(38, 19);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Şifre:";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.Location = new System.Drawing.Point(26, 121);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(249, 27);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // btnAdminLogin
            // 
            this.btnAdminLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnAdminLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdminLogin.FlatAppearance.BorderSize = 0;
            this.btnAdminLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminLogin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAdminLogin.ForeColor = System.Drawing.Color.White;
            this.btnAdminLogin.Location = new System.Drawing.Point(26, 165);
            this.btnAdminLogin.Name = "btnAdminLogin";
            this.btnAdminLogin.Size = new System.Drawing.Size(116, 35);
            this.btnAdminLogin.TabIndex = 2;
            this.btnAdminLogin.Text = "Giriş Yap";
            this.btnAdminLogin.UseVisualStyleBackColor = false;
            this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(159, 165);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(116, 35);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Geri Dön";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(429, 390);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelMainMenu);
            this.Controls.Add(this.panelAdminLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IT Destek Sistemi";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelMainMenu.ResumeLayout(false);
            this.panelAdminLogin.ResumeLayout(false);
            this.groupBoxAdminLogin.ResumeLayout(false);
            this.groupBoxAdminLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}