using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Hotel.Queries;

namespace SmartHotel.Services.Hotel.Controllers
{
    [Route("[controller]")]
    public class ServicesController : Controller
    {
        private readonly ServicesQuery _servicesQuery;
        public ServicesController(ServicesQuery servicesQuery)
        {
            _servicesQuery = servicesQuery;
        }

        [HttpGet("hotel")]
        public async Task<IActionResult> GetHotelServices()
        {
            var data = await _servicesQuery.GetAllHotelServices();

            return Ok(data);
        }

        [HttpGet("room")]
        public async Task<IActionResult> GetRoomServices()
        {
            var data = await _servicesQuery.GetAllRoomServices();

            return Ok(data);
        }
    }
}