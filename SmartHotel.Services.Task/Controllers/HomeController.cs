using Microsoft.AspNetCore.Mvc;

namespace SmartHotel.Services.Task.Controllers
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [HttpGet()]
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}