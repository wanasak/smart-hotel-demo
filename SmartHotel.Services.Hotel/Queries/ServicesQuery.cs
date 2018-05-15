using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotel.Data;
using SmartHotel.Services.Hotel.Domain.Hotel;

namespace SmartHotel.Services.Hotel.Queries
{
    public class ServicesQuery
    {
        private readonly HotelDbContext _db;
        public ServicesQuery(HotelDbContext db) => _db = db;

        public async Task<IEnumerable<HotelService>> GetAllHotelServices()
        {
            return await _db.HotelServices.OrderBy(s => s.Id).ToListAsync();
        }

        public async Task<IEnumerable<RoomService>> GetAllRoomServices()
        {
            return await _db.RoomServices.OrderBy(s => s.Id).ToListAsync();
        }
    }
}