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
        public DbSet<TFGLineModel> tfg_line { get; set; }
        public DbSet<TFGModel> tfg { get; set; }
        public DbSet<WorkingGroupModel> working_group { get; set; }
        public DbSet<TFGProfessorModel> tfg_professor { get; set; }
        public DbSet<TFGLineCareerModel> tfg_line_career { get; set; }
        public DbSet<TFGStudentModel> tfg_student { get; set; }
        public DbSet<WorkingGroupStudentModel> working_group_student { get; set; }
        public DbSet<WorkingGroupProfessorModel> working_group_professor { get; set; }
        public DbSet<WorkingGroupTFGModel> working_group_tfg { get; set; }
        public DbSet<TFGLineProfessorModel> tfg_line_professor { get; set; }
        public DbSet<UniversityDepartmentModel> university_department { get; set; }
        public DbSet<DoubleCareerModel> double_career { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new CareerConfiguration());
            modelBuilder.ApplyConfiguration(new ProfessorConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new TFGLineConfiguration());
            modelBuilder.ApplyConfiguration(new TFGConfiguration());
            modelBuilder.ApplyConfiguration(new TFGProfessorConfiguration());
            modelBuilder.ApplyConfiguration(new TFGLineCareerConfiguration());
            modelBuilder.ApplyConfiguration(new TFGStudentConfiguration());
            modelBuilder.ApplyConfiguration(new WorkingGroupStudentConfiguration());
            modelBuilder.ApplyConfiguration(new WorkingGroupProfessorConfiguration());
            modelBuilder.ApplyConfiguration(new WorkingGroupTFGConfiguration());
            modelBuilder.ApplyConfiguration(new TFGLineProfessorConfiguration());
            modelBuilder.ApplyConfiguration(new UniversityDepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DoubleCareerConfiguration());
        }
    }
}
