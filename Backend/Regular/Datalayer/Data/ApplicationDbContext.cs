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
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<EmployeeInvites> EmployeeInvites { get; set; }
        public DbSet<Organization_Project> Organization_Project { get; set; }
        public DbSet<User_Project> User_Project { get; set; }
    }
}
