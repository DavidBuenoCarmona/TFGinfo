using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string? password { get; set; }
        public string? auth_code { get; set; }
        public int role { get; set; }

        [JsonIgnore]
        public RoleModel roleModel { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasOne(d => d.roleModel)
                   .WithMany(u => u.Users)
                   .HasForeignKey(d => d.role)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}