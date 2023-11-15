using Microsoft.EntityFrameworkCore;

namespace GeoExplorerApi.Models
{
    public class GeoExplorerContext : DbContext
    {
        public GeoExplorerContext(DbContextOptions<GeoExplorerContext> options) : base(options)
        { }

        public DbSet<State> States { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, constraints, etc.
            modelBuilder.Entity<Location>()
                .HasOne(l => l.State)
                .WithMany(s => s.Locations)
                .HasForeignKey(l => l.StateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data
            modelBuilder.Entity<State>().HasData(
                new State { Id = 1, Name = "New York" }
                // ... add other states
            );
        }
    }
}