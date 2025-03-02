namespace Aplikacja_Projektowa
{
    public partial class MainForm : Form
    {
        private DatabaseManager db;

        public MainForm()
        {
            InitializeComponent();
            db = new DatabaseManager();
            //ustawienie combobox, jak nie wybrano nic innego na all files
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0; // Wybiera pierwszy element
            }
        }
        //wyszukiwanie
        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();
            string selectedFilter = comboBox1.SelectedItem?.ToString() ?? "All files";
            DatabaseManager db = new DatabaseManager();

            if (selectedFilter == "Projects ()")
            {
                List<Project> projects = string.IsNullOrEmpty(searchText) ? db.GetProjects() : db.SearchProjects(searchText);

                if (projects.Count == 0)
                {
                    MessageBox.Show("No projects found.");
                    return;
                }

                Results.DataSource = null;  // 🔹 Resetujemy DataGridView
                Results.DataSource = projects;
            }
            else
            {
                FileEntry.FileType? filterType = null;

                if (selectedFilter == "Design (*.prt)")
                    filterType = FileEntry.FileType.Design;
                else if (selectedFilter == "Measurement (*.txt)")
                    filterType = FileEntry.FileType.Measurement;
                else if (selectedFilter == "Approval (*.pdf)")
                    filterType = FileEntry.FileType.Approval;

                // Jeśli searchText jest pusty → Pobierz wszystkie pliki
                List<FileEntry> files = string.IsNullOrEmpty(searchText)
                    ? db.SearchFiles(null, filterType)
                    : db.SearchFiles(searchText, filterType);

                if (files.Count == 0)
                {
                    MessageBox.Show("No files found.");
                    return;
                }
                Results.DataSource = files;
            }
        }
        //menu rozwijalne
        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject projectExplorer = new NewProject();
            projectExplorer.Show();
        }

        private void designToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAddItemWindow(FileEntry.FileType.Design);
        }
        private void measurementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAddItemWindow(FileEntry.FileType.Measurement);
        }
        private void approvalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAddItemWindow(FileEntry.FileType.Approval);
        }

        // Funkcja pomocnicza do otwierania `AddItem`
        private void OpenAddItemWindow(FileEntry.FileType fileType)
        {
            AddItem addItem = new AddItem(new FileEntry(-1, 1, "", fileType, "", DateTime.Now));
            addItem.Show();
        }
        //edytowanie istniejacego pliku
        private void EditItem_Click(object sender, EventArgs e)
        {
            if (Results.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to edit.");
                return;
            }

            DataGridViewRow selectedRow = Results.SelectedRows[0];

            try
            {
                DatabaseManager db = new DatabaseManager();

                // 🔹 Sprawdzenie, czy `"FileType"` istnieje w DataGridView
                bool hasFileTypeColumn = Results.Columns.Cast<DataGridViewColumn>().Any(col => col.Name == "Type");

                if (!hasFileTypeColumn)
                {
                    MessageBox.Show("You cannot edit a Project.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 Pobranie wartości `"Type"` (FileType)
                object fileTypeObj = selectedRow.Cells["Type"].Value;

                if (fileTypeObj == null || string.IsNullOrEmpty(fileTypeObj.ToString()))
                {
                    MessageBox.Show("You cannot edit a Project.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 Konwersja `string -> FileEntry.FileType` z walidacją
                if (!Enum.TryParse(fileTypeObj.ToString(), out FileEntry.FileType fileType))
                {
                    MessageBox.Show("Invalid file type detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 🔹 Pobranie danych pliku
                int fileId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string fileName = selectedRow.Cells["FileName"].Value.ToString();
                string filePath = selectedRow.Cells["FilePath"].Value.ToString();
                DateTime modifiedDate = DateTime.Parse(selectedRow.Cells["ModifiedDate"].Value.ToString());

                // 🔹 Pobranie `ProjectId` na podstawie pliku
                int projectId = db.GetProjectIdByFileId(fileId);

                // 🔹 Utworzenie obiektu `FileEntry` i otwarcie okna edycji
                FileEntry selectedFile = new FileEntry(fileId, projectId, fileName, fileType, filePath, modifiedDate);
                EditItem editItem = new EditItem(selectedFile);
                editItem.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (Results.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a project to open.");
                return;
            }

            DataGridViewRow selectedRow = Results.SelectedRows[0];

            try
            {
                // 🔹 Sprawdzenie, czy `Results` zawiera kolumnę "FileType"
                bool hasFileTypeColumn = Results.Columns.Cast<DataGridViewColumn>().Any(col => col.Name == "FileType");

                // 🔹 Jeśli kolumna `FileType` istnieje → to plik, nie projekt
                if (hasFileTypeColumn)
                {
                    MessageBox.Show("You cannot open a file. Please select a project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Pobranie ID i nazwy projektu
                object projectIdObj = selectedRow.Cells["ID"].Value;
                object projectNameObj = selectedRow.Cells["Name"].Value;

                if (projectIdObj == null || projectNameObj == null)
                {
                    MessageBox.Show("Invalid project selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int projectId = Convert.ToInt32(projectIdObj);
                string projectName = projectNameObj.ToString();

                // ✅ Otwórz `ProjectExplorer` i przekaż `projectId`
                ProjectExplorer projectExplorer = new ProjectExplorer(projectId, projectName);
                projectExplorer.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"You cannot open a file. Select a project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Results.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = Results.SelectedRows[0];

            try
            {
                // Sprawdzenie, czy zaznaczony wiersz ma kolumnę "Type" → to oznacza, że to plik
                bool hasFileTypeColumn = Results.Columns.Cast<DataGridViewColumn>().Any(col => col.Name == "Type");

                if (!hasFileTypeColumn)
                {
                    MessageBox.Show("You cannot delete a Project. Please select a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Pobranie `File ID`
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
                    button1_Click(sender, e); // Ponowne wyszukanie plików
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
