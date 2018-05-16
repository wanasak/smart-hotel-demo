namespace SmartHotel.Services.Booking.Data.Repositories
{
    public class BookingRepository
    {
        private readonly BookingDbContext _db;
        public BookingRepository(BookingDbContext db)
        {
            _db = db;
        }

        public void Add(Booking.Domain.Booking booking)
        {
            _db.Bookings.Add(booking);
        }
    }
}