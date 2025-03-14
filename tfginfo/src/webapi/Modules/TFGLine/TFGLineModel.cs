using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class TFGLineModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int slots { get; set; }
        public int group { get; set; }
        public int department { get; set; }

        [JsonIgnore]
        public DepartmentModel departmentModel { get; set; }
    }

    public class TFGLineConfiguration : IEntityTypeConfiguration<TFGLineModel>
    {
        public void Configure(EntityTypeBuilder<TFGLineModel> builder)
        {
            builder.HasOne(d => d.departmentModel)
                   .WithMany(u => u.TFGLines)
                   .HasForeignKey(d => d.department)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}