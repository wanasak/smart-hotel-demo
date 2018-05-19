using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Review.Data;

namespace SmartHotel.Services.Review.Query
{
    public class ReviewQuery
    {
        private readonly ReviewDbContext _db;
        public ReviewQuery(ReviewDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Review.Data.Review>> GetByHotel(int hotelId)
        {
            return await _db.Reviews
                .Where(r => r.HotelId == hotelId)
                .OrderByDescending(r => r.Submiited)
                .ToListAsync();
        }
    }
}