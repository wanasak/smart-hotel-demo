using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Hotel.Queries;

namespace SmartHotel.Services.Hotel.Controllers
{
    [Route("[controller]")]
    public class ReviewsController : Controller
    {
        private readonly HotelReviewsQuery _hotelReviewsQuery;
        public ReviewsController(HotelReviewsQuery hotelReviewsQuery)
        {
            _hotelReviewsQuery = hotelReviewsQuery;
        }

        [HttpGet("{hotelId:int}")]
        public async Task<IActionResult> Get(int hotelid)
        {
            var reviews = await _hotelReviewsQuery.Get(hotelid);

            return Ok(reviews);
        }
    }
}