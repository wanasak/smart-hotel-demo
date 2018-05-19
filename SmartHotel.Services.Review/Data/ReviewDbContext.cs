using Microsoft.EntityFrameworkCore;

namespace SmartHotel.Services.Review.Data
{
    public class ReviewDbContext : DbContext
    {
        public DbSet<Review> Reviews { get; set; }

        public ReviewDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().ToTable("review");
            modelBuilder.Entity<Review>().Ignore(r => r.FormattedDate);
            modelBuilder.Entity<Review>().Property(r => r.Description).HasMaxLength(1024);
        }
    }
}