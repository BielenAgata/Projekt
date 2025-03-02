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

            // 🔹 Ładujemy projekty do `ComboBox`
            LoadProjectsIntoComboBox();

            // 🔹 Automatyczne wypełnienie ComboBox typami plików
            comboBox1.Items.AddRange(Enum.GetNames(typeof(FileEntry.FileType)));

            // 🔹 Wypełnienie formularza domyślnymi wartościami
            textBox1.Text = currentFile.FileName;
            textBox3.Text = currentFile.FilePath;
            comboBox1.SelectedItem = currentFile.Type.ToString();

            if (comboBox1.Items.Count == 0)
            {
                comboBox1.Items.AddRange(Enum.GetNames(typeof(FileEntry.FileType)));
            }

            // 🔹 Jeśli użytkownik edytuje plik, wybierz jego projekt
            if (currentFile.ProjectId != -1)
            {
                comboBox1ProjectId.SelectedItem = comboBox1ProjectId.Items
                    .Cast<string>()
                    .FirstOrDefault(item => item.Contains($"(ID: {currentFile.ProjectId})"));
            }
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

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid file type.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Enum.TryParse(comboBox1.SelectedItem.ToString(), out FileEntry.FileType fileType))
            {
                MessageBox.Show("Invalid file type selected.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            // Add file and retrieve new ID
            int newFileId = db.AddFile(projectId, fileName, fileType, filePath, DateTime.Now);

            if (newFileId != -1)
            {
                textBox4.Text = newFileId.ToString();  // ✅ Auto-fill File ID
                MessageBox.Show("New file added successfully.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Error adding file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void LoadProjectsIntoComboBox()
        {
            comboBox1ProjectId.Items.Clear(); // Czyszczenie starej listy projektów

            List<Project> projects = db.GetProjects(); // 🔹 Pobranie listy projektów

            if (projects.Count == 0)
            {
                MessageBox.Show("No projects available. Please create a project first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var project in projects)
            {
                comboBox1ProjectId.Items.Add($"{project.Name} (ID: {project.Id})");
            }

            // Ustawienie domyślnego projektu, jeśli użytkownik nie edytuje istniejącego pliku
            if (comboBox1ProjectId.Items.Count > 0 && currentFile.ProjectId == -1)
            {
                comboBox1ProjectId.SelectedIndex = 0;
            }
        }

    }
}