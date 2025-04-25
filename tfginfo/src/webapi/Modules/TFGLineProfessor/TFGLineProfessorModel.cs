using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGLineProfessorModel
    {
        public int id { get; set; }
        public int professor { get; set; }
        public int tfg_line { get; set; }

        [JsonIgnore]
        public TFGLineModel tfgLineModel { get; set; }

        [JsonIgnore]
        public ProfessorModel professorModel { get; set; }
        
    }

    public class TFGLineProfessorConfiguration : IEntityTypeConfiguration<TFGLineProfessorModel>
    {
        public void Configure(EntityTypeBuilder<TFGLineProfessorModel> builder)
        {
            builder.HasOne(d => d.tfgLineModel)
                   .WithMany(u => u.Professors)
                   .HasForeignKey(d => d.tfg_line)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.professorModel)
                    .WithMany(u => u.TFGLines)
                    .HasForeignKey(d => d.professor)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}