using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class UniversityDepartmentModel
    {
        public int id { get; set; }
        public int university { get; set; }
        public int department { get; set; }

        [JsonIgnore]
        public UniversityModel universityModel { get; set; }

        [JsonIgnore]
        public DepartmentModel departmentModel { get; set; }
    }

    public class UniversityDepartmentConfiguration : IEntityTypeConfiguration<UniversityDepartmentModel>
    {
        public void Configure(EntityTypeBuilder<UniversityDepartmentModel> builder)
        {
            builder.HasOne(d => d.universityModel)
                   .WithMany(u => u.Departments)
                   .HasForeignKey(d => d.university)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.departmentModel)
                   .WithMany(d => d.Universities)
                   .HasForeignKey(d => d.department)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}