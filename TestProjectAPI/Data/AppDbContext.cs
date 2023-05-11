using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestProjectAPI.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Booking> Bookings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.BookingDate).IsRequired();
                entity.Property(e => e.Flexibility).IsRequired();
                entity.Property(e => e.VehicleSize).IsRequired();
                entity.Property(e => e.ContactNumber).HasMaxLength(20);
                entity.Property(e => e.EmailAddress).HasMaxLength(100);
                entity.Property(e => e.IsApproved).IsRequired();
            });
        }
    }
}
