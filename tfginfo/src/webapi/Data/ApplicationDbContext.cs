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
        public DbSet<ProfessorModel> professor { get; set; }
        public DbSet<RoleModel> role  { get; set; }
        public DbSet<UserModel> user { get; set; }
        public DbSet<StudentModel> student { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new CareerConfiguration());
            modelBuilder.ApplyConfiguration(new ProfessorConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
        }
    }
}
