using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicketApp.Helpers;

namespace TicketApp
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.InitializeUI();

        }
        private void InitializeUI()
        {
            // Form ayarları
            this.Text = "Ticket Submission Panel";
            this.Width = 600;
            this.Height = 450;

            // Label - Area
            Label labelArea = new Label
            {
                Text = "Area:",
                Location = new System.Drawing.Point(30, 30),
                Width = 100
            };
            this.Controls.Add(labelArea);

            comboBoxArea = new ComboBox
            {
                Name = "comboBoxArea",
                Location = new System.Drawing.Point(150, 25),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(comboBoxArea);

            // Label - Issue
            Label labelIssue = new Label
            {
                Text = "Issue:",
                Location = new System.Drawing.Point(30, 70),
                Width = 100
            };
            this.Controls.Add(labelIssue);

            comboBoxIssue = new ComboBox
            {
                Name = "comboBoxIssue",
                Location = new System.Drawing.Point(150, 65),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(comboBoxIssue);

            // Label - Description
            Label labelDesc = new Label
            {
                Text = "Description:",
                Location = new System.Drawing.Point(30, 110),
                Width = 100
            };
            this.Controls.Add(labelDesc);

            textBoxDescription = new TextBox
            {
                Name = "textBoxDescription",
                Location = new System.Drawing.Point(150, 105),
                Width = 300,
                Height = 60,
                Multiline = true
            };
            this.Controls.Add(textBoxDescription);

            // Submit Button
            btnSubmit = new Button
            {
                Name = "btnSubmit",
                Text = "Submit Ticket",
                Location = new System.Drawing.Point(150, 180),
                Width = 150
            };
            btnSubmit.Click += BtnSubmit_Click;
            this.Controls.Add(btnSubmit);

            // ListBox - Tickets
            listBoxTickets = new ListBox
            {
                Name = "listBoxTickets",
                Location = new System.Drawing.Point(30, 230),
                Width = 520,
                Height = 150
            };
            this.Controls.Add(listBoxTickets);
        }
        #endregion
    }
}

