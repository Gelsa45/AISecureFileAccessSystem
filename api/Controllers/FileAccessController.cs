using Microsoft.AspNetCore.Mvc;
using FileAccessSystem.Data;
using FileAccessSystem.Models;

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
    var log = new FileAccessLog
    {
        UserId = userId,
        FileItemId = fileId,
        AccessTime = DateTime.Now
    };

    _context.FileAccessLogs.Add(log);
    _context.SaveChanges();

    // 🔥 RISK LOGIC START

    int riskScore = 0;

    // 1. Frequency check
    var count = _context.FileAccessLogs
        .Count(x => x.UserId == userId);

    if (count > 20)
        riskScore += 50;
    else if (count > 10)
        riskScore += 40;
    else if (count > 5)
        riskScore += 20;

    // 2. Time check (FIXED)
    var hour = DateTime.Now.Hour;

    if (hour < 6 || hour > 22)
        riskScore += 20;
    else if (hour < 9 || hour > 18)
        riskScore += 10;

    // 3. File sensitivity (FIXED)
    var file = _context.Files.FirstOrDefault(f => f.Id == fileId);

    if (file != null)
    {
        if (file.Sensitivity == "High")
            riskScore += 30;
        else
            riskScore += 10;
    }

    // Risk level
    string riskLevel = "Low";

    if (riskScore >= 70)
        riskLevel = "High";
    else if (riskScore >= 40)
        riskLevel = "Medium";

    // 🔥 SAVE RISK
    var riskLog = new RiskLog
    {
        UserId = userId,
        RiskScore = riskScore,
        RiskLevel = riskLevel,
        CreatedAt = DateTime.Now
    };

    _context.RiskLogs.Add(riskLog);
    _context.SaveChanges();

    return Ok(new
    {
        message = "Access logged",
        totalAccess = count,
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
}}