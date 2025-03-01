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
                //ConfigureProjectResultsGrid(); // 🔹 Konfigurujemy DataGridView

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

                // Pobranie FileType (sprawdzamy, czy to plik czy projekt)
                string fileTypeStr = selectedRow.Cells["FileType"].Value?.ToString();

                if (!string.IsNullOrEmpty(fileTypeStr))
                {
                    // ✅ Konwersja string → enum FileEntry.FileType
                    FileEntry.FileType fileType = Enum.Parse<FileEntry.FileType>(fileTypeStr);

                    // ✅ Pobranie danych pliku
                    int fileId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                    string fileName = selectedRow.Cells["FileName"].Value.ToString();
                    string filePath = selectedRow.Cells["FilePath"].Value.ToString();
                    DateTime modifiedDate = DateTime.Parse(selectedRow.Cells["ModifiedDate"].Value.ToString());

                    // ✅ Pobranie ProjectId na podstawie pliku
                    int projectId = db.GetProjectIdByFileId(fileId);

                    // ✅ Utworzenie obiektu FileEntry i otwarcie okna edycji
                    FileEntry selectedFile = new FileEntry(fileId, projectId, fileName, fileType, filePath, modifiedDate);
                    EditItem editItem = new EditItem(selectedFile);
                    editItem.Show();
                }
                else
                {
                    MessageBox.Show("You cannot edit Project");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading details: " + ex.Message);
            }
        }
    }
}
