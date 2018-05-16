using System.Threading.Tasks;
using SmartHotel.Services.Booking.Data;

namespace SmartHotel.Services.Booking.Domain
{
    public class UnitOfWork
    {
        private readonly BookingDbContext _db;
        public UnitOfWork(BookingDbContext db)
        {
            _db = db;
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}