using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGModel
    {
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public int tfgLine { get; set; }

        [JsonIgnore]
        public TFGLineModel tfgLineModel { get; set; }
        public List<TFGStudentModel> Students { get; set; }
        public List<TFGProfessorModel> Professors { get; set; }
        public List<TFGExternalTutorModel> ExternalTutors { get; set; }
    }

    public class TFGConfiguration : IEntityTypeConfiguration<TFGModel>
    {
        public void Configure(EntityTypeBuilder<TFGModel> builder)
        {
            builder.HasOne(d => d.tfgLineModel)
                   .WithMany(u => u.TFGs)
                   .HasForeignKey(d => d.tfgLine)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}