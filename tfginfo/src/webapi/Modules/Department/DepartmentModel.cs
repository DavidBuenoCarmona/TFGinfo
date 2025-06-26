using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class DepartmentModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string acronym { get; set; }

        [JsonIgnore]
        public List<UniversityDepartmentModel> Universities { get; set; }
        public List<ProfessorModel> Professors { get; set; }
        public List<TFGLineModel> TFGLines { get; set; }
    }

    public class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentModel>
    {
        public void Configure(EntityTypeBuilder<DepartmentModel> builder)
        {
            builder.ToTable("Department");

            builder.HasKey(d => d.id);

            builder.Property(d => d.name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.acronym)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasMany(d => d.Universities)
                .WithOne(u => u.departmentModel);

            builder.HasMany(d => d.Professors)
                .WithOne(p => p.departmentModel)
                .HasForeignKey(p => p.department);

            builder.HasMany(d => d.TFGLines)
                .WithOne(t => t.departmentModel)
                .HasForeignKey(t => t.department);
        }
    }

}