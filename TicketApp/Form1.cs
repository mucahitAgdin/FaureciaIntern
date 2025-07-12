using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicketApp.Models;
using TicketApp.Helpers;

namespace TicketApp
{
    public partial class Form1 : Form
    {
        private ComboBox comboBoxArea;
        private ComboBox comboBoxIssue;
        private TextBox textBoxDescription;
        private Button btnSubmit;
        private ListBox listBoxTickets;
        private Dictionary<string, List<string>> issueMap = new Dictionary<string, List<string>>();

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            InitializeApp();
        }

        private void InitializeApp()
        {
            try
            {
                DatabaseHelper.InitializeDatabase();

                comboBoxArea.Items.AddRange(new string[] { "UAP-1", "UAP-2", "UAP-3", "UAP-4", "FES" });
                comboBoxArea.SelectedIndexChanged += ComboBoxArea_SelectedIndexChanged;

                // Sorun haritaları
                issueMap["GENEL"] = new List<string> {
                    "Computer won't start",
                    "Mouse not working",
                    "Keyboard issue",
                    "Paper jam in printer",
                    "Printer not connected",
                    "Printer not working"
                };

                issueMap["UAP-1"] = new List<string> { "SAP Terminal freezes", "Barcode printer issue" };
                issueMap["UAP-2"] = new List<string> { "IP unreachable" };
                issueMap["FES"] = new List<string> { "PLC connection lost", "Label printer offline" };
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Initialization error. Check log file.", "Error");
            }
        }

        private void ComboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string area = comboBoxArea.SelectedItem.ToString();
                comboBoxIssue.Items.Clear();

                if (issueMap.ContainsKey(area))
                    comboBoxIssue.Items.AddRange(issueMap[area].ToArray());

                comboBoxIssue.Items.AddRange(issueMap["GENEL"].ToArray());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Error loading issues.", "Error");
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxArea.SelectedItem == null || comboBoxIssue.SelectedItem == null)
                {
                    MessageBox.Show("Please select both Area and Issue.", "Validation Error");
                    return;
                }

                var ticket = new Ticket
                {
                    Area = comboBoxArea.SelectedItem.ToString(),
                    Issue = comboBoxIssue.SelectedItem.ToString(),
                    Description = textBoxDescription.Text.Trim(),
                    CreatedAt = DateTime.Now
                };

                DatabaseHelper.InsertTicket(ticket);

                listBoxTickets.Items.Add($"[{ticket.CreatedAt}] {ticket.Area} - {ticket.Issue}");
                comboBoxIssue.SelectedIndex = -1;
                textBoxDescription.Clear();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Ticket submission failed.", "Error");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Not used anymore
        }
    }
}