using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class WorkingGroupStudentModel
    {
        public int id { get; set; }
        public int student { get; set; }
        public int working_group { get; set; }

        [JsonIgnore]
        public WorkingGroupModel workingGroupModel { get; set; }

        [JsonIgnore]
        public StudentModel studentModel { get; set; }
        
    }

    public class WorkingGroupStudentConfiguration : IEntityTypeConfiguration<WorkingGroupStudentModel>
    {
        public void Configure(EntityTypeBuilder<WorkingGroupStudentModel> builder)
        {
            builder.HasOne(d => d.workingGroupModel)
                   .WithMany(u => u.Students)
                   .HasForeignKey(d => d.working_group)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.studentModel)
                    .WithMany(u => u.WorkingGroups)
                    .HasForeignKey(d => d.student)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}