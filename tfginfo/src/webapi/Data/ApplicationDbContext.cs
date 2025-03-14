using Microsoft.EntityFrameworkCore;
using TFGinfo.Models;

namespace TFGinfo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}


        public DbSet<UniversityModel> university { get; set; }
        public DbSet<DepartmentModel> department { get; set; }
        public DbSet<CareerModel> career { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new CareerConfiguration());
        }
    }
}
