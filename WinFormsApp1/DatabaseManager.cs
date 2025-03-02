using Microsoft.Data.Sqlite;

namespace Aplikacja_Projektowa
{
    public class DatabaseManager
    {
        private readonly string connectionString = "Data Source=projects.db";

        public DatabaseManager()
        {
            EnsureDatabaseExists();
        }
        // Sprawdzenie, czy DB istnieje
        private void EnsureDatabaseExists()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS projects (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS files (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    project_id INTEGER NOT NULL,
                    filename TEXT NOT NULL,
                    filetype TEXT NOT NULL,
                    filepath TEXT NOT NULL,
                    modified_date TEXT NOT NULL,
                    FOREIGN KEY (project_id) REFERENCES projects(id) ON DELETE CASCADE
                );";
                command.ExecuteNonQuery();
            }
        }
        // Dodawanie projektu do bazy, numer autoinkrementowany uzywana w oknie NewProject
        public int AddProject(string name)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
            INSERT INTO projects (name) 
            VALUES ($name);
            SELECT last_insert_rowid();"; // Returns the auto-incremented ID

                command.Parameters.AddWithValue("$name", name);
                int newProjectId = Convert.ToInt32(command.ExecuteScalar());

                return newProjectId;
            }
        }
        //Pobranie listy istniejących projektow w bazie, uzywana w oknie MainForm
        public List<Project> GetProjects()
        {
            var projects = new List<Project>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, name FROM projects;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        projects.Add(new Project(id, name));
                    }
                }
            }
            return projects;
        }

        // Funkcja pozwalająca na dodanie nowego pliku w oknie ViewDetails
        public int AddFile(int projectId, string fileName, FileEntry.FileType fileType, string filePath, DateTime modifiedDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
            INSERT INTO files (project_id, filename, filetype, filepath, modified_date) 
            VALUES ($projectId, $filename, $filetype, $filepath, $modifiedDate);
            SELECT last_insert_rowid();"; // ✅ Retrieve new ID

                command.Parameters.AddWithValue("$projectId", projectId);
                command.Parameters.AddWithValue("$filename", fileName);
                command.Parameters.AddWithValue("$filetype", fileType.ToString());
                command.Parameters.AddWithValue("$filepath", filePath);
                command.Parameters.AddWithValue("$modifiedDate", modifiedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                return Convert.ToInt32(command.ExecuteScalar());  // ✅ Return new file ID
            }
        }


        //funkcja update file, zmienia istniejący rekord typu file
        public void UpdateFile(int fileId, int projectId, string fileName, FileEntry.FileType fileType, string filePath, DateTime modifiedDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // ✅ Check if Project ID exists before updating
                var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT COUNT(*) FROM projects WHERE id = $projectId;";
                checkCommand.Parameters.AddWithValue("$projectId", projectId);

                long projectExists = (long)checkCommand.ExecuteScalar();
                if (projectExists == 0)
                {
                    MessageBox.Show($"Project ID {projectId} does not exist. Cannot update file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var command = connection.CreateCommand();
                command.CommandText = @"
            UPDATE files 
            SET project_id = $projectId, 
                filename = $fileName, 
                filetype = $fileType, 
                filepath = $filePath, 
                modified_date = $modifiedDate
            WHERE id = $fileId;
        ";

                command.Parameters.AddWithValue("$fileId", fileId);
                command.Parameters.AddWithValue("$projectId", projectId);
                command.Parameters.AddWithValue("$fileName", fileName);
                command.Parameters.AddWithValue("$fileType", fileType.ToString()); // Convert enum to string
                command.Parameters.AddWithValue("$filePath", filePath);
                command.Parameters.AddWithValue("$modifiedDate", modifiedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
            }
        }

        // Metoda pobierania plików projektu
        public List<FileEntry> GetFiles(int projectId)
        {
            var files = new List<FileEntry>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, project_id, filename, filetype, filepath, modified_date FROM files WHERE project_id = $projectId;";
                command.Parameters.AddWithValue("$projectId", projectId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        files.Add(new FileEntry(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetString(2),
                            Enum.Parse<FileEntry.FileType>(reader.GetString(3)), // Convert string to enum
                            reader.GetString(4),
                            DateTime.Parse(reader.GetString(5))
                        ));
                    }
                }
            }
            return files;
        }
        //wyszukiwanie plikow
        public List<FileEntry> SearchFiles(string query, FileEntry.FileType? fileType = null)
        {
            var files = new List<FileEntry>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Obsługa pustego `query` → Jeśli brak tekstu, SQL pobierze wszystkie pliki
                string searchCondition = string.IsNullOrEmpty(query) ? "1=1" : "(filename LIKE $query OR filepath LIKE $query)";

                // Obsługa `fileType` → Jeśli brak filtru, pobiera wszystkie typy
                string fileTypeCondition = fileType == null ? "1=1" : "filetype = $fileType";

                // Tworzenie zapytania SQL
                command.CommandText = $@"
                    SELECT id, project_id, filename, filetype, filepath, modified_date 
                    FROM files 
                    WHERE {searchCondition} 
                    AND {fileTypeCondition};";

                // Dodanie parametrów
                if (!string.IsNullOrEmpty(query))
                    command.Parameters.AddWithValue("$query", "%" + query + "%");

                if (fileType != null)
                    command.Parameters.AddWithValue("$fileType", fileType.ToString());

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        files.Add(new FileEntry(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetString(2),
                            Enum.Parse<FileEntry.FileType>(reader.GetString(3)), // Zamiana string na enum
                            reader.GetString(4),
                            DateTime.Parse(reader.GetString(5))
                        ));
                    }
                }
            }
            return files;
        }
        //wyszukiwanie projektow
        public List<Project> SearchProjects(string query)
        {
            var projects = new List<Project>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Jeśli brak `query`, pobierz wszystkie projekty
                string searchCondition = string.IsNullOrEmpty(query) ? "1=1" : "name LIKE $query";

                command.CommandText = $@"
                    SELECT id, name FROM projects
                    WHERE {searchCondition};";

                if (!string.IsNullOrEmpty(query))
                    command.Parameters.AddWithValue("$query", "%" + query + "%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projects.Add(new Project(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return projects;
        }
        //pobiera id do edycji pliku
        public int GetProjectIdByFileId(int fileId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT project_id FROM files WHERE id = $fileId;";
                command.Parameters.AddWithValue("$fileId", fileId);
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        //usuwa plik z db
        public void DeleteFile(int fileId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM files WHERE id = $fileId;";
                    command.Parameters.AddWithValue("$fileId", fileId);
                    command.ExecuteNonQuery();
                }
            }
        }
        //sprawdza czy istnieje proejkt
        public bool ProjectExists(int projectId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM projects WHERE id = $projectId;";
                command.Parameters.AddWithValue("$projectId", projectId);

                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

    }
}
