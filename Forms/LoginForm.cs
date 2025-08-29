// Forms/LoginForm.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using TicketApp.Helpers;

namespace TicketApp.Forms
{
    public partial class LoginForm : Form
    {
        private int failedAttempts = 0;
        private const int MAX_ATTEMPTS = 3;
        private bool isAdminMode = false; // Hangi modda olduğumuzu takip eder

        public LoginForm()
        {
            InitializeComponent();
            SetupForm();
        }

        /// <summary>
        /// Form ayarlarını yapar
        /// </summary>
        private void SetupForm()
        {
            // Form başlığı ve ikonu
            this.Text = "IT Destek Sistemi - Giriş";
            this.Icon = SystemIcons.Application;

            // ESC tuşu ile formu kapat veya geri dön
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (isAdminMode)
                    {
                        ShowMainMenu(); // Admin modundaysa ana menüye dön
                    }
                    else
                    {
                        this.Close(); // Ana menüdeyse uygulamayı kapat
                    }
                }
            };
        }

        /// <summary>
        /// Form yüklendiğinde ana menüyü göster
        /// </summary>
        private void LoginForm_Load(object sender, EventArgs e)
        {
            ShowMainMenu();
        }

        /// <summary>
        /// Ana menüyü gösterir (Panel seçimi)
        /// </summary>
        private void ShowMainMenu()
        {
            isAdminMode = false;

            // Admin giriş panelini gizle
            panelAdminLogin.Visible = false;

            // Ana menü panelini göster
            panelMainMenu.Visible = true;

            // Form başlığını güncelle
            lblFormTitle.Text = "Hoş Geldiniz";
            lblFormSubtitle.Text = "Lütfen giriş türünü seçin";
        }

        /// <summary>
        /// Admin giriş panelini gösterir
        /// </summary>
        private void ShowAdminPanel()
        {
            isAdminMode = true;

            // Ana menü panelini gizle
            panelMainMenu.Visible = false;

            // Admin giriş panelini göster
            panelAdminLogin.Visible = true;

            // Form başlığını güncelle
            lblFormTitle.Text = "IT Yönetici Girişi";
            lblFormSubtitle.Text = "Güvenli admin paneli erişimi";

            // Kullanıcı adı kutusuna odaklan
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        /// <summary>
        /// IT Paneli butonuna tıklandığında
        /// </summary>
        private void btnITPanel_Click(object sender, EventArgs e)
        {
            ShowAdminPanel();
        }

        /// <summary>
        /// Kullanıcı Paneli butonuna tıklandığında
        /// </summary>
        private void btnUserPanel_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Log("Kullanıcı paneline giriş yapıldı");

                // Ana formu aç
                var mainForm = new MainForm();
                mainForm.FormClosed += (s, args) =>
                {
                    // Ana form kapandığında login formunu göster
                    this.Show();
                    ShowMainMenu(); // Ana menüye dön
                };

                mainForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                ShowError("Kullanıcı paneli açılırken hata oluştu.");
            }
        }

        /// <summary>
        /// Admin giriş butonuna tıklandığında
        /// </summary>
        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Giriş denemesi kontrolü
                if (failedAttempts >= MAX_ATTEMPTS)
                {
                    MessageBox.Show(
                        "Çok fazla başarısız giriş denemesi!\n" +
                        "Güvenlik nedeniyle giriş kilitlendi.\n" +
                        "Lütfen sistem yöneticisi ile iletişime geçin.",
                        "Hesap Kilitli",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                string enteredUsername = txtUsername.Text.Trim();
                string enteredPassword = txtPassword.Text;

                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(enteredUsername))
                {
                    ShowWarning("Kullanıcı adı boş olamaz!");
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(enteredPassword))
                {
                    ShowWarning("Şifre boş olamaz!");
                    txtPassword.Focus();
                    return;
                }

                // Giriş bilgilerini kontrol et
                if (ValidateAdminCredentials(enteredUsername, enteredPassword))
                {
                    // Başarılı giriş
                    failedAttempts = 0;
                    Logger.Log($"Admin girişi başarılı: {enteredUsername}");

                    // Admin formunu aç
                    OpenAdminForm(enteredUsername);
                }
                else
                {
                    // Başarısız giriş
                    failedAttempts++;
                    int remainingAttempts = MAX_ATTEMPTS - failedAttempts;

                    Logger.Log($"Başarısız admin girişi: {enteredUsername} - Deneme: {failedAttempts}/{MAX_ATTEMPTS}");

                    ShowError(
                        $"Kullanıcı adı veya şifre hatalı!\n" +
                        $"Kalan deneme hakkı: {remainingAttempts}"
                    );

                    // Şifre kutusunu temizle ve odaklan
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                ShowError("Giriş işlemi sırasında beklenmeyen bir hata oluştu.");
            }
        }

        /// <summary>
        /// Geri dön butonuna tıklandığında
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            ShowMainMenu();
            failedAttempts = 0; // Geri dönünce sayacı sıfırla
        }

        /// <summary>
        /// Admin bilgilerini doğrular
        /// </summary>
        private bool ValidateAdminCredentials(string username, string password)
        {
            try
            {
                string expectedUsername = AppConfigReader.Get("AdminUsername");
                string expectedPassword = AppConfigReader.Get("AdminPassword");

                return username == expectedUsername && password == expectedPassword;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        /// Admin formunu açar
        /// </summary>
        private void OpenAdminForm(string username)
        {
            var adminForm = new AdminForm(username);
            adminForm.FormClosed += (s, args) =>
            {
                // Admin formu kapandığında
                this.Show();
                ShowMainMenu(); // Ana menüye dön
            };

            adminForm.Show();
            this.Hide();
        }

        /// <summary>
        /// Şifre kutusunda Enter'a basıldığında
        /// </summary>
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAdminLogin.PerformClick();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Uyarı mesajı gösterir
        /// </summary>
        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Hata mesajı gösterir
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void lblChoosePanel_Click(object sender, EventArgs e)
        {

        }

        private void lblFormSubtitle_Click(object sender, EventArgs e)
        {

        }
    }
}