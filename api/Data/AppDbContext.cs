using Microsoft.EntityFrameworkCore;
using FileAccessSystem.Models;

namespace FileAccessSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FileItem> Files { get; set; }
        public DbSet<FileAccessLog> FileAccessLogs { get; set; }
        public DbSet<RiskLog> RiskLogs { get; set; }
    }
}