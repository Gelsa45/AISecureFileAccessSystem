using Microsoft.AspNetCore.Mvc;
using FileAccessSystem.Data;
using FileAccessSystem.Models;

namespace FileAccessSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Name == request.Username &&
                u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(user);
        }
    }
}