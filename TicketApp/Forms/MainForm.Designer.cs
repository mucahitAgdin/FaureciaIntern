// TicketApp/Forms/MainForm.Designer.cs
namespace TicketApp.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Form üzerindeki bileşenleri tutan değişken. Dispose işlemi için gerekir.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Görsel bileşenler (UI)
        private System.Windows.Forms.ComboBox comboBoxArea;
        private System.Windows.Forms.ComboBox comboBoxIssue;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ListBox listBoxTickets;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelIssue;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelHistory;

        /// <summary>
        /// Bellek temizliği için kullanılan metod (otomatik çağrılır)
        /// </summary>
        /// <param name="disposing">Bileşenler manuel olarak mı yoksa otomatik mi dispose ediliyor?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Form üzerindeki tüm bileşenleri başlatır ve yerleştirir
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.comboBoxIssue = new System.Windows.Forms.ComboBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.listBoxTickets = new System.Windows.Forms.ListBox();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelIssue = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelHistory = new System.Windows.Forms.Label();

            this.SuspendLayout(); // Performans için geçici olarak çizimi durdurur

            //
            // labelArea
            //
            this.labelArea.AutoSize = true;
            this.labelArea.Location = new System.Drawing.Point(30, 30);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(80, 20);
            this.labelArea.Text = "Alan (UAP/FES):";

            //
            // comboBoxArea
            //
            this.comboBoxArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArea.FormattingEnabled = true;
            this.comboBoxArea.Location = new System.Drawing.Point(150, 30);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(200, 28);

            //
            // labelIssue
            //
            this.labelIssue.AutoSize = true;
            this.labelIssue.Location = new System.Drawing.Point(30, 70);
            this.labelIssue.Name = "labelIssue";
            this.labelIssue.Size = new System.Drawing.Size(100, 20);
            this.labelIssue.Text = "Sorun Tipi:";

            //
            // comboBoxIssue
            //
            this.comboBoxIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIssue.FormattingEnabled = true;
            this.comboBoxIssue.Location = new System.Drawing.Point(150, 70);
            this.comboBoxIssue.Name = "comboBoxIssue";
            this.comboBoxIssue.Size = new System.Drawing.Size(200, 28);

            //
            // labelDescription
            //
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(30, 110);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(85, 20);
            this.labelDescription.Text = "Açıklama:";

            //
            // textBoxDescription
            //
            this.textBoxDescription.Location = new System.Drawing.Point(150, 110);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(300, 100);
            this.textBoxDescription.MaxLength = 300; // Açıklama 300 karakter ile sınırlı

            //
            // btnSubmit
            //
            this.btnSubmit.Location = new System.Drawing.Point(150, 220);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(120, 40);
            this.btnSubmit.Text = "Talebi Gönder";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click); // Tıklama eventi bağlanıyor

            //
            // labelHistory
            //
            this.labelHistory.AutoSize = true;
            this.labelHistory.Location = new System.Drawing.Point(30, 280);
            this.labelHistory.Name = "labelHistory";
            this.labelHistory.Size = new System.Drawing.Size(136, 20);
            this.labelHistory.Text = "Gönderilen Talepler:";

            //
            // listBoxTickets
            //
            this.listBoxTickets.FormattingEnabled = true;
            this.listBoxTickets.ItemHeight = 20;
            this.listBoxTickets.Location = new System.Drawing.Point(30, 310);
            this.listBoxTickets.Name = "listBoxTickets";
            this.listBoxTickets.Size = new System.Drawing.Size(420, 140);

            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 480);
            this.Controls.Add(this.labelArea);
            this.Controls.Add(this.comboBoxArea);
            this.Controls.Add(this.labelIssue);
            this.Controls.Add(this.comboBoxIssue);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelHistory);
            this.Controls.Add(this.listBoxTickets);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "IT Destek Talep Sistemi";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout(); // Tüm yerleşim işlemleri bittikten sonra yeniden çizimi başlatır
        }
    }
}
