using Microsoft.EntityFrameworkCore;
using EventEase.Models;

namespace EventEase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    public DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EventType>().HasData(
            new EventType { EventTypeId = 1, TypeName = "Conference", Description = "Business conferences" },
            new EventType { EventTypeId = 2, TypeName = "Wedding", Description = "Wedding ceremonies" },
            new EventType { EventTypeId = 3, TypeName = "Party", Description = "Celebrations, birthdays" },
            new EventType { EventTypeId = 4, TypeName = "Meeting", Description = "Corporate meetings" },
            new EventType { EventTypeId = 5, TypeName = "Concert", Description = "Live music events" }
        );

            // Prevent deleting venue if it has events
            modelBuilder.Entity<Venue>()
                .HasMany(v => v.Events)
                .WithOne(e => e.Venue)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent deleting event if it has booking
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Booking)
                .WithOne(b => b.Event)
                .HasForeignKey<Booking>(b => b.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
