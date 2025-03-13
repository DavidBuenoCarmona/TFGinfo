using Microsoft.EntityFrameworkCore;
using TFGinfo.Models;

namespace TFGinfo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        // Agrega aquí los DbSet para tus modelos
        public DbSet<UniversityModel> university { get; set; }
    }
}
