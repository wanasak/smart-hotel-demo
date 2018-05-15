using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotel.Data;
using SmartHotel.Services.Hotel.Domain.Hotel;

namespace SmartHotel.Services.Hotel.Queries
{
    public class HotelDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int PricePerNight { get; set; }
        public string DefaultPicture { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public RoomSummary[] Rooms { get; set; }
        public IEnumerable<HotelService> Services { get; set; }
        public int NumPhotos { get; set; }
    }

    public class RoomSummary
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal OriginalRoomPrice { get; set; }
        public decimal LocalRoomPrice { get; set; }
        public decimal LocalOriginalRoomPrice { get; set; }
        public string BadgeSymbol { get; set; }
    }

    public class RoomDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string DefaultPicture { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public IEnumerable<RoomService> Services { get; set; }
        public int TwinBeds { get; set; }
        public int DoubleBeds { get; set; }
        public int SingleBeds { get; set; }
        public int Numphotos { get; set; }
    }

    public class HotelDetailQuery
    {
        private readonly HotelDbContext _db;
        public HotelDetailQuery(HotelDbContext db)
        {
            _db = db;
        }

        public async Task<HotelDetail> Get(int hotelId, double discount)
        {
            var hotel = await _db.Hotels
                .Include(h => h.RoomTypes)
                .Include(h => h.Services).ThenInclude(s => s.Service)
                .Include(h => h.City)
                .SingleOrDefaultAsync(h => h.Id == hotelId);

            var detail = new HotelDetail
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                Rating = hotel.Rating,
                City = $"{hotel.City.Name} {hotel.City.Country}",
                Street = hotel.Address.Street,
                Latitude = hotel.Location.Latitude,
                Longitude = hotel.Location.Longtitude,
                PricePerNight = hotel.StarterPricePerNight,
                CheckInTime = hotel.CheckinTime,
                CheckOutTime = hotel.CheckoutTime,
                NumPhotos = hotel.NumPhotos,
                DefaultPicture = $"pichotels/{hotel.Id}_default.png",
                Pictures = Enumerable.Range(1, hotel.NumPhotos)
                    .Select(idx => $"pichotels/{hotel.Id}_{idx}.png"),
                Services = hotel.Services.Select(sph => sph.Service),
                Rooms = hotel.RoomTypes.Select(room => new RoomSummary
                {
                    RoomName = room.Name,
                    RoomId = room.Id,
                    RoomPrice = room.Price - (room.Price * (decimal)discount),
                    DiscountApplied = (decimal)discount,
                    OriginalRoomPrice = room.Price
                }).ToArray()
            };

            return detail;
        }

        public async Task<IEnumerable<RoomDetail>> GetRoomsByHotel(int hotelId)
        {
            var hotel = await _db.Hotels
                .Include(h => h.RoomTypes)
                    .ThenInclude(r => r.Services)
                        .ThenInclude(s => s.Service)
                .SingleOrDefaultAsync(h => h.Id == hotelId);

            if (hotel == null) return null;

            var rooms = hotel.RoomTypes.Select(room => new RoomDetail
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                Capacity = room.Capacity,
                DefaultPicture = $"picrooms/{hotel.Id}_default.png",
                Services = room.Services.Select(s => s.Service),
                DoubleBeds = room.DoubleBeds,
                TwinBeds = room.TwinBeds,
                SingleBeds = room.SingleBeds,
                Numphotos = room.NumPhotos
            }).ToList();

            rooms.ForEach(r => r.Pictures = Enumerable.Range(1, r.Numphotos)
                .Select(idx => $"picrooms/{r.Id}_{idx}.png"));

            return rooms;
        }
    }
}