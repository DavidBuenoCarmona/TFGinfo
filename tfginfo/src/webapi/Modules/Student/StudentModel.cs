using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class StudentModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string dni { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public int career { get; set; }
        public int user { get; set; }
        public DateTime? birthdate { get; set; }

        [JsonIgnore]
        public CareerModel careerModel { get; set; }

        public List<TFGStudentModel> TFGs { get; set; }
    }

    public class StudentConfiguration : IEntityTypeConfiguration<StudentModel>
    {
        public void Configure(EntityTypeBuilder<StudentModel> builder)
        {
            builder.HasOne(p => p.careerModel)
                   .WithMany(u => u.Students)
                   .HasForeignKey(p => p.career)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}