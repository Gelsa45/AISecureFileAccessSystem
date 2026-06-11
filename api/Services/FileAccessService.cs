using FileAccessSystem.Data;
using System.Linq;

namespace FileAccessSystem.Services
{
    public class FileAccessService
    {
        public int CalculateRisk(int userId, int fileId, AppDbContext context)
        {
            int riskScore = 0;

            var count = context.FileAccessLogs.Count(x => x.UserId == userId);

            if (count > 20)
                riskScore += 50;
            else if (count > 10)
                riskScore += 40;
            else if (count > 5)
                riskScore += 20;

            var hour = DateTime.Now.Hour;

            if (hour < 6 || hour > 22)
                riskScore += 20;
            else if (hour < 9 || hour > 18)
                riskScore += 10;

            var file = context.Files.FirstOrDefault(f => f.Id == fileId);

            if (file != null)
            {
                if (file.Sensitivity == "High")
                    riskScore += 30;
                else if (file.Sensitivity == "Medium")
                    riskScore += 20;
                else
                    riskScore += 10;
            }
             var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                if (user.Role == "Admin")
                    riskScore -= 20;
                else if (user.Role == "Manager")
                    riskScore -= 10;
            }

            // Keep score within range
            riskScore = Math.Max(0, riskScore);

            

            return riskScore;
        }
        public string GetRiskLevel(int riskScore)
        {
            if (riskScore >= 70)
                return "High";
            else if (riskScore >= 40)
                return "Medium";
            else
                return "Low";
        }
                public string GetAIReason(int riskScore, int accessCount, string sensitivity)
        {
            // Simple rule-based reasoning 

            if (riskScore >= 70)
            {
                if (sensitivity == "High")
                    return "High risk: User frequently accessed sensitive files with unusual behavior";
                
                return "High risk: Abnormal access pattern detected with high frequency";
            }
            else if (riskScore >= 40)
            {
                return "Medium risk: Repeated access behavior observed";
            }
            else
            {
                return "Low risk: Normal user activity";
            }
        }
    }
}