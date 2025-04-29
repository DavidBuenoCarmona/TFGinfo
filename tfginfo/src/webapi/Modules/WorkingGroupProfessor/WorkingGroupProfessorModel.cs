using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class WorkingGroupProfessorModel
    {
        public int id { get; set; }
        public int professor { get; set; }
        public int working_group { get; set; }

        [JsonIgnore]
        public WorkingGroupModel workingGroupModel { get; set; }

        [JsonIgnore]
        public ProfessorModel professorModel { get; set; }
        
    }

    public class WorkingGroupProfessorConfiguration : IEntityTypeConfiguration<WorkingGroupProfessorModel>
    {
        public void Configure(EntityTypeBuilder<WorkingGroupProfessorModel> builder)
        {
            builder.HasOne(d => d.workingGroupModel)
                   .WithMany(u => u.Professors)
                   .HasForeignKey(d => d.working_group)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.professorModel)
                    .WithMany(u => u.WorkingGroups)
                    .HasForeignKey(d => d.professor)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}