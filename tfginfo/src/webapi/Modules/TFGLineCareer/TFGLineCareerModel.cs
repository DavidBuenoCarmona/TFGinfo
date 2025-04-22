using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGLineCareerModel
    {
        public int id { get; set; }
        public int career { get; set; }
        public int tfg_line { get; set; }

        [JsonIgnore]
        public TFGLineModel tfgLineModel { get; set; }

        [JsonIgnore]
        public CareerModel careerModel { get; set; }
        
    }

    public class TFGLineCareerConfiguration : IEntityTypeConfiguration<TFGLineCareerModel>
    {
        public void Configure(EntityTypeBuilder<TFGLineCareerModel> builder)
        {
            builder.HasOne(d => d.tfgLineModel)
                   .WithMany(u => u.Careers)
                   .HasForeignKey(d => d.tfg_line)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.careerModel)
                    .WithMany(u => u.TFGLines)
                    .HasForeignKey(d => d.career)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}