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
                    id INTEGER PRIMARY KEY NOT NULL UNIQUE,
                    name TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS files (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
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
        // Dodawanie projektu do bazy, nie ma on numeru autoinkrementowanego uzywana w oknie NewProject
        public int AddProject(int id, string name)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT COUNT(*) FROM projects WHERE id = $id";
                checkCommand.Parameters.AddWithValue("$id", id);

                long count = (long)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show($"Projekt o ID {id} już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO projects (id, name) 
                    VALUES ($id, $name);";

                command.Parameters.AddWithValue("$id", id);
                command.Parameters.AddWithValue("$name", name);

                return command.ExecuteNonQuery(); // Zwraca 1, jeśli udało się dodać rekord
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
        public void AddFile(int projectId, string fileName, string fileType, string filePath, DateTime modifiedDate)
        {
            if (projectId == -1)
            {
                MessageBox.Show("Error: No valid project selected for this file.");
                return;
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO files (project_id, filename, filetype, filepath, modified_date) 
                VALUES ($projectId, $filename, $filetype, $filepath, $modifiedDate);";
                command.Parameters.AddWithValue("$projectId", projectId);
                command.Parameters.AddWithValue("$filename", fileName);
                command.Parameters.AddWithValue("$filetype", fileType);
                command.Parameters.AddWithValue("$filepath", filePath);
                command.Parameters.AddWithValue("$modifiedDate", modifiedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
            }
        }
        
        public void UpdateFile(int fileId, int projectId, string fileName, string fileType, string filePath, DateTime modifiedDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
            UPDATE files 
            SET project_Id = $projectId, 
                fileName = $fileName, 
                fileType = $fileType, 
                filePath = $filePath, 
                modified_Date = $modifiedDate
            WHERE id = $fileId;
        ";

                command.Parameters.AddWithValue("$fileId", fileId);
                command.Parameters.AddWithValue("$projectId", projectId);
                command.Parameters.AddWithValue("$fileName", fileName);
                command.Parameters.AddWithValue("$fileType", fileType);
                command.Parameters.AddWithValue("$filePath", filePath);
                command.Parameters.AddWithValue("$modifiedDate", modifiedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
            }
        }
        

        // ✅ Metoda pobierania plików projektu
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
                            reader.GetString(3),
                            reader.GetString(4),
                            DateTime.Parse(reader.GetString(5))
                        ));
                    }
                }
            }
            return files;
        }
        public List<FileEntry> SearchFiles(string query)
        {
            var files = new List<FileEntry>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
            SELECT id, project_id, filename, filetype, filepath, modified_date 
            FROM files 
            WHERE filename LIKE $query OR filetype LIKE $query OR filepath LIKE $query;";

                command.Parameters.AddWithValue("$query", "%" + query + "%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        files.Add(new FileEntry(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            DateTime.Parse(reader.GetString(5))
                        ));
                    }
                }
            }
            return files;
        }
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



    }
}
