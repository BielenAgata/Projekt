namespace Aplikacja_Projektowa
{
    public class FileEntry
    {
        public enum FileType
        {
            Design,
            Approval,
            Measurement
        }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string FileName { get; set; }
        public FileType Type { get; set; }
        public string FilePath { get; set; }
        public DateTime ModifiedDate { get; set; }

        public FileEntry(int id, int projectId, string fileName, FileType fileType, string filePath, DateTime modifiedDate)
        {
            Id = id;
            ProjectId = projectId;
            FileName = fileName;
            Type = fileType;
            FilePath = filePath;
            ModifiedDate = modifiedDate;
        }
    }
}