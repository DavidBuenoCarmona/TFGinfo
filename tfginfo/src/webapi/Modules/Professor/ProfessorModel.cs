using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class ProfessorModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public int department_boss { get; set; }
        public int department { get; set; }
        public int user { get; set; }

        [JsonIgnore]
        public DepartmentModel departmentModel { get; set; }
    }

    public class ProfessorConfiguration : IEntityTypeConfiguration<ProfessorModel>
    {
        public void Configure(EntityTypeBuilder<ProfessorModel> builder)
        {
            builder.HasOne(p => p.departmentModel)
                   .WithMany(u => u.Professors)
                   .HasForeignKey(p => p.department)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}