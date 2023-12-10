using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<Coffee> Coffee { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<BuyHistory> BuyHistories { get; set; }
    }
}
