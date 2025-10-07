using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Models;

public class LogiTrackContext : DbContext
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=logitrack.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-many relationship
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items);
        }
}
