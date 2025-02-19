namespace Aplikacja_Projektowa
{
    public partial class MainForm : Form
    {
        private DatabaseManager db;

        public MainForm()
        {
            InitializeComponent();
            db = new DatabaseManager();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();
            string selectedFilter = comboBox1.SelectedItem?.ToString() ?? "All files"; // Domyślnie ustawiam "All files"
            DatabaseManager db = new DatabaseManager();

            if (selectedFilter == "Projects ()")
            {
                // 🔹 Jeśli wybrano "Projects ()" → wyszukaj projekty
                List<Project> projects = db.GetProjects();
                if (projects.Count == 0)
                {
                    MessageBox.Show("No projects found.");
                    return;
                }
                Results.DataSource = projects;
            }
            else
            {
                List<FileEntry> files = db.SearchFiles(searchText);

                // Filtrowanie wyników na podstawie ComboBox
                if (selectedFilter == "Reports (*.txt)")
                    files = files.Where(f => f.FileType == "Report").ToList();
                else if (selectedFilter == "Design (*.prt; *.stp)")
                    files = files.Where(f => f.FileType == "Design" || f.FileType == "stp").ToList();
                else if (selectedFilter == "Approval (*.pdf)")
                    files = files.Where(f => f.FileType == "Approval").ToList();

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
            EditItem editItem = new EditItem(new FileEntry(-1, 1, "", "Design", "", DateTime.Now));
            editItem.Show();
        }

        private void measurementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditItem editItem = new EditItem(new FileEntry(-1, 1, "", "Measurement", "", DateTime.Now));
            editItem.Show();
        }

        private void approvalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditItem editItem = new EditItem(new FileEntry(-1, 1, "", "Approval", "", DateTime.Now));
            editItem.Show();
        }

        private void Results_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

                // Pobieramy kolumnę "FileType", jeśli istnieje - czyli sprawdzamy, czy użytkownik wybrał plik czy projekt
                string fileType = selectedRow.Cells["FileType"].Value?.ToString();

                if (!string.IsNullOrEmpty(fileType))
                {
                    // 🟢 To jest plik, więc wczytujemy jego dane
                    int fileId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                    string fileName = selectedRow.Cells["FileName"].Value.ToString();
                    string filePath = selectedRow.Cells["FilePath"].Value.ToString();
                    DateTime modifiedDate = DateTime.Parse(selectedRow.Cells["ModifiedDate"].Value.ToString());

                    // Pobieramy ProjectId na podstawie pliku
                    int projectId = db.GetProjectIdByFileId(fileId);

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
