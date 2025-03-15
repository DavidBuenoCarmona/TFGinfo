using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGStudentModel
    {
        public int id { get; set; }
        public int student { get; set; }
        public int tfg { get; set; }

        [JsonIgnore]
        public StudentModel studentModel { get; set; }

        [JsonIgnore]
        public TFGModel tfgModel { get; set; }
        
    }

    public class TFGStudentConfiguration : IEntityTypeConfiguration<TFGStudentModel>
    {
        public void Configure(EntityTypeBuilder<TFGStudentModel> builder)
        {
            builder.HasOne(d => d.studentModel)
                   .WithMany(u => u.TFGs)
                   .HasForeignKey(d => d.student)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.tfgModel)
                    .WithMany(u => u.Students)
                    .HasForeignKey(d => d.tfg)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}