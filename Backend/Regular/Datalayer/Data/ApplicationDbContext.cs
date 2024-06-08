using Datalayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Users_Users> Users_Users { get; set; }
        public DbSet<Organizations_Users> Organizations_Users { get; set; }
        public DbSet<User_Project> User_Project { get; set; }
        public DbSet<LoginsLog> LoginsLog { get; set; }
    }
}
