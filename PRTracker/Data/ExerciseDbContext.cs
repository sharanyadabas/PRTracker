
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRTracker.Entities;

namespace PRTracker.Data
{
    public class ExerciseDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ExerciseDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<Exercise> Exercises { get; set; }
    }
}
