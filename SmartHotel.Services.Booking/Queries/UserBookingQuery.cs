using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Booking.Data;

namespace SmartHotel.Services.Booking.Queries
{
    public class UserBooking
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public byte Adults { get; set; }
        public byte Babies { get; set; }
        public byte Kids { get; set; }
        public decimal Price { get; set; }
    }

    public class UserBookingQuery
    {
        private readonly BookingDbContext _db;
        public UserBookingQuery(BookingDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserBooking>> GetAll(string userId)
        {
            return await _db
                .Bookings
                .Where(b => b.ClientEmail == userId)
                .OrderByDescending(b => b.CheckOutDate)
                .Take(100)
                .Select(booking => new UserBooking
                {
                    Id = booking.Id,
                    HotelId = booking.IdHotel,
                    From = booking.CheckInDate,
                    To = booking.CheckOutDate,
                    Adults = booking.NumberOfAdults,
                    Babies = booking.NumberOfBabies,
                    Kids = booking.NumberOfChildren,
                    Price = booking.TotalCost
                })
                .ToListAsync();
        }

        public async Task<UserBooking> GetLatest(string userId)
        {
            return await _db
                .Bookings
                .Where(b => b.ClientEmail == userId)
                .OrderByDescending(b => b.CheckOutDate)
                .Select(booking => new UserBooking
                {
                    Id = booking.Id,
                    HotelId = booking.IdHotel,
                    From = booking.CheckInDate,
                    To = booking.CheckOutDate,
                    Adults = booking.NumberOfAdults,
                    Babies = booking.NumberOfBabies,
                    Kids = booking.NumberOfChildren
                })
                .FirstOrDefaultAsync();
        }
    }
}