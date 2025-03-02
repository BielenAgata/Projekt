using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Aplikacja_Projektowa
{
    public partial class ProjectExplorer : Form
    {
        private int projectId;
        private string projectName;
        private DatabaseManager db;

        public ProjectExplorer(int projectId, string projectName)
        {
            InitializeComponent();
            this.projectId = projectId;
            this.projectName = projectName;
            this.db = new DatabaseManager();

            this.Text = $"Project Explorer - {projectName}"; // Ustawienie tytułu okna

            // ✅ Wypełniamy pola ID i nazwy projektu
            textBox1.Text = projectName;
            textBox2.Text = projectId.ToString();

            LoadProjectFiles();
        }

        private void LoadProjectFiles()
        {
            List<FileEntry> files = db.GetFiles(projectId);

            if (files.Count == 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear(); // ✅ Ensure UI is reset properly
                return;
            }

            var fileDisplayList = files.Select(file => new
            {
                ID = file.Id,
                Name = file.FileName,  // ✅ Ensure column names match
                Type = file.Type.ToString(),
                Path = file.FilePath,
                ModifiedDate = file.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            dataGridView1.DataSource = null;  // ✅ Clear before updating
            dataGridView1.DataSource = fileDisplayList;
        }
        //edit item
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to edit.");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            try
            {
                DatabaseManager db = new DatabaseManager();

                // 🔹 Sprawdzenie, czy `"Type"` istnieje w tabeli (czy to plik, a nie projekt)
                bool hasFileTypeColumn = dataGridView1.Columns.Cast<DataGridViewColumn>().Any(col => col.Name == "Type");

                if (!hasFileTypeColumn)
                {
                    MessageBox.Show("You cannot edit a Project.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 Pobranie wartości `"Type"`
                object fileTypeObj = selectedRow.Cells["Type"]?.Value;
                if (fileTypeObj == null || string.IsNullOrEmpty(fileTypeObj.ToString()))
                {
                    MessageBox.Show("Invalid file selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 Konwersja `string -> FileEntry.FileType`
                if (!Enum.TryParse(fileTypeObj.ToString(), out FileEntry.FileType fileType))
                {
                    MessageBox.Show("Invalid file type detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 🔹 Pobranie szczegółów pliku
                int fileId = Convert.ToInt32(selectedRow.Cells[0].Value);  // ID
                string fileName = selectedRow.Cells[1].Value.ToString();   // Name
                string filePath = selectedRow.Cells[3].Value.ToString();   // Path
                DateTime modifiedDate = DateTime.Parse(selectedRow.Cells[4].Value.ToString()); // Modified Date

                // 🔹 Pobranie `ProjectId`
                int projectId = db.GetProjectIdByFileId(fileId);

                // 🔹 Otworzenie okna edycji
                FileEntry selectedFile = new FileEntry(fileId, projectId, fileName, fileType, filePath, modifiedDate);
                EditItem editItem = new EditItem(selectedFile);

                // 🔹 Jeśli użytkownik kliknie "Save", odśwież listę plików
                if (editItem.ShowDialog() == DialogResult.OK)
                {
                    LoadProjectFiles();  // ✅ Odświeżenie listy po edycji
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //add item
        private void button1_Click(object sender, EventArgs e)
        {
            // 🔹 Pobranie listy projektów, aby użytkownik mógł wybrać projekt dla pliku
            List<Project> projects = db.GetProjects();

            if (projects.Count == 0)
            {
                MessageBox.Show("No projects available. Please create a project first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Otwórz `AddItem`, domyślnie ustawiając projekt i typ "Design"
            AddItem addItem = new AddItem(new FileEntry(-1, projectId, "", FileEntry.FileType.Design, "", DateTime.Now));

            // 🔹 Sprawdzenie, czy użytkownik dodał plik (zamknął okno `AddItem` z `DialogResult.OK`)
            if (addItem.ShowDialog() == DialogResult.OK)
            {
                LoadProjectFiles(); // ⬅️ Odśwież listę plików
            }
        }

        //delete item
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a file to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            try
            {
                // Pobieramy `File ID`
                object fileIdObj = selectedRow.Cells["ID"].Value;

                if (fileIdObj == null)
                {
                    MessageBox.Show("Error: Invalid file selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int fileId = Convert.ToInt32(fileIdObj);

                // Potwierdzenie usunięcia
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this file?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // ✅ Usunięcie pliku z bazy danych
                    db.DeleteFile(fileId);

                    // ✅ Odświeżenie listy plików
                    LoadProjectFiles();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
