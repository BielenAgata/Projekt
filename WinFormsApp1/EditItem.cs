﻿using System.Data;

namespace Aplikacja_Projektowa
{
    public partial class EditItem : Form
    {
        private FileEntry file;
        //private FileEntry createdFile;
        private DatabaseManager db;

        // 🔹 Konstruktor dla edycji istniejącego pliku
        public EditItem(FileEntry file)
        {
            InitializeComponent();
            this.db = new DatabaseManager();
            this.file = file;
            this.Text = "Edit File";
            button1.Text = "Save Changes";

            LoadProjectsIntoComboBox(); //Ładujemy listę projektów
            LoadFileDetails(); // Dane są wczytywane

            // Wybieramy ID projektu w comboBox
            comboBox1ProjectId.SelectedItem = comboBox1ProjectId.Items
                .Cast<string>()
                .FirstOrDefault(item => item.Contains($"(ID: {file.ProjectId})"));
        }


        private void LoadFileDetails()
        {
            textBox1.Text = file.FileName;
            textBox3.Text = file.FilePath;
            textBox4.Text = file.Id.ToString();
            comboBox1.Text = file.FileType;

            // Jeśli Project ID jest teraz w ComboBoxie, zaznacz istniejący projekt
            if (comboBox1ProjectId.Items.Contains($"ID: {file.ProjectId}"))
            {
                comboBox1ProjectId.SelectedItem = $"ID: {file.ProjectId}";
            }
        }

        private void LoadProjectsIntoComboBox()
        {
            comboBox1ProjectId.Items.Clear();

            List<Project> projects = db.GetProjects();

            foreach (var project in projects)
            {
                comboBox1ProjectId.Items.Add($"{project.Name} (ID: {project.Id})");
            }
        }

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

            Console.WriteLine($"Updating new file: {fileName}, Type: {fileType}, ProjectID: {projectId}");
            db.UpdateFile(file.Id, projectId, fileName, fileType, filePath, DateTime.Now);
            MessageBox.Show("File was updated added successfully.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
