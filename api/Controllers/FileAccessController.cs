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
        private readonly AIExplanationService _aiService;
        public FileAccessController(AppDbContext context, AIExplanationService aiService)
        {
    _context = context;
    _aiService = aiService;
        
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogAccess(int userId, int fileId)
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
            
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var file = _context.Files.FirstOrDefault(f => f.Id == fileId);

            string aiReason;

            if (riskLevel == "High")
            {
                aiReason = await _aiService.GetAIExplanation(
                    user?.Name ?? "Unknown User",
                    file?.Name ?? "Unknown File",
                    file?.Sensitivity ?? "Low",
                    riskScore);
            }
            else
            {
                aiReason = service.GetAIReason(
                    riskScore,
                    _context.FileAccessLogs.Count(x => x.UserId == userId),
                    file?.Sensitivity ?? "Low");
            }
            // 🔹 Step 4: Save risk log
            var riskLog = new RiskLog
            {
                UserId = userId,
                RiskScore = riskScore,
                RiskLevel = riskLevel,
                AIReason = aiReason,
                CreatedAt = DateTime.Now
            };

            _context.RiskLogs.Add(riskLog);
            _context.SaveChanges();
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
                    aiReason = r.AIReason
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
                    new User { Name = "Alice", Role = "Admin", Password = "admin123" },
                    new User { Name = "Bob", Role = "Employee", Password = "bob123" },
                    new User { Name = "John", Role = "Manager", Password = "john123"     }
                );
            }

            if (!_context.Files.Any())
            {
                _context.Files.AddRange(
                    new FileItem { Name = "Financial_Report.pdf", Sensitivity = "High", FilePath = "Uploads/Financial_Report.pdf" },

                    new FileItem { Name = "HR_Policy.docx", Sensitivity = "Medium", FilePath = "Uploads/HR_Policy.docx" },
                    new FileItem { Name = "Public_Notice.txt", Sensitivity = "Low", FilePath = "Uploads/Public_Notice.txt" }
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
        [HttpGet("open/{fileId}")]
        public IActionResult OpenFile(int fileId)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == fileId);

            if (file == null)
                return NotFound("File not found");

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                file.FilePath);

            if (!System.IO.File.Exists(fullPath))
                return NotFound("Physical file not found");

            var bytes = System.IO.File.ReadAllBytes(fullPath);

            return File(
                bytes,
                "application/octet-stream",
                file.Name);
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