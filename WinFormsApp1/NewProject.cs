namespace Aplikacja_Projektowa
{
    public partial class NewProject : Form
    {
        private DatabaseManager db;

        public NewProject()
        {
            InitializeComponent();
            db = new DatabaseManager();
        }

        // Przycisk zapisz projekt
        private void ButtonSaveProject_Click(object sender, EventArgs e)
        {
            string projectName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(projectName))
            {
                MessageBox.Show("Please enter a project name.");
                return;
            }

            int projectId = db.AddProject(projectName);

            if (projectId > 0)
            {
                MessageBox.Show($"Project added successfully!\nID: {projectId}\nName: {projectName}",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Failed to add project. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
