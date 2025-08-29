// TicketApp/Forms/MainForm.Designer.cs
namespace TicketApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // ---- UI bileşenleri ----
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.Panel panelHistory;

        private System.Windows.Forms.GroupBox groupBoxNewTicket;
        private System.Windows.Forms.GroupBox groupBoxHistory;

        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelSubArea;
        private System.Windows.Forms.Label labelLine;       // YENİ: Hat etiketi
        private System.Windows.Forms.Label labelIssue;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.Label labelPhoneNumber;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelCharCount;

        private System.Windows.Forms.ComboBox comboBoxArea;
        private System.Windows.Forms.ComboBox comboBoxSubArea;
        private System.Windows.Forms.ComboBox comboBoxLine; // YENİ: Hat combobox
        private System.Windows.Forms.ComboBox comboBoxIssue;

        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.TextBox textBoxPhoneNumber;

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.ListBox listBoxTickets;
        private System.Windows.Forms.PictureBox pictureBoxIcon;

        /// <summary>Kaynakları temizler.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>Form üzerindeki tüm bileşenleri başlatır ve yerleştirir</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.panelHeader = new System.Windows.Forms.Panel();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();

            this.panelMain = new System.Windows.Forms.Panel();
            this.panelForm = new System.Windows.Forms.Panel();
            this.groupBoxNewTicket = new System.Windows.Forms.GroupBox();

            this.labelArea = new System.Windows.Forms.Label();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();

            this.labelSubArea = new System.Windows.Forms.Label();
            this.comboBoxSubArea = new System.Windows.Forms.ComboBox();

            this.labelLine = new System.Windows.Forms.Label();
            this.comboBoxLine = new System.Windows.Forms.ComboBox();

            this.labelIssue = new System.Windows.Forms.Label();
            this.comboBoxIssue = new System.Windows.Forms.ComboBox();

            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelCharCount = new System.Windows.Forms.Label();

            this.labelFirstName = new System.Windows.Forms.Label();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();

            this.labelLastName = new System.Windows.Forms.Label();
            this.textBoxLastName = new System.Windows.Forms.TextBox();

            this.labelPhoneNumber = new System.Windows.Forms.Label();
            this.textBoxPhoneNumber = new System.Windows.Forms.TextBox();

            this.btnSubmit = new System.Windows.Forms.Button();

            this.panelHistory = new System.Windows.Forms.Panel();
            this.groupBoxHistory = new System.Windows.Forms.GroupBox();
            this.listBoxTickets = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();

            // ---- panelHeader ----
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 60;
            this.panelHeader.Controls.Add(this.pictureBoxIcon);
            this.panelHeader.Controls.Add(this.labelTitle);

            // ---- pictureBoxIcon ----
            this.pictureBoxIcon.Location = new System.Drawing.Point(15, 15);
            this.pictureBoxIcon.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxIcon.BackColor = System.Drawing.Color.Transparent;

            // ---- labelTitle ----
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(55, 18);
            this.labelTitle.Text = "IT Destek Talep Sistemi";

            // ---- panelMain ----
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Padding = new System.Windows.Forms.Padding(10);
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.panelMain.Controls.Add(this.panelForm);
            this.panelMain.Controls.Add(this.panelHistory);

            // ---- panelForm ----
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelForm.Width = 450;
            this.panelForm.Controls.Add(this.groupBoxNewTicket);

            // ---- groupBoxNewTicket ----
            this.groupBoxNewTicket.Text = "Yeni Talep";
            this.groupBoxNewTicket.BackColor = System.Drawing.Color.White;
            this.groupBoxNewTicket.Dock = System.Windows.Forms.DockStyle.Fill;

            // Saha formu düzeni (kolon genişlikleri sabit tutuldu)
            int leftX = 15;
            int labelW = 120;
            int inputW = 290;
            int rowH = 28;
            int gapY = 8;
            int curY = 25;

            // Alan
            this.labelArea.Text = "Alan";
            this.labelArea.SetBounds(leftX, curY, labelW, rowH);
            this.comboBoxArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArea.SetBounds(leftX + labelW, curY, inputW, rowH);
            curY += rowH + gapY;

            // Alt Alan
            this.labelSubArea.Text = "Alt Alan";
            this.labelSubArea.SetBounds(leftX, curY, labelW, rowH);
            this.comboBoxSubArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubArea.SetBounds(leftX + labelW, curY, inputW, rowH);
            curY += rowH + gapY;

            // YENİ — Hat
            this.labelLine.Text = "Hat";
            this.labelLine.SetBounds(leftX, curY, labelW, rowH);
            this.comboBoxLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLine.SetBounds(leftX + labelW, curY, inputW, rowH);
            curY += rowH + gapY;

            // Sorun
            this.labelIssue.Text = "Sorun";
            this.labelIssue.SetBounds(leftX, curY, labelW, rowH);
            this.comboBoxIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIssue.SetBounds(leftX + labelW, curY, inputW, rowH);
            curY += rowH + gapY;

            // Açıklama
            this.labelDescription.Text = "Açıklama (max 300)";
            this.labelDescription.SetBounds(leftX, curY, labelW, rowH);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.SetBounds(leftX + labelW, curY, inputW, 80);
            curY += 80 + 2;
            this.labelCharCount.Text = "0 / 300";
            this.labelCharCount.SetBounds(leftX + labelW + inputW - 70, curY, 70, rowH);
            curY += rowH + gapY;

            // Ad
            this.labelFirstName.Text = "Ad";
            this.labelFirstName.SetBounds(leftX, curY, labelW, rowH);
            this.textBoxFirstName.SetBounds(leftX + labelW, curY, inputW, rowH);
            curY += rowH + gapY;

            // Soyad
            this.labelLastName.Text = "Soyad";
            this.labelLastName.SetBounds(leftX, curY, labelW, rowH);
            this.textBoxLastName.SetBounds(leftX + labelW, curY, inputW, rowH);
            curY += rowH + gapY;

            // Telefon
            this.labelPhoneNumber.Text = "Telefon";
            this.labelPhoneNumber.SetBounds(leftX, curY, labelW, rowH);
            this.textBoxPhoneNumber.SetBounds(leftX + labelW, curY, inputW, rowH);
            curY += rowH + 12;

            // Gönder
            this.btnSubmit.Text = "Talebi Gönder";
            this.btnSubmit.SetBounds(leftX + labelW, curY, 150, 32);
            curY += 32 + gapY;

            // GroupBox'a ekle
            this.groupBoxNewTicket.Controls.AddRange(new System.Windows.Forms.Control[] {
                labelArea, comboBoxArea,
                labelSubArea, comboBoxSubArea,
                labelLine, comboBoxLine,         // YENİ
                labelIssue, comboBoxIssue,
                labelDescription, textBoxDescription, labelCharCount,
                labelFirstName, textBoxFirstName,
                labelLastName, textBoxLastName,
                labelPhoneNumber, textBoxPhoneNumber,
                btnSubmit
            });

            // ---- panelHistory ----
            this.panelHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistory.Padding = new System.Windows.Forms.Padding(10);
            this.panelHistory.Controls.Add(this.groupBoxHistory);

            // ---- groupBoxHistory ----
            this.groupBoxHistory.Text = "Geçmiş Talepler";
            this.groupBoxHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxHistory.BackColor = System.Drawing.Color.White;

            this.listBoxTickets.Dock = System.Windows.Forms.DockStyle.Fill;

            this.btnRefresh.Text = "Yenile";
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRefresh.Height = 36;

            this.groupBoxHistory.Controls.Add(this.listBoxTickets);
            this.groupBoxHistory.Controls.Add(this.btnRefresh);

            // ---- Form ----
            this.Text = "TicketApp - Main";
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Başlangıçta bazı alanlar devre dışı (zincir tamamlandıkça açılacak)
            this.comboBoxSubArea.Enabled = false;
            this.comboBoxLine.Enabled = false;   // YENİ
            this.comboBoxIssue.Enabled = false;
        }
    }
}
