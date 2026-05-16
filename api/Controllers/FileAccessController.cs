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
            var count = _context.FileAccessLogs.Count(x => x.UserId == userId);
            var file = _context.Files.FirstOrDefault(f => f.Id == fileId);

            string sensitivity = file?.Sensitivity ?? "Low";

            string aiReason = service.GetAIReason(riskScore, count, sensitivity);

            // 🔹 Step 5: Return response
            return Ok(new
            {
                message = "Access logged",
                riskScore = riskScore,
                riskLevel = riskLevel,
                aireason = aiReason
            });
        }

        [HttpGet("alerts")]
        public IActionResult GetAlerts()
        {
            var service = new FileAccessService();

            var alerts = _context.RiskLogs
                .Where(r => r.RiskLevel == "High")
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            var result = alerts.Select(r =>
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == r.UserId);

                return new
                {
                    userName = user?.Name ?? "Unknown",
                    riskScore = r.RiskScore,
                    riskLevel = r.RiskLevel,
                    createdAt = r.CreatedAt,
                    aiReason = service.GetAIReason(r.RiskScore, 0, "High")
                };
            }).ToList();
            return Ok(result);
        }
        [HttpDelete("clear")]
        public IActionResult ClearLogs()
        {
            _context.FileAccessLogs.RemoveRange(_context.FileAccessLogs);
            _context.RiskLogs.RemoveRange(_context.RiskLogs);
            _context.SaveChanges();

            return Ok(new { message = "All logs cleared" });
        }
        [HttpGet("seed")]
        public IActionResult SeedData()
        {
            if (!_context.Users.Any())
            {
                _context.Users.AddRange(
                    new User { Name = "Alice", Role = "Admin" },
                    new User { Name = "Bob", Role = "Employee" },
                    new User { Name = "John", Role = "Manager" }
                );
            }

            if (!_context.Files.Any())
            {
                _context.Files.AddRange(
                    new FileItem { Name = "Financial_Report.pdf", Sensitivity = "High" },
                    new FileItem { Name = "HR_Policy.docx", Sensitivity = "Medium" },
                    new FileItem { Name = "Public_Notice.txt", Sensitivity = "Low" }
                );
            }

            _context.SaveChanges();

            return Ok("Sample data inserted");
        }
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.Select(u => new
            {
                u.Id,
                u.Name
            }).ToList();

            return Ok(users);
        }
        [HttpGet("files")]
        public IActionResult GetFiles()
        {
            var files = _context.Files.Select(f => new
            {
                f.Id,
                f.Name,
                f.Sensitivity
            }).ToList();

            return Ok(files);
        }
        [HttpGet("activity")]
        public IActionResult GetActivityLogs()
        {
            var logs = _context.FileAccessLogs
                .OrderByDescending(l => l.AccessTime)
                .ToList();

            var result = logs.Select(log =>
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == log.UserId);

                var file = _context.Files.FirstOrDefault(f => f.Id == log.FileItemId);

                return new
                {
                    userName = user?.Name ?? "Unknown",
                    fileName = file?.Name ?? "Unknown",
                    accessTime = log.AccessTime
                };
            });

            return Ok(result);
        }
        [HttpGet("reset")]
        public IActionResult ResetData()
        {
            _context.FileAccessLogs.RemoveRange(_context.FileAccessLogs);
            _context.RiskLogs.RemoveRange(_context.RiskLogs);
            _context.Users.RemoveRange(_context.Users);
            _context.Files.RemoveRange(_context.Files);

            _context.SaveChanges();

            return Ok("All data reset");
        }
    }
}