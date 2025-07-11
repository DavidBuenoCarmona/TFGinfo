using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class DoubleCareerModel
    {
        public int id { get; set; }
        public int career { get; set; }
        public int primary_career { get; set; }
        public int secondary_career { get; set; }

        [JsonIgnore]
        public CareerModel careerModel { get; set; }
        [JsonIgnore]
        public CareerModel primaryCareerModel { get; set; }
        [JsonIgnore]
        public CareerModel secondaryCareerModel { get; set; }
    }

    public class DoubleCareerConfiguration : IEntityTypeConfiguration<DoubleCareerModel>
    {
        public void Configure(EntityTypeBuilder<DoubleCareerModel> builder)
        {
            builder.HasOne(d => d.careerModel)
                   .WithOne(c => c.DoubleCareer)
                     .HasForeignKey<DoubleCareerModel>(d => d.career)
                     .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.primaryCareerModel)
                   .WithMany()
                   .HasForeignKey(d => d.primary_career)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.secondaryCareerModel)
                   .WithMany()
                   .HasForeignKey(d => d.secondary_career)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}