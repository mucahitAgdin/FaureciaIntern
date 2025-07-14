using System;
using System.Windows.Forms;
using TicketApp.Helpers;

namespace TicketApp.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Kullanıcı giriş bilgileri TextBox'lardan alınır
            string enteredUsername = txtUsername.Text.Trim();
            string enteredPassword = txtPassword.Text;

            // App.config dosyasından beklenen değerler alınır
            string expectedUsername = AppConfigReader.Get("AdminUsername");
            string expectedPassword = AppConfigReader.Get("AdminPassword");

            // Girilen bilgiler doğruysa admin paneli açılır
            if (enteredUsername == expectedUsername && enteredPassword == expectedPassword)
            {
                AdminForm adminForm = new AdminForm();
                adminForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Geçersiz kullanıcı adı veya şifre.", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }
    }
}
