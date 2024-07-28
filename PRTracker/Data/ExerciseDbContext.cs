
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
        public DbSet<User> Users { get; set; }
        public DbSet<UserLift> UserLifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLift>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.UserLifts)
                .HasForeignKey(ul => ul.UserId);

            modelBuilder.Entity<UserLift>()
                .HasOne(ul => ul.Exercise)
                .WithMany(e => e.UserLifts)
                .HasForeignKey(ul => ul.ExerciseId);
        }
    }
}
