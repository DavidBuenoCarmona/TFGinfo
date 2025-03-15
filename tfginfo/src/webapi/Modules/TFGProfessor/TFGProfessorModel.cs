using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGProfessorModel
    {
        public int id { get; set; }
        public int professor { get; set; }
        public int tfg { get; set; }

        [JsonIgnore]
        public ProfessorModel professorModel { get; set; }

        [JsonIgnore]
        public TFGModel tfgModel { get; set; }
        
    }

    public class TFGProfessorConfiguration : IEntityTypeConfiguration<TFGProfessorModel>
    {
        public void Configure(EntityTypeBuilder<TFGProfessorModel> builder)
        {
            builder.HasOne(d => d.professorModel)
                   .WithMany(u => u.TFGs)
                   .HasForeignKey(d => d.professor)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.tfgModel)
                    .WithMany(u => u.Professors)
                    .HasForeignKey(d => d.tfg)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}