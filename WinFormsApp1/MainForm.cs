using System;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnAddPart_Click(object sender, EventArgs e)
        {
            string partName = Microsoft.VisualBasic.Interaction.InputBox("Enter part name:", "Add Part", "");
            if (!string.IsNullOrWhiteSpace(partName))
            {
                string status = "Ordered"; // Domyœlny status zamówienia
                string orderDate = DateTime.Now.ToString("yyyy-MM-dd"); // Dzisiejsza data
                string measurementFile = "Not assigned"; // Brak pliku na pocz¹tku
                string measurementStatus = "No"; // Brak wykonanego pomiaru

                // Tworzenie nowego elementu ListView
                ListViewItem item = new ListViewItem(new string[]
                {
                    partName,
                    status,
                    orderDate,
                    measurementFile,
                    measurementStatus
                });

                // Dodanie elementu do ListView
                lstParts.Items.Add(item);
            }
        }

        private void BtnSaveReport_Click(object sender, EventArgs e)
        {
            if (lstParts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a part to assign a measurement file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files|*.txt|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Pobranie wybranego elementu
                    ListViewItem selectedItem = lstParts.SelectedItems[0];

                    // Zmiana wartoœci w kolumnie "Measurement File"
                    selectedItem.SubItems[3].Text = ofd.FileName;
                    selectedItem.SubItems[4].Text = "Yes"; // Oznaczenie, ¿e pomiar zosta³ dokonany
                }
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReportPath.Text))
            {
                MessageBox.Show("Please select a report file location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (StreamWriter writer = new StreamWriter(txtReportPath.Text))
            {
                writer.WriteLine("Part Name,Status,Order Date,Measurement File,Measured?");
                foreach (ListViewItem item in lstParts.Items)
                {
                    writer.WriteLine($"{item.SubItems[0].Text},{item.SubItems[1].Text},{item.SubItems[2].Text},{item.SubItems[3].Text},{item.SubItems[4].Text}");
                }
            }
            MessageBox.Show("Report saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
 }

