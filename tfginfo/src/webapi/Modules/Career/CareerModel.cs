using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class CareerModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? university { get; set; }
        public int double_career { get; set; }

        [JsonIgnore]
        public UniversityModel? universityModel { get; set; }
        public DoubleCareerModel? DoubleCareer { get; set; }
        public List<StudentModel> Students { get; set; }
        public List<TFGLineCareerModel> TFGLines { get; set; }
    }

    public class CareerConfiguration : IEntityTypeConfiguration<CareerModel>
    {
        public void Configure(EntityTypeBuilder<CareerModel> builder)
        {
            builder.HasKey(c => c.id);
            builder.Property(c => c.name).IsRequired();
            builder.Property(c => c.university).IsRequired(false);
            builder.Property(c => c.double_career).IsRequired();

            builder.HasOne(c => c.universityModel)
                .WithMany(u => u.Careers)
                .HasForeignKey(c => c.university)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.DoubleCareer)
                .WithMany()
                .HasForeignKey(c => c.double_career)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}