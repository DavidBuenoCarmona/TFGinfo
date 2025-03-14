using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class DepartmentModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int university { get; set; }

        [JsonIgnore]
        public UniversityModel universityModel { get; set; }
        public List<ProfessorModel> Professors { get; set; }
        public List<TFGLineModel> TFGLines { get; set; }
    }

    public class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentModel>
    {
        public void Configure(EntityTypeBuilder<DepartmentModel> builder)
        {
            builder.HasOne(d => d.universityModel)
                   .WithMany(u => u.Departments)
                   .HasForeignKey(d => d.university)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}