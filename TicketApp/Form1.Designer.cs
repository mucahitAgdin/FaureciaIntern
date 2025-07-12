// Form1.Designer.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicketApp.Helpers;

namespace TicketApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // UI controls - declared here to match drag/drop style structure
        private ComboBox comboBoxArea;
        private ComboBox comboBoxIssue;
        private TextBox textBoxDescription;
        private Button btnSubmit;
        private ListBox listBoxTickets;
        private Label labelArea;
        private Label labelIssue;
        private Label labelDescription;
        private Button btnDelete;

        /// <summary>
        /// Disposes resources in the form
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method initializes UI components of the form. If you use drag/drop tools,
        /// Visual Studio auto-generates code here. For consistency, we also use it for manual UI creation.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.comboBoxIssue = new System.Windows.Forms.ComboBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.listBoxTickets = new System.Windows.Forms.ListBox();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelIssue = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxArea
            // 
            this.comboBoxArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArea.Location = new System.Drawing.Point(150, 25);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(200, 21);
            this.comboBoxArea.TabIndex = 0;
            this.comboBoxArea.SelectedIndexChanged += new System.EventHandler(this.comboBoxArea_SelectedIndexChanged_1);
            // 
            // comboBoxIssue
            // 
            this.comboBoxIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIssue.Location = new System.Drawing.Point(150, 65);
            this.comboBoxIssue.Name = "comboBoxIssue";
            this.comboBoxIssue.Size = new System.Drawing.Size(300, 21);
            this.comboBoxIssue.TabIndex = 1;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(150, 105);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(300, 60);
            this.textBoxDescription.TabIndex = 2;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(150, 180);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(150, 30);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Talep Gönder";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // listBoxTickets
            // 
            this.listBoxTickets.FormattingEnabled = true;
            this.listBoxTickets.Location = new System.Drawing.Point(30, 230);
            this.listBoxTickets.Name = "listBoxTickets";
            this.listBoxTickets.Size = new System.Drawing.Size(520, 160);
            this.listBoxTickets.TabIndex = 4;
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Location = new System.Drawing.Point(50, 28);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(53, 13);
            this.labelArea.TabIndex = 5;
            this.labelArea.Text = "Alan Seç:";
            // 
            // labelIssue
            // 
            this.labelIssue.AutoSize = true;
            this.labelIssue.Location = new System.Drawing.Point(50, 68);
            this.labelIssue.Name = "labelIssue";
            this.labelIssue.Size = new System.Drawing.Size(60, 13);
            this.labelIssue.TabIndex = 6;
            this.labelIssue.Text = "Sorun Seç:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(50, 108);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(77, 13);
            this.labelDescription.TabIndex = 7;
            this.labelDescription.Text = "Açıklama Ekle:";

            this.btnDelete = new System.Windows.Forms.Button();
            this.Controls.Add(this.btnDelete);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(310, 180);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 30);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Seçili Talebi Sil";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.labelArea);
            this.Controls.Add(this.labelIssue);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.comboBoxArea);
            this.Controls.Add(this.comboBoxIssue);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.listBoxTickets);
            this.Name = "Form1";
            this.Text = "Destek Talep Paneli";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}