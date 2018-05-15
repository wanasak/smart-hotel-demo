using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotel.Data;

namespace SmartHotel.Services.Hotel.Queries
{
    public class FeaturedItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
        public int Price { get; set; }
        public string Picture { get; set; }
    }

    public class FeaturedItemsHotelsQuery
    {
        private readonly HotelDbContext _db;
        public FeaturedItemsHotelsQuery(HotelDbContext db)
        {
            _db = db;
        }

        public Task<IEnumerable<FeaturedItem>> GetForuser(string userId)
        {
            return Get();
        }

        public async Task<IEnumerable<FeaturedItem>> Get()
        {
            var hotels = await _db.Hotels
                .OrderByDescending(h => h.Visits)
                .Include(h => h.ConferenceRooms)
                .Include(h => h.City)
                .Include(h => h.RoomTypes)
                .Take(2)
                .ToListAsync();

            var featuredHotels = hotels.Select(h => new FeaturedItem
            {
                Id = h.Id,
                ItemType = "hotel",
                Name = h.Name,
                City = "",
                Price = h.StarterPricePerNight,
                Picture = $"pichotels/{h.Id}_featured.png"
            }).ToList();

            var featuredConferenceRooms = hotels.Select(h => new FeaturedItem
            {
                Id = h.ConferenceRooms.First().Id,
                ItemType = "conferenceRoom",
                Name = h.ConferenceRooms.First().Name,
                Rating = h.ConferenceRooms.First().Rating,
                City = $"{h.City.Name} {h.City.Country}",
                Price = h.ConferenceRooms.First().PricePerHour
            }).ToList();

            featuredConferenceRooms.ForEach(fc => fc.Picture = $"picconf/{fc.Id}_featured.png");

            return new List<FeaturedItem>
            {
                featuredHotels[0],
                featuredConferenceRooms[0],
                featuredHotels[1],
                featuredConferenceRooms[1]  
            };
        }
    }
}