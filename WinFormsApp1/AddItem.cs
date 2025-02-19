using System.Data;

namespace Aplikacja_Projektowa
{
    public partial class AddItem : Form
    {
        private DatabaseManager db;
       
        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = textBox1.Text.Trim();
            string filePath = textBox3.Text.Trim();
            string fileType = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please enter a valid file name and path.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1ProjectId.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid project before saving.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pobranie ID projektu
            string selectedProjectText = comboBox1ProjectId.SelectedItem.ToString();
            int projectId = ExtractProjectId(selectedProjectText);

            if (projectId == -1)
            {
                MessageBox.Show("Error: No valid project selected.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine($"Adding new file: {fileName}, Type: {fileType}, ProjectID: {projectId}");
            db.AddFile(projectId, fileName, fileType, filePath, DateTime.Now);
            MessageBox.Show("New file added successfully.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private int ExtractProjectId(string selectedProjectText)
        {
            // Znalezienie wartości ID wewnątrz nawiasów (ID: X)
            var match = System.Text.RegularExpressions.Regex.Match(selectedProjectText, @"\d+");
            return match.Success ? int.Parse(match.Value) : -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}