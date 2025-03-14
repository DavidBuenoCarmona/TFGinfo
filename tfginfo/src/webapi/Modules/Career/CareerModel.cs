using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class CareerModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int university { get; set; }

        [JsonIgnore]
        public UniversityModel universityModel { get; set; }
    }

    public class CareerConfiguration : IEntityTypeConfiguration<CareerModel>
    {
        public void Configure(EntityTypeBuilder<CareerModel> builder)
        {
            builder.HasOne(d => d.universityModel)
                   .WithMany(u => u.Careers)
                   .HasForeignKey(d => d.university)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}