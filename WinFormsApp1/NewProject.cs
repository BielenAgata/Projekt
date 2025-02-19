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
            string projectIdText = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(projectName))
            {
                MessageBox.Show("Please enter a project name.");
                return;
            } //sprawdzenie czy podano nazwe
            if (string.IsNullOrEmpty(projectIdText))
            {
                MessageBox.Show("Please enter a project ID.");
                return;
            } //sprawdzenie czy podano ID
            if (!int.TryParse(projectIdText, out int projectId) || projectId < 1)
            {
                MessageBox.Show("Error creating project.");
                return;
            } //sprawdzenie czy ID>0 oraz czy jest to liczba
            int result = db.AddProject(projectId, projectName);
            if (result > 0)
            {
                MessageBox.Show($"Projekt został dodany!\nID: {projectId}\nNazwa: {projectName}",
                                "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == -1)
            {
                MessageBox.Show("Nie udało się dodać projektu, ponieważ ID jest już zajęte.",
                                "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
