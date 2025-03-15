using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGExternalTutorModel
    {
        public int id { get; set; }
        public int externalTutor { get; set; }
        public int tfg { get; set; }

        [JsonIgnore]
        public ExternalTutorModel externalTutorModel { get; set; }

        [JsonIgnore]
        public TFGModel tfgModel { get; set; }
        
    }

    public class TFGExternalTutorConfiguration : IEntityTypeConfiguration<TFGExternalTutorModel>
    {
        public void Configure(EntityTypeBuilder<TFGExternalTutorModel> builder)
        {
            builder.HasOne(d => d.externalTutorModel)
                   .WithMany(u => u.TFGs)
                   .HasForeignKey(d => d.externalTutor)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.tfgModel)
                    .WithMany(u => u.ExternalTutors)
                    .HasForeignKey(d => d.tfg)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}