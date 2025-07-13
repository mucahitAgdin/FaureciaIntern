//Form1.Designer.cs

using System;
using System.Windows.Forms;

namespace TicketApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboBoxArea;
        private ComboBox comboBoxIssue;
        private TextBox textBoxDescription;
        private Button btnSubmit;
        private ListBox listBoxTickets;
        private Button btnDelete;
        private Label labelArea;
        private Label labelIssue;
        private Label labelDescription;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxArea = new ComboBox();
            this.comboBoxIssue = new ComboBox();
            this.textBoxDescription = new TextBox();
            this.btnSubmit = new Button();
            this.btnDelete = new Button();
            this.listBoxTickets = new ListBox();
            this.labelArea = new Label();
            this.labelIssue = new Label();
            this.labelDescription = new Label();
            this.SuspendLayout();

            // labelArea
            this.labelArea.AutoSize = true;
            this.labelArea.Location = new System.Drawing.Point(50, 28);
            this.labelArea.Text = "Alan Seç:";

            // labelIssue
            this.labelIssue.AutoSize = true;
            this.labelIssue.Location = new System.Drawing.Point(50, 68);
            this.labelIssue.Text = "Sorun Seç:";

            // labelDescription
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(50, 108);
            this.labelDescription.Text = "Açıklama Ekle:";

            // comboBoxArea
            this.comboBoxArea.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxArea.Location = new System.Drawing.Point(150, 25);
            this.comboBoxArea.Size = new System.Drawing.Size(200, 21);
            this.comboBoxArea.SelectedIndexChanged += new EventHandler(this.comboBoxArea_SelectedIndexChanged_1);

            // comboBoxIssue
            this.comboBoxIssue.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxIssue.Location = new System.Drawing.Point(150, 65);
            this.comboBoxIssue.Size = new System.Drawing.Size(300, 21);

            // textBoxDescription
            this.textBoxDescription.Location = new System.Drawing.Point(150, 105);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Size = new System.Drawing.Size(300, 60);

            // btnSubmit
            this.btnSubmit.Location = new System.Drawing.Point(150, 180);
            this.btnSubmit.Size = new System.Drawing.Size(150, 30);
            this.btnSubmit.Text = "Talep Gönder";
            this.btnSubmit.Click += new EventHandler(this.BtnSubmit_Click);

            // listBoxTickets
            this.listBoxTickets.Location = new System.Drawing.Point(30, 230);
            this.listBoxTickets.Size = new System.Drawing.Size(520, 160);

            // Form1
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.comboBoxArea);
            this.Controls.Add(this.comboBoxIssue);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.listBoxTickets);
            this.Controls.Add(this.labelArea);
            this.Controls.Add(this.labelIssue);
            this.Controls.Add(this.labelDescription);
            this.Name = "Form1";
            this.Text = "Destek Talep Paneli";
            this.Load += new EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
