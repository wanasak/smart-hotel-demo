using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Review.Query;

namespace SmartHotel.Services.Review.Controllers
{
    [Route("[controller]")]
    public class ReviewsController : Controller
    {
        private readonly FormatDateService formatDateService;
        private readonly ReviewQuery query;
        public ReviewsController(ReviewQuery query, FormatDateService fds)
        {
            this.query = query;
            this.formatDateService = fds;
        }

        [HttpGet("hotel/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var reviews = await this.query.GetByHotel(id);

            foreach (var review in reviews)
            {
                review.FormattedDate = this.formatDateService.FormatAsString(review.Submiited);
            }

            return Ok(reviews);
        }   
    }
}