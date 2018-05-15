using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Hotel.Queries;

namespace SmartHotel.Services.Hotel.Controllers
{
    [Route("[controller]")]
    public class CitiesController : Controller
    {
        private readonly CitiesQuery _citiesQuery;
        public CitiesController(CitiesQuery citiesQuery)
        {
            _citiesQuery = citiesQuery;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name = "")
        {
            var cities = string.IsNullOrEmpty(name) ?
                await _citiesQuery.GetDefaultCities() :
                await _citiesQuery.Get(name);

            return Ok(cities);
        }
    }
}