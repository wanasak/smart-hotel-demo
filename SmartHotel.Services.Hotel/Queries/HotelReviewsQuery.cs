using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotel.Data;

namespace SmartHotel.Services.Hotel.Queries
{
    public class HotelReview
    {
        public string User { get; set; }
        public string Room { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }

    public class HotelReviewsQuery
    {
        private readonly HotelDbContext _db;
        public HotelReviewsQuery(HotelDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelReview>> Get(int hotelId, int take = 20)
        {
            return await _db
                .Reviews
                .Where(review => review.HotelId == hotelId)
                .Take(take)
                .Select(review => new HotelReview
                {
                    User = review.Username,
                    Room = review.RoomType,
                    Message = review.Message,
                    Rating = review.Rating,
                    Date = review.Date
                }).ToListAsync();
        }
    }
}