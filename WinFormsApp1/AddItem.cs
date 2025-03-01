using System.Data;

namespace Aplikacja_Projektowa
{
    public partial class AddItem : Form
    {
        private DatabaseManager db;
        private FileEntry currentFile;

        public AddItem(FileEntry fileEntry)
        {
            InitializeComponent();
            db = new DatabaseManager();
            currentFile = fileEntry;

            // 🔹 Automatyczne wypełnienie ComboBox wartościami z `FileEntry.FileType`
            comboBox1.Items.AddRange(Enum.GetNames(typeof(FileEntry.FileType)));

            // 🔹 Ustawienie domyślnych wartości w formularzu
            textBox1.Text = currentFile.FileName;
            textBox3.Text = currentFile.FilePath;
            comboBox1.SelectedItem = currentFile.Type.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = textBox1.Text.Trim();
            string filePath = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please enter a valid file name and path.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Sprawdzamy, czy wybrano `FileType`
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid file type.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Pobranie wartości `enum` zamiast string
            FileEntry.FileType fileType;
            if (!Enum.TryParse(comboBox1.SelectedItem.ToString(), out fileType))
            {
                MessageBox.Show("Invalid file type selected.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Pobranie ID projektu
            if (comboBox1ProjectId.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid project before saving.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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