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
                else
                    riskScore += 10;
            }

            return riskScore;
        }
    }
}