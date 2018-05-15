using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotel.Data;

namespace SmartHotel.Services.Hotel.Queries
{
    public class ConferenceRoomSearchFilter
    {
        public int? CityId { get; set; }
        public int? Rating { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? Guests { get; set; }
    }

    public class ConferenceRoomSearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
        public int PricePerHour { get; set; }
        public int Capacity { get; set; }
        public string Picture { get; set; }
        public int NumPhotos { get; set; }
    }


    public class ConferenceRoomSearchQuery
    {
        private readonly HotelDbContext _db;
        public ConferenceRoomSearchQuery(HotelDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ConferenceRoomSearchResult>> Get(ConferenceRoomSearchFilter filter)
        {
            var query = _db.Hotels
                .OrderByDescending(h => h.Visits)
                .Include(h => h.ConferenceRooms)
                .Include(h => h.Location)
                .Include(h => h.City)
                .Include(h => h.Address)
                .AsQueryable();

            if (filter.CityId.HasValue)
                query = query.Where(h => h.City.Id == filter.CityId);

            var conferencesQuery = query.SelectMany(h => h.ConferenceRooms.Select(cfr => new ConferenceRoomSearchResult
            {
                Id = cfr.Id,
                Name = cfr.Name,
                HotelId = h.Id,
                HotelName = h.Name,
                Rating = cfr.Rating,
                City = $"{h.City.Name} {h.City.Country}",
                PricePerHour = cfr.PricePerHour,
                Capacity = cfr.Capacity,
                NumPhotos = cfr.NumPhotos
            }));

            if (filter.Rating.HasValue)
                query = query.Where(cfr => cfr.Rating >= filter.Rating);

            if (filter.MinPrice.HasValue)
                conferencesQuery = conferencesQuery.Where(cfr => cfr.PricePerHour >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                conferencesQuery = conferencesQuery.Where(cfr => cfr.PricePerHour <= filter.MaxPrice);

            if (filter.Guests.HasValue)
                conferencesQuery = conferencesQuery.Where(cfr => filter.Guests <= cfr.Capacity);

            var result = await conferencesQuery.ToListAsync();

            result.ForEach(cfr => cfr.Picture = cfr.NumPhotos > 0 ? $"picconf/{cfr.Id}_1.png" : "");

            return result;
        }
    }
}