namespace FileAccessSystem.Models
{
    public class RiskLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int RiskScore { get; set; }
        public string RiskLevel { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}