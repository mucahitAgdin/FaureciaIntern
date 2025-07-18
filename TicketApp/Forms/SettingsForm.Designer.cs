using System.Drawing;
using System.Windows.Forms;

namespace TicketApp.Forms
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form ayarları
            this.Text = "Sistem Ayarları - Alan ve Sorun Yönetimi";
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // TabControl oluştur
            tabControl = new TabControl();
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(760, 500);
            tabControl.Font = new Font("Segoe UI", 10F);

            // Alan yönetimi tab'ı
            CreateAreaManagementTab();

            // Sorun yönetimi tab'ı
            CreateIssueManagementTab();

            // Genel butonlar
            CreateGeneralButtons();

            this.Controls.Add(tabControl);
            this.ResumeLayout();
        }
        #endregion
    }
}