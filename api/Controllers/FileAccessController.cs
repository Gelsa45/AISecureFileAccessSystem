using Microsoft.AspNetCore.Mvc;
using FileAccessSystem.Data;
using FileAccessSystem.Models;
using FileAccessSystem.Services;

namespace FileAccessSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileAccessController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FileAccessController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("log")]
        public IActionResult LogAccess(int userId, int fileId)
        {
            // 🔹 Step 1: Save access log
            var log = new FileAccessLog
            {
                UserId = userId,
                FileItemId = fileId,
                AccessTime = DateTime.Now
            };

            _context.FileAccessLogs.Add(log);
            _context.SaveChanges();

            // 🔹 Step 2: Call service for risk logic
            var service = new FileAccessService();
            int riskScore = service.CalculateRisk(userId, fileId, _context);

            // 🔹 Step 3: Risk level (still here for now)
            string riskLevel = service.GetRiskLevel(riskScore);
            // 🔹 Step 4: Save risk log
            var riskLog = new RiskLog
            {
                UserId = userId,
                RiskScore = riskScore,
                RiskLevel = riskLevel,
                CreatedAt = DateTime.Now
            };

            _context.RiskLogs.Add(riskLog);
            _context.SaveChanges();

            // 🔹 Step 5: Return response
            return Ok(new
            {
                message = "Access logged",
                riskScore = riskScore,
                riskLevel = riskLevel
            });
        }

        [HttpGet("alerts")]
        public IActionResult GetAlerts()
        {
            var alerts = _context.RiskLogs
                .Where(r => r.RiskLevel == "High")
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return Ok(alerts);
        }
    }
}