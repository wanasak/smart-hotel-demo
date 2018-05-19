using Microsoft.AspNetCore.Mvc;

namespace SmartHotel.Services.Review.Controllers
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [HttpGet()]
        public ActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}