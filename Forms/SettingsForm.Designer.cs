// TicketApp/Forms/SettingsForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;

namespace TicketApp.Forms
{
    partial class SettingsForm
    {
        /// <summary>Designer container</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Dispose</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Designer: Temel form ayarlarını kurar; TabControl ekler ve
        /// ayrıntılı yerleşimi SettingsForm.cs içindeki Create* metodlarına bırakır.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form
            this.Text = "Sistem Ayarları - Alan / Alt Alan / Hat ve Sorun Yönetimi";
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // TabControl
            tabControl = new TabControl
            {
                Location = new Point(10, 10),
                Size = new Size(760, 500),
                Font = new Font("Segoe UI", 10F)
            };

            // Sekmeleri ve alt butonları oluştur (SettingsForm.cs)
            CreateAreaManagementTab();
            CreateIssueManagementTab();
            CreateGeneralButtons();

            this.Controls.Add(tabControl);
            this.ResumeLayout(false);
        }
        #endregion
    }
}
