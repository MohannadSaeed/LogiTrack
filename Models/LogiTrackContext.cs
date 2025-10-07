using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Models;

public class LogiTrackContext : DbContext
{
    public LogiTrackContext(DbContextOptions<LogiTrackContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId);
        }
}
