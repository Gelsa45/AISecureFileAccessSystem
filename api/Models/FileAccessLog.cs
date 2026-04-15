namespace FileAccessSystem.Models
{
    public class FileAccessLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int FileItemId { get; set; }

        public DateTime AccessTime { get; set; }
    }
}