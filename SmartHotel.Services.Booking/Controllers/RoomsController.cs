using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Booking.Queries;

namespace SmartHotel.Services.Booking.Controllers
{
    [Route("[controller]")]
    public class RoomsController : Controller
    {
        private readonly OccupancyQuery _occupancyQuery;
        public RoomsController(OccupancyQuery occupancyQuery)
        {
            _occupancyQuery = occupancyQuery;
        }

        [HttpGet("{idRoom}/occupancy")]
        public async Task<ActionResult> PredictRoomOcupation(int idRoom, string date)
        {
            if (DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                (double sunny, double notSunny) = await _occupancyQuery.GetRoomOcuppancy(dt, idRoom);
                return Ok(new { OcuppancyIfSunny =  sunny, OcupancyIfNotSunny = notSunny });
            }
            else
                return BadRequest("Invalid date " + date);
        }
    }
}