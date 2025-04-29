using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class WorkingGroupTFGModel
    {
        public int id { get; set; }
        public int tfg { get; set; }
        public int working_group { get; set; }

        [JsonIgnore]
        public WorkingGroupModel workingGroupModel { get; set; }

        [JsonIgnore]
        public TFGModel tfgModel { get; set; }
        
    }

    public class WorkingGroupTFGConfiguration : IEntityTypeConfiguration<WorkingGroupTFGModel>
    {
        public void Configure(EntityTypeBuilder<WorkingGroupTFGModel> builder)
        {
            builder.HasOne(d => d.workingGroupModel)
                   .WithMany(u => u.TFGs)
                   .HasForeignKey(d => d.working_group)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.tfgModel)
                    .WithMany(u => u.WorkingGroups)
                    .HasForeignKey(d => d.tfg)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}