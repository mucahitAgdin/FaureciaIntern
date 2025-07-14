namespace TicketApp.Forms
{
    partial class LoginForm
    {
        // Formdaki bileşenleri (textbox, button vs.) tutan konteyner
        private System.ComponentModel.IContainer components = null;

        // Giriş ekranı bileşenleri
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;

        // Bellek temizliği için kullanılan override metod
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // Bileşenlerin form üzerine yerleştirildiği kısım
        private void InitializeComponent()
        {
            // Bileşenlerin örnekleri oluşturuluyor
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // Kullanıcı Adı Etiketi
            this.lblUsername.Location = new System.Drawing.Point(30, 30);
            this.lblUsername.Size = new System.Drawing.Size(100, 23);
            this.lblUsername.Text = "Kullanıcı Adı:";

            // Kullanıcı Adı TextBox
            this.txtUsername.Location = new System.Drawing.Point(140, 30);
            this.txtUsername.Size = new System.Drawing.Size(200, 20);

            // Şifre Etiketi
            this.lblPassword.Location = new System.Drawing.Point(30, 70);
            this.lblPassword.Size = new System.Drawing.Size(100, 23);
            this.lblPassword.Text = "Şifre:";

            // Şifre TextBox
            this.txtPassword.Location = new System.Drawing.Point(140, 70);
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.PasswordChar = '*'; // Şifre gizlenir

            // Giriş Butonu
            this.btnLogin.Location = new System.Drawing.Point(140, 110);
            this.btnLogin.Size = new System.Drawing.Size(200, 30);
            this.btnLogin.Text = "Giriş Yap";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // LoginForm genel özellikleri
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Name = "LoginForm";
            this.Text = "Admin Giriş Paneli";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout(); // Form öğelerini ekrana yerleştirir
        }
    }
}
