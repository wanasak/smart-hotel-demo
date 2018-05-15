using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Hotel.Queries;

namespace SmartHotel.Services.Hotel.Controllers
{
    [Route("[controller]")]
    public class FeaturedController : Controller
    {
        private readonly FeaturedItemsHotelsQuery _featuredQuery;

        public FeaturedController(
            FeaturedItemsHotelsQuery featuredQuery
            )
        {
            _featuredQuery = featuredQuery;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var userId = (string)null;
            if (User.Identity.IsAuthenticated)
                userId = User.Claims.First(c => c.Type == "emails").Value;

            var hotels = userId != null ? 
                await _featuredQuery.GetForuser(userId) :
                await _featuredQuery.Get();

            return Ok(hotels);
        }
    }
}