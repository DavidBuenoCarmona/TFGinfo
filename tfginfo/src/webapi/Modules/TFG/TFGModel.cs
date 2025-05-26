using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGModel
    {
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public int tfg_line { get; set; }
        public string? external_tutor_name { get; set; }
        public string? external_tutor_email { get; set; }
        public int accepted { get; set; }
        [JsonIgnore]
        public TFGLineModel tfgLineModel { get; set; }
        public List<TFGStudentModel> Students { get; set; }
        public List<TFGProfessorModel> Professors { get; set; }
        public List<WorkingGroupTFGModel> WorkingGroups { get; set; }
    }

    public class TFGConfiguration : IEntityTypeConfiguration<TFGModel>
    {
        public void Configure(EntityTypeBuilder<TFGModel> builder)
        {
            builder.HasOne(d => d.tfgLineModel)
                   .WithMany(u => u.TFGs)
                   .HasForeignKey(d => d.tfg_line)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}