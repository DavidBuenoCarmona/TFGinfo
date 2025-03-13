using Microsoft.EntityFrameworkCore;
using TFGinfo.Models;

namespace TFGinfo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}


        public DbSet<UniversityModel> university { get; set; }
        public DbSet<DepartmentModel> department { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        }
    }
}
