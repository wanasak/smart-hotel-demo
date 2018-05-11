using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Discount.Services;

namespace SmartHotel.Services.Discount.Controllers
{
    [Route("[controller]")]
    public class DiscountController : Controller
    {
        private readonly LoyaltyService loyaltyService;
        public DiscountController(LoyaltyService loyaltyService) => this.loyaltyService = loyaltyService;

        [Route("{userid}")]
        [HttpGet]
        public async Task<IActionResult> GetDiscountPeruser(string userid)
        {
            Loyalty loyaltyLevel = await this.loyaltyService.GetLoyaltyByCustomer(userid);

            double discount = 0.0d;
            switch (loyaltyLevel)
            {
                case Loyalty.Silver:
                    discount = 0.05d;
                    break;
                case Loyalty.Platnum:
                    discount = 0.10d;
                    break;
                case Loyalty.Latinum:
                    discount = 0.20d;
                    break;
                default:
                    break;
            }

            var discountResult = new { Discount = discount };

            return Ok(discountResult);
        }
    }
}