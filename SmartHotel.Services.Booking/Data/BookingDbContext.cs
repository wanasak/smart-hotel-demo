using Microsoft.EntityFrameworkCore;

namespace SmartHotel.Services.Booking.Data
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Booking.Domain.Booking> Bookings { get; set; }   

        public BookingDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking.Domain.Booking>().ToTable("Booking");

            modelBuilder.Entity<Booking.Domain.Booking>()
                .Property(b => b.ClientEmail)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Booking.Domain.Booking>()
                .Property(b => b.TotalCost)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<Booking.Domain.Booking>()
                .Property(b => b.CheckInDate)
                .HasColumnType("date");

            modelBuilder.Entity<Booking.Domain.Booking>()
                .Property(b => b.CheckOutDate)
                .HasColumnType("date");
        }    
    }
}