using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartHotel.Services.Hotel.Queries;
using SmartHotel.Services.Hotel.Services;
using SmartHotel.Services.Hotel.Settings;

namespace SmartHotel.Services.Hotel.Controllers
{
    [Route("[controller]")]
    public class HotelsController : Controller
    {
        private readonly HotelsSearchQuery _hotelsSearchQuery;
        private readonly HotelDetailQuery _hotelDetailQuery;
        private readonly DiscountService _discountService;
        private readonly CurrencySettings _currencyConf;

        public HotelsController(
            HotelsSearchQuery hotelsSearchQuery,
            HotelDetailQuery hotelDetailQuery,
            DiscountService discountService,
            IOptions<CurrencySettings> currencyConf
            )
        {
            _hotelsSearchQuery = hotelsSearchQuery;
            _hotelDetailQuery = hotelDetailQuery;
            _discountService = discountService;
            _currencyConf = currencyConf.Value;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(int? cityId, int? rating, int? minPrice, int? maxPrice)
        {
            var filter = new HotelSearchFilter
            {
                CityId = cityId,
                Rating = rating,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var hotels = await _hotelsSearchQuery.Get(filter);

            return Ok(hotels);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, string user)
        {
            var userId = User.Claims.SingleOrDefault(c => c.Type == "emails")?.Value;
            
            if (!string.IsNullOrEmpty(user))
                userId = user;

            var discount = 0.0d;
            if (!string.IsNullOrEmpty(userId))
                discount = await _discountService.GetDiscountByCustomer(userId);

            var hotel = await _hotelDetailQuery.Get(id, discount);

            if (hotel == null) return NotFound();

            foreach (var roomSummary in hotel.Rooms)
            {
                roomSummary.BadgeSymbol = _currencyConf.Badge;
                roomSummary.LocalOriginalRoomPrice = roomSummary.OriginalRoomPrice * _currencyConf.BaseConversion;
                roomSummary.LocalRoomPrice = roomSummary.RoomPrice * _currencyConf.BaseConversion;
            }   

            return Ok(hotel);
        }

        [HttpGet("{hotel:int}/rooms")]
        public async Task<IActionResult> GetRoomsByHotel(int hotelId)
        {
            var rooms = await _hotelDetailQuery.GetRoomsByHotel(hotelId);

            if (rooms == null) return NotFound($"Hotel {hotelId} could not be found");

            return Ok(rooms);
        }
    }
}