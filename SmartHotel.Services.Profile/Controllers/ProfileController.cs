using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Profile.Data;

namespace SmartHotel.Services.Profile.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ProfileDbContext context;
        public ProfileController(ProfileDbContext context)
        {
            this.context = context;
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetAction(string id)
        {
            var profile = this.context.Profiles.SingleOrDefault(p => p.UserId == id);
            if (profile == null)
                profile = this.context.Profiles.FirstOrDefault(p => p.Alias == id);

            return profile != null ? (IActionResult)Ok(profile) : (IActionResult)NotFound();
        }
    }   
}