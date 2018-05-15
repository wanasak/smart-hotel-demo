using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotel.Domain.Hotel;
using SmartHotel.Services.Hotel.Domain.Review;

namespace SmartHotel.Services.Hotel.Data
{
    public class HotelDbContext : DbContext
    { 
        public HotelDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<HotelService> HotelServices { get; set; }
        public DbSet<RoomService> RoomServices { get; set; }
        public DbSet<Domain.Hotel.Hotel> Hotels { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Hotel.Hotel>()
                .Property(h => h.Id)
                .ForSqlServerUseSequenceHiLo("hotelseq");
            modelBuilder.Entity<Domain.Hotel.Hotel>()
                .OwnsOne(h => h.Address);
            modelBuilder.Entity<Domain.Hotel.Hotel>()
                .OwnsOne(h => h.Location);
            modelBuilder.Entity<Domain.Hotel.Hotel>()
                .Property(h => h.CheckinTime).HasColumnType("time");
            modelBuilder.Entity<Domain.Hotel.Hotel>()
                .Property(h => h.CheckoutTime).HasColumnType("time");

            modelBuilder.Entity<HotelService>()
                .Property(s => s.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<HotelService>()
                .Property(s => s.Name)
                .HasMaxLength(32);

            modelBuilder.Entity<ServicePerHotel>()
                .HasKey(sph => new { sph.HotelId, sph.ServiceId });

            modelBuilder.Entity<RoomService>()
                .Property(s => s.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<RoomService>()
                .Property(s => s.Name)
                .HasMaxLength(32);

            modelBuilder.Entity<ServicePerRoom>()
                .HasKey(spr => new { spr.RoomTypeId, spr.ServiceId });

            modelBuilder.Entity<City>()
                .Property(c => c.Id)
                .ValueGeneratedNever();
        }
    }
}