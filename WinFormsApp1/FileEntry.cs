namespace Aplikacja_Projektowa
{
    public class FileEntry
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime ModifiedDate { get; set; }

        public FileEntry(int id, int projectId, string fileName, string fileType, string filePath, DateTime modifiedDate)
        {
            Id = id;
            ProjectId = projectId;
            FileName = fileName;
            FileType = fileType;
            FilePath = filePath;
            ModifiedDate = modifiedDate;
        }
    }
}// Obiekty klasy plik